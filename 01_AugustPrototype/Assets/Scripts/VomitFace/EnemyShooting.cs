using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class EnemyShooting : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("What to shoot")]
		public GameObject BulletPrefab;
		public Transform OriginPoint;
		[Header("How to shoot it")]
		public float ShootInterval = 0.2f;
		[SerializeField]
		private float _intervalDominationModifer = 0.1f;

		private Transform _player;
		[SerializeField]
		private bool _hasAimOnPlayer;
		[SerializeField]
		private bool _canShoot;
		
		private Ray _ray;
		private RaycastHit _hit;
		private int _mask;
		[SerializeField]
		private float _targetingRange = 10f;

		[Header("Melee")]
		[SerializeField]
		private float _meleeDamage = 0.02f;
		[SerializeField]
		private float _meleeModifier = 0.05f;

		/* --- UNITY METHODS --- */
		void Start () 
		{
			_player = GameObject.FindWithTag("Player").GetComponent<Transform>();

			_ray = new Ray();
			_hit = new RaycastHit();
			_mask = LayerMask.NameToLayer("Player");

			_canShoot = true;
		}
		
		void FixedUpdate () 
		{
			if(Physics.Raycast(transform.position, transform.forward, out _hit))
			{
				if(!_hasAimOnPlayer && _canShoot && _hit.transform.tag == "Player")
				{
					_hasAimOnPlayer = true;
					StartCoroutine(ShootRoutine());
				}
				else if(_hasAimOnPlayer && _hit.transform.tag != "Player")
				{
					_hasAimOnPlayer = false;
					StopAllCoroutines();
				}
			}
			else
			{
				if(_hasAimOnPlayer)
				{
					_hasAimOnPlayer = false;
					StopAllCoroutines();
				}
			}
		}

		void OnCollisionEnter(Collision col)
		{
			if(col.transform.tag == "Player")
			{
				DominationMeter.instance.ApplyDominatorHit(_meleeDamage + (_meleeModifier * DominationMeter.instance.CurrentValue));
			}
		}

		/* --- CUSTOM METHODS --- */
		private IEnumerator ShootRoutine()
		{
			while(_hasAimOnPlayer)
			{
				if(_canShoot)
				{
					Shoot();
				}
				yield return true;
			}
			StopCoroutine(ShootRoutine());
		}

		private void Shoot()
		{
			Instantiate(BulletPrefab, OriginPoint.position, OriginPoint.rotation);
			_canShoot = false;
			Invoke("StopShootingCooldown", ShootInterval - _intervalDominationModifer * DominationMeter.instance.CurrentValue);
		}

		private void StopShootingCooldown()
		{
			_canShoot = true;
		}

		public void StopShooting()
		{
			StopAllCoroutines();
		}
	}
}