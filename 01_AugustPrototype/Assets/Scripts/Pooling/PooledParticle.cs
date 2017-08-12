using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class PooledParticle : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		public ParticleType Type;
		private ParticleSystem _particles;
		[SerializeField]
		private float _autoResetAfter = 3f;
		private Vector3 _resetPosition;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_particles = GetComponent<ParticleSystem>();
			_particles.loop = false; //Assuming one-shot particles will be pooled
			_particles.playOnAwake = false;

			_resetPosition = transform.position;
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		public void PlaceAndPlay(Vector3 position, Quaternion rotation)
		{
			transform.position = position;
			transform.rotation = rotation;
			_particles.Play();

			Invoke("Reset", _autoResetAfter);
		}

		public void Reset()
		{
			_particles.Stop();
			transform.position = _resetPosition;
			transform.rotation = Quaternion.identity;
		}
	}
}