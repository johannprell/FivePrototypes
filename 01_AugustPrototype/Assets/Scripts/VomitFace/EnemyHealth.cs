using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Domination
{
	public class EnemyHealth : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private float _maxHealth = 10f;
		[SerializeField]
		private float _currentHealth;
		//Assign death presentation from spawner
		public GameObject DeathPresentation;
		[SerializeField]
		private float _deathReward = 0.01f;
		[SerializeField]
		private float _deathRewardModifier = 0.008f;
		//References for becoming docile when player wins
		private EnemyShooting _shooting;
		private VomitFaceMovement _movement;
		private CapsuleCollider _collider;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			Initiate();
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		public void Initiate()
		{
			_currentHealth = _maxHealth;
			_shooting = GetComponent<EnemyShooting>();
			_movement = GetComponent<VomitFaceMovement>();
			_collider = GetComponent<CapsuleCollider>();

			DominationMeter.instance.OnDominationStateChange += ReceiveDominationStateChange;
		}

		public void DealDamage(float amount)
		{
			_currentHealth -= amount;
			DominationMeter.instance.ApplyHitReward();
			if(_currentHealth <= 0f)
			{
				Death();
			}
		}

		private void Death()
		{
			float modifiedReward = _deathReward - _deathRewardModifier * DominationMeter.instance.CurrentValue;
			//Raise domination meter
			DominationMeter.instance.ApplyEnemyKill(modifiedReward);
			//I present to you: Death
			Instantiate(DeathPresentation, transform.position, transform.rotation); //TODO pooling also this
			//Done
			Remove();
		}

		private void Remove()
		{
			Destroy(gameObject); //TODO pooling
		}

		private void ReceiveDominationStateChange(DominationState state)
		{
			if(state == DominationState.SOVEREIGN)
			{
				BecomeDocileOnPlayerWin();
			}
		}

		private void BecomeDocileOnPlayerWin()
		{
			_movement.StopMoving();
			_movement.enabled = false;
			_shooting.StopShooting();
			_shooting.enabled = false;
			//_collider.enabled = false; //Probably not needed
		}
	}
}