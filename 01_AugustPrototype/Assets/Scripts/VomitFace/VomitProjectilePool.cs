using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class VomitProjectilePool : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Pooling")]
		[SerializeField]
		private GameObject _prefab;
		[SerializeField]
		private int _capacity;
		[SerializeField]
		private Vector3 _hiddenPosition;
		private Queue<VomitProjectile> _pool;

		public static VomitProjectilePool instance;
		
		/* --- UNITY METHODS --- */
		void Awake()
		{
			//Simpleton pattern
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
			}
			Initiate();
		}

		void Start() 
		{

		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		private void Initiate()
		{
			_pool = new Queue<VomitProjectile>();
			for (int i = 0; i < _capacity; i++)
			{
				_pool.Enqueue(Instantiate(_prefab, _hiddenPosition, Quaternion.identity, transform).GetComponent<VomitProjectile>());
			}
			foreach (var projectile in _pool)
			{
				projectile.AttachToPool(this);
			}
		}

		public void LaunchProjectile(Vector3 position, Quaternion rotation)
		{
			var projectile = _pool.Dequeue();
			projectile.Launch(position, rotation);
		}

		public void EnqueueInPool(VomitProjectile projectile)
		{
			_pool.Enqueue(projectile);
			projectile.gameObject.transform.position = _hiddenPosition;
		}
	}
}