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
		[Header("Audio")]
		[SerializeField]
		private AudioClip _stepSound;
		[SerializeField]
		private AudioSource _stepSource;
		[SerializeField]
		private float _pitchMin;
		[SerializeField]
		private float _pitchMax;
		[SerializeField]
		[Range(0f, 1f)]
		private float _stepVolume;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_walkingPants.SetActive(false);
			_standingPants.SetActive(true);

			_stepSource.clip = _stepSound;
		}
		
		void Update() 
		{
			if(_movement.MovementState == DominatorMovementState.Walk && !_isAnimating)
			{
				_walkingPants.SetActive(true);
				_standingPants.SetActive(false);
				_isAnimating = true;
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

		/* --- CUSTOM METHODS --- */
		private IEnumerator WalkRoutine()
		{
			while(_isAnimating)
			{
				FlipPants();
				_particles.Emit(_particleEmission);
				PlayAudio();
				yield return new WaitForSeconds(_stepRate);
			}
		}

		private void FlipPants()
		{
			transform.localRotation = _isPantsFlipped ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
			_isPantsFlipped = !_isPantsFlipped;
		}

		private void PlayAudio()
		{
			_stepSource.pitch = Random.Range(_pitchMin, _pitchMax);
			_stepSource.Play();
		}
	}
}