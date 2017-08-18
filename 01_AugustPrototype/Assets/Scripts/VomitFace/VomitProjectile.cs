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

			private VomitProjectilePool _pool;

			
			/* --- UNITY METHODS --- */
			void Start() 
			{
				_body = GetComponent<Rigidbody>();
				

				//Launch();
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

			/* --- CUSTOM METHODS --- */
			public void AttachToPool(VomitProjectilePool pool)
			{
				_pool = pool;
			}

			public void Launch(Vector3 position, Quaternion rotation)
			{
				transform.position = position;
				transform.rotation = rotation;

				float dominationClamped = Mathf.Clamp(DominationMeter.instance.CurrentValue, 0.10f, 1f);
				float modifiedForce = _force + _forceDominationModifier * dominationClamped;
				_body.AddForce(transform.forward * modifiedForce, ForceMode.Impulse);
				
				Invoke("SelfDestruct", _lifeTime);
			} 

			private void SelfDestruct()
			{
				Impact();
			}

			private void Remove()
			{
				_body.velocity = Vector3.zero;
				_pool.EnqueueInPool(this);
			}

			private void Impact()
			{
				//Particles or other impact fx go here
				Remove();
			}
	}
}