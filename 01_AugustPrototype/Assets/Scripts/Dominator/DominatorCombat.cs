using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	///<summary>
	/// Combat is Life.
	///</summary>
	public class DominatorCombat : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Movement reference")]
		[SerializeField]
		private DominatorMovement _dominatorMovement;
		[Header("Handle weapon")]
		[SerializeField]
		private KeyCode _fireButton = KeyCode.Mouse0;
		[SerializeField]
		private DominatorWeapon _equippedWeapon;
		
		
		/* --- UNITY METHODS --- */
		void Awake() 
		{
			if(!_dominatorMovement)
			{
				_dominatorMovement = GetComponent<DominatorMovement>();
			}
			Invoke("ShowEquippedWeapon", 0.5f);	
		}
		
		void FixedUpdate() 
		{
			//Check state of the Dominator
			if(_dominatorMovement.MovementState == DominatorMovementState.Idle || _dominatorMovement.MovementState == DominatorMovementState.Walk)
			{
				//Check input
				if(Input.GetKeyDown(_fireButton))
				{
					_equippedWeapon.BeginAttack();
					return;
				}
				if(Input.GetKeyUp(_fireButton))
				{
					_equippedWeapon.EndAttack();
					return;
				}
				// ... and double check input
				if(!Input.GetKey(_fireButton))
				{
					_equippedWeapon.EndAttack();
					return;
				}
				if(Input.GetKey(_fireButton) && !_equippedWeapon.IsAttacking)
				{
					_equippedWeapon.BeginAttack();
				}
			}
		}

		void OnDisable()
		{
			_equippedWeapon.EndAttack();
		}

		/* --- CUSTOM METHODS --- */
		public void HideEquippedWeapon()
		{
			_equippedWeapon.Hide();
		}

		public void ShowEquippedWeapon()
		{
			_equippedWeapon.Show();
		}
	}
}