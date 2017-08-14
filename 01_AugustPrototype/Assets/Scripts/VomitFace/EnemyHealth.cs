using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		}

		public void DealDamage(float amount)
		{
			_currentHealth -= amount;
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
	}
}