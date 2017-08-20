using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class DominatorShell : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Damage")]
		[SerializeField]
		private float _baseDamage = 5f;
		[SerializeField]
		private float _distanceModifier = 0.5f;
		[SerializeField]
		private Transform _damageOrigin;

		[Header("Lifetime")]
		[SerializeField]
		float _destroyAfter = 0.4f;

		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			Invoke("Done", _destroyAfter);
		}

		void Update() 
		{
			
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Enemy")
			{
				float damage = _baseDamage - (Vector3.Distance(other.transform.position, _damageOrigin.position) * _distanceModifier);
				Debug.Log("Shotgun shell dealt damage: " + damage);
				other.GetComponent<EnemyHealth>().DealDamage(damage);
			}
		}

		/* --- CUSTOM METHODS --- */
		private void Done()
		{
			//This is parented under origin object with particle fx, so destroy parent
			Destroy(transform.parent.gameObject);
		}
	}
}