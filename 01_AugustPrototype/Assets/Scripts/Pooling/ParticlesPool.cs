using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class ParticlesPool : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private GameObject _particleObj;
		[SerializeField]
		private int _instancesInPool;
		[SerializeField]
		private int _currentIndex;

		private List<PooledParticle> _pooledParticles;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			InitiatePool();
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		public void PlaceAndPlayParticle(Vector3 position, Quaternion rotation)
		{
			_pooledParticles[_currentIndex].PlaceAndPlay(position, rotation);
			CycleIndex();
		}

		private void InitiatePool()
		{
			_pooledParticles = new List<PooledParticle>();
			for (int i = 0; i < _instancesInPool; i++)
			{
				_pooledParticles.Add(Instantiate(_particleObj, transform.position, transform.rotation, transform).GetComponent<PooledParticle>());
			}
		}

		private void CycleIndex()
		{
			if(_currentIndex == _pooledParticles.Count - 1)
			{
				_currentIndex = 0;
			}
			else
			{
				_currentIndex++;
			}
		}
	}
}