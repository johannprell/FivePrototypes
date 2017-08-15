	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class VomitProjectile : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
			[SerializeField]
			private float _force;
			[SerializeField]
			private float _forceDominationModifier;
			[SerializeField]
			private float _damage;
			[SerializeField]
			private float _lifeTime = 5f;
			private Rigidbody _body;
			[SerializeField]
			private GameObject _impactObject;

			
			/* --- UNITY METHODS --- */
			void Start() 
			{
				_body = GetComponent<Rigidbody>();
				Invoke("SelfDestruct", _lifeTime);

				Launch();
			}
			
			void Update() 
			{
				
			}

			private void OnTriggerEnter(Collider other)
			{
				switch(other.tag)
				{
					case "Player":
						DominationMeter.instance.ApplyDominatorHit(_damage);
						Impact();
						break;
					case "Obstacle":
						Impact();
						break;
					default:
						break;
				}
			}

			// private void OnCollisionEnter(Collision col)
			// {
			// 	switch(col.transform.tag)
			// 	{
			// 		case "Player":
			// 			DominationMeter.instance.ApplyDominatorHit(_damage);
			// 			Impact();
			// 			break;
			// 		case "Obstacle":
			// 			Impact();
			// 			break;
			// 		default:
			// 			break;
			// 	}
			// }

			/* --- CUSTOM METHODS --- */
			private void Launch()
			{
				float dominationClamped = Mathf.Clamp(DominationMeter.instance.CurrentValue, 0.10f, 1f);
				float modifiedForce = _force + _forceDominationModifier * dominationClamped;
				_body.AddForce(transform.forward * modifiedForce, ForceMode.Impulse);
			} 

			private void SelfDestruct()
			{
				Impact();
			}

			private void Remove()
			{
				Destroy(gameObject); //TODO pooling
			}

			private void Impact()
			{
				Instantiate(_impactObject, transform.position, transform.rotation); //TODO le pooling
				Remove();
			}
	}
}