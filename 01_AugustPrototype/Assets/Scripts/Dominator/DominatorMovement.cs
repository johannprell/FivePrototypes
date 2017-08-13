using System.Collections;
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
		private KeyCode _dodgeButton = KeyCode.Mouse1;
		[SerializeField]
		private float _dodgeDuration = 0.6f;
		[SerializeField]
		private float _dodgeSpeed = 10f;
		private Vector3 _dodgeDirection;

		[Header("Presentation")]
		[SerializeField]
		private GameObject _dominatorPresentation;
		[SerializeField]
		private GameObject _dodgingPresentation;
		[SerializeField]
		private GameObject _combatArmPresentation;

		[Header("Audio")]
		[SerializeField]
		private AudioSource _dodgeSource;
		[SerializeField]
		private float _pitchMin;
		[SerializeField]
		private float _pitchMax;

		/* --- UNITY METHODS --- */
		void Awake() 
		{
			//Physics
			_body = GetComponent<Rigidbody>();

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
					if(CheckDodgeInput())
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
					if(CheckDodgeInput())
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
			MovementState = DominatorMovementState.Dodge;
			Invoke("EndDodge", _dodgeDuration);
			_dodgeDirection.Set(_x, 0f, _z);
			
			//Audio
			_dodgeSource.Play();

			//Representation - NOTE could be moved entirely to DominatorRepresentation script, outside of scope
			_dominatorCombat.HideEquippedWeapon();
			_dominatorPresentation.SetActive(false); //I'll be back.
			_combatArmPresentation.SetActive(false);
			_dodgingPresentation.SetActive(true);
		}

		private void DodgeUpdate()
		{
			Vector3 dodgeMovement = _dodgeDirection.normalized * _dodgeSpeed * Time.deltaTime;
			_body.MovePosition(transform.position + dodgeMovement);
		}

		private void EndDodge()
		{
			MovementState = DominatorMovementState.Idle;
			
			//Representation
			_dominatorCombat.ShowEquippedWeapon();
			_dominatorPresentation.SetActive(true); //I said I'll be back!
			_combatArmPresentation.SetActive(true);
			_dodgingPresentation.SetActive(false);
		}

		private void Move(float x, float z)
		{
			_movement.Set(x, 0f, z);
			//Normalize vector, apply speed per second
			_movement = _movement.normalized * _speed * Time.deltaTime;
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
