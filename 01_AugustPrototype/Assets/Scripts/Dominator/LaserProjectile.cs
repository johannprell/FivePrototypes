using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class LaserProjectile : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Settings")]
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _duration = 0.1f;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			Invoke("End", _duration);
		}
		
		void Update() 
		{
			
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Enemy")
			{
				other.GetComponent<EnemyHealth>().DealDamage(_damage);
			}
		}

		/* --- CUSTOM METHODS --- */
		private void End()
		{
			Destroy(gameObject);
		}
	}
}