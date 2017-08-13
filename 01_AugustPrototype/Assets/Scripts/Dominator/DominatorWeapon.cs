using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	///<summary>
	/// 
	///</summary>
	public class DominatorWeapon : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("What to shoot")]
		[SerializeField]
		private GameObject _projectilePrefab;
		[Header("How to shoot it")]
		[SerializeField]
		private float _autoRate;
		[SerializeField]
		private float _manualRate;
		[SerializeField]
		private Transform _originPoint;
		[Header("State")]
		[SerializeField]
		private bool _canStartAttack;
		public bool IsAttacking;
		//NOTE everything reload related outside 1st prototype scope

		[Header("Audio")]
		[SerializeField]
		private AudioSource _source;
		[SerializeField]
		private AudioClip _clip;
		[SerializeField]
		private float _volume = 0.8f;
		[SerializeField]
		private float _pitchMin = 0.9f;
		[SerializeField]
		private float _pitchMax = 1.1f;

		//Generic hide solution for 1st prototype iteration
		private List<MeshRenderer> _weapontRenderers;
		private bool _isHidden;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_canStartAttack = true;
			_isHidden = true;
			GetWeaponRenderersInChildren();
			Hide();

			InitiateAudio();
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		public void BeginAttack()
		{
			if(_isHidden)
			{
				Show();
			}
			if(_canStartAttack)
			{
				StopAllCoroutines();
				IsAttacking = true;
				_canStartAttack = false;
				Invoke("ResetManualCooldown", _manualRate);
				StartCoroutine(AttackRoutine());
			}
		}

		public void EndAttack()
		{
			IsAttacking = false;
			StopAllCoroutines();
		}

		public void Hide()
		{
			IsAttacking = false;
			_isHidden = true;
			SetWeaponRenderingState(false);
			StopAllCoroutines();
		}

		public void Show()
		{
			_isHidden = false;
			SetWeaponRenderingState(true);
		}

		private void ResetManualCooldown()
		{
			_canStartAttack = true;
		}

		private IEnumerator AttackRoutine()
		{
			while(IsAttacking)
			{
				Instantiate(_projectilePrefab, _originPoint.position, _originPoint.rotation);
				PlayAudio();
				yield return new WaitForSeconds(_autoRate);
			}
			ResetManualCooldown();
			StopCoroutine(AttackRoutine());
		}

		/* --- 1st Prototype hiding solution --- */
		private void GetWeaponRenderersInChildren()
		{
			_weapontRenderers = new List<MeshRenderer>();
			for (int i = 0; i < transform.childCount; i++)
			{
				if(transform.GetChild(i).GetComponent<MeshRenderer>())
				{
					_weapontRenderers.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
				}
			}
		}

		private void SetWeaponRenderingState(bool isEnabled)
		{
			foreach (var renderer in _weapontRenderers)
			{
				renderer.enabled = isEnabled;
			}
		}

		private void InitiateAudio()
		{
			if(!_source)
			{
				_source = GetComponent<AudioSource>();
			}
			_source.clip = _clip;
			_source.volume = _volume;
			_source.playOnAwake = false;
			_source.loop = false;
		}

		private void PlayAudio()
		{
			_source.pitch = Random.Range(_pitchMin, _pitchMax);
			_source.Play();
		}
	}
}