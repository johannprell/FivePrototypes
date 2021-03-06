﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	///<summary>
	/// Dominator is always on the move.
	///</summary>
	public class DominatorMovement : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("General Movement")]
		public DominatorMovementState MovementState = DominatorMovementState.Idle;
		[SerializeField]
		private float _speed = 5f;	    		//Movement speed
		[SerializeField]
		private float _dominanceSpeedModifier = 3f; //Speed buff as domination meter goes up
		private Vector3 _movement;				//Movement direction
		private bool _isMovementInput;  		//Check if any movement is triggered by player
		private Rigidbody _body;				//Physics
		private float _x;
		private float _z;
		
		[Header("Turn to aim")]
		private Camera _mainCamera;				//Reference to main camera
		private int _floorMask;					//Used for mouse ray
		private float _camRayLength = 100f;		//Camera to floor ray length
		private Ray _camRay;
		private RaycastHit _camRayHit;
		[SerializeField]
		private float _aimVectorY = 1f;
		
		[Header("Combat Reference")]
		[SerializeField]
		private DominatorCombat _dominatorCombat;
		
		[Header("Dodge")]
		[SerializeField]
		private KeyCode _dodgeButton = KeyCode.Mouse1;
		[SerializeField]
		private float _dodgeDuration = 0.6f;
		[SerializeField]
		private float _dodgeSpeed = 10f;
		[SerializeField]
		private float _dodgeCooldown = 1f;
		[SerializeField]
		private bool _canDodge;
		private Vector3 _dodgeDirection;
		private CapsuleCollider _collider;

		[Header("Presentation")]
		[SerializeField]
		private GameObject _dominatorPresentation;
		[SerializeField]
		private GameObject _dodgingPresentation;
		[SerializeField]
		private GameObject _combatArmPresentation;
		[SerializeField]
		private Animator _freeArmAnimCTL;

		/* --- UNITY METHODS --- */
		void Awake() 
		{
			//Physics
			_body = GetComponent<Rigidbody>();
			_collider = GetComponent<CapsuleCollider>();

			//Aiming & raycasting
			_mainCamera = Camera.main;
			_camRay = new Ray();
			_camRayHit = new RaycastHit();
			_floorMask = LayerMask.GetMask("Floor");

			//Combat
			if(!_dominatorCombat)
			{
				_dominatorCombat = GetComponent<DominatorCombat>();
			}

			_dodgingPresentation.SetActive(false);
			_canDodge = true;
		}
		
		void Update()
		{
			if(MovementState != DominatorMovementState.Dodge && _dominatorCombat.EquippedWeaponHidden())
			{
				_dominatorCombat.ShowEquippedWeapon();
			}
		}

		void FixedUpdate() 
		{
			//Get input
			_x = Input.GetAxisRaw("Horizontal");
			_z = Input.GetAxisRaw("Vertical");
			_isMovementInput = _x != 0f || _z != 0f;
			
			switch(MovementState)
			{
				case DominatorMovementState.Idle:
					if(CheckDodgeInput() && _canDodge)
					{
						BeginDodge();
						break;
					}
					if(_isMovementInput)
					{
						MovementState = DominatorMovementState.Walk;
						Move(_x, _z);
					}
					TurnToAim();
					break;
				case DominatorMovementState.Walk:
					if(CheckDodgeInput() && _canDodge)
					{
						BeginDodge();
						break;
					}
					if(!_isMovementInput)
					{
						MovementState = DominatorMovementState.Idle;
						break;
					}
					Move(_x, _z);
					TurnToAim();
					break;
				case DominatorMovementState.Dodge:
					DodgeUpdate();
					break;
				case DominatorMovementState.Blocked:
					if(_dominatorCombat.enabled)
					{
						_dominatorCombat.enabled = false;
					}
					break;
				default:
					break;
			}

		}

		/* --- CUSTOM METHODS --- */
		private bool CheckDodgeInput()
		{
			bool result = Input.GetKeyDown(_dodgeButton);
			if(result)
			{
				return result;
			}
			if(Input.GetKey(_dodgeButton) && MovementState != DominatorMovementState.Dodge)
			{
				result = true;
			}
			return result;
		}

		private void BeginDodge()
		{
			_canDodge = false;
			_collider.enabled = false; //safe while dodging!
			MovementState = DominatorMovementState.Dodge;
			Invoke("EndDodge", _dodgeDuration);
			_dodgeDirection.Set(_x, 0f, _z);
			
			//Audio
			DominatorAudio.instance.PlayDodgeSound();

			//Anim
			//_freeArmAnimCTL.SetBool("DoDodge", true);

			//Representation - NOTE could be moved entirely to DominatorRepresentation script, outside of scope
			_dominatorCombat.HideEquippedWeapon();
			_dominatorPresentation.SetActive(false); //I'll be back.
			_combatArmPresentation.SetActive(false);
			_dodgingPresentation.SetActive(true);
		}

		private void DodgeUpdate()
		{
			Vector3 dodgeMovement = _dodgeDirection.normalized * (_dodgeSpeed + _dominanceSpeedModifier * DominationMeter.instance.CurrentValue) * Time.deltaTime;
			_body.MovePosition(transform.position + dodgeMovement);
		}

		private void EndDodge()
		{
			MovementState = DominatorMovementState.Idle;
			_collider.enabled = true;
			
			//Representation
			_dominatorCombat.ShowEquippedWeapon();
			_dominatorPresentation.SetActive(true); //I said I'll be back!
			_combatArmPresentation.SetActive(true);
			_dodgingPresentation.SetActive(false);

			//Anim
			//_freeArmAnimCTL.SetBool("DoDodge", false);

			Invoke("EndDodgeCooldown", _dodgeCooldown);
		}

		private void EndDodgeCooldown()
		{
			_canDodge = true;
		}

		private void Move(float x, float z)
		{
			_movement.Set(x, 0f, z);
			//Normalize vector, apply speed per second
			_movement = _movement.normalized * (_speed + _dominanceSpeedModifier * DominationMeter.instance.CurrentValue) * Time.deltaTime;
			//Move rigidbody
			_body.MovePosition(transform.position + _movement);
		}

		private void TurnToAim()
		{
			//Create a ray from the mouse cursor in the direction of the camera ? really?
			_camRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
			//Perform raycast looking for floor layer
			if(Physics.Raycast(_camRay, out _camRayHit, _camRayLength, _floorMask))
			{
				//Vector from player position to mouse ray hitpoint
				Vector3 playerToMouse = _camRayHit.point - transform.position;
				//Enforce y floor plane
				playerToMouse.y = _aimVectorY;
				//Create a quaternion based on looking down the vector from the player (arm parent) to the mouse
				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
				//Apply rotation to attack arm parent
				_body.MoveRotation(newRotation); 
			}
		}
	}

	public enum DominatorMovementState
	{
		Idle,
		Walk,
		Dodge,
		Blocked
	}
}
