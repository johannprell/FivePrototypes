using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class DominatorPants : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Detect movement")]
		[SerializeField]
		private DominatorMovement _movement;
		[Header("Particles")]
		[SerializeField]
		private ParticleSystem _particles;
		[SerializeField]
		private int _particleEmission;
		[SerializeField]
		private int _particleEmissionOnStop;
		[Header("Animation")]
		[SerializeField]
		private GameObject _standingPants;
		[SerializeField]
		private GameObject _walkingPants;
		[SerializeField]
		private float _stepRate = 0.2f;
		[SerializeField]
		private bool _isAnimating;
		private bool _isPantsFlipped;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_walkingPants.SetActive(false);
			_standingPants.SetActive(true);
			_isAnimating = false;
		}

		void OnEnable()
		{
			_particles.Emit(_particleEmission); //Parent object the one being disabled/enabled, so not working
		}
		
		void Update() 
		{
			if(_movement.MovementState == DominatorMovementState.Walk && !_isAnimating)
			{
				_walkingPants.SetActive(true);
				_standingPants.SetActive(false);
				StartCoroutine(WalkRoutine());
			}
			else if(_movement.MovementState == DominatorMovementState.Idle && _isAnimating)
			{
				_walkingPants.SetActive(false);
				_standingPants.SetActive(true);
				_particles.Emit(_particleEmissionOnStop);
				_isAnimating = false;
				StopAllCoroutines();
			}
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			_isAnimating = false;
		}

		/* --- CUSTOM METHODS --- */
		private IEnumerator WalkRoutine()
		{
			_isAnimating = true;
			while(_isAnimating)
			{
				FlipPants();
				_particles.Emit(_particleEmission);
				DominatorAudio.instance.PlayWalkSound();
				yield return new WaitForSeconds(_stepRate);
			}
		}

		private void FlipPants()
		{
			transform.localRotation = _isPantsFlipped ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
			_isPantsFlipped = !_isPantsFlipped;
		}
	}
}