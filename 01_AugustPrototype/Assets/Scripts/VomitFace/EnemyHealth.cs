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
		[SerializeField]
		private GameObject _deathPresentationPrefab;
		[SerializeField]
		private float _deathPoints = 0.01f;
		
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
			//Raise domination meter
			DominationMeter.instance.ApplyEnemyKill(_deathPoints);
			//I present to you: Death
			Instantiate(_deathPresentationPrefab, transform.position, transform.rotation); //TODO pooling also this
			//Done
			Remove();
		}

		private void Remove()
		{
			Destroy(gameObject); //TODO pooling
		}
	}
}