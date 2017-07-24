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
	}
}