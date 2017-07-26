using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class DominatorProjectile : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private float _force;
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _lifeTime = 5f;
		private Rigidbody _body;
		[SerializeField]
		private GameObject _impactObject;
		[SerializeField]
		private bool _enemyHit;

		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_body = GetComponent<Rigidbody>();
			Invoke("Remove", _lifeTime);

			Launch();
		}
		
		void Update() 
		{
			
		}

		private void OnTriggerEnter(Collider other)
		{
			switch(other.tag)
			{
				case "Enemy":
					_enemyHit = true;
					other.GetComponent<EnemyHealth>().DealDamage(_damage);
					Impact();
					break;
				case "Obstacle":
					Impact();
					break;
				default:
					break;
			}
		}

		/* --- CUSTOM METHODS --- */
		private void Launch()
		{
			float updatedForce = _force; //TODO adjust to Domination meter value
			_body.AddForce(transform.forward * updatedForce, ForceMode.Impulse);
		} 

		private void Remove()
		{
			Destroy(gameObject); //TODO pooling
		}

		private void Impact()
		{
			switch(_enemyHit) //TODO domination meter logic
			{
				case true:
					break;
				case false:
					break;
			}
			Instantiate(_impactObject, transform.position, transform.rotation); //TODO le pooling
			Remove();
		}
	}
}