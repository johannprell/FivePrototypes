using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class DominatorAudio : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		//Static simpleton instace
		public static DominatorAudio instance;

		[Header("Pitch Randomization")]
		[SerializeField]
		private float _pitchFactorMin = 0.9f;
		[SerializeField]
		private float _pitchFactorMax = 1.1f;

		[Header("Walk")]
		[SerializeField]
		private AudioSource _walkSource;
		[SerializeField]
		private AudioClip _walkClip;
		[SerializeField]
		private float _walkVolume;
		[SerializeField]
		private float _walkBasePitch = 1f;

		[Header("Dodge")]
		[SerializeField]
		private AudioSource _dodgeSource;
		[SerializeField]
		private AudioClip _dodgeClip;
		[SerializeField]
		private float _dodgeVolume;
		[SerializeField]
		private float _dodgeBasePitch = 1f;

		private List<AudioSource> _allSources;

		/* --- UNITY METHODS --- */
		void Start() 
		{
			InitiateSources();

			//Simpleton pattern
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
			}
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		
		public void PlayWalkSound()
		{
			PlayWithRandomizedPitch(_walkSource, _walkBasePitch);
		}

		public void PlayDodgeSound()
		{
			PlayWithRandomizedPitch(_dodgeSource, _dodgeBasePitch);
		}

		private void PlayWithRandomizedPitch(AudioSource source, float basePitch)
		{
			source.pitch = Random.Range(basePitch * _pitchFactorMin, basePitch * _pitchFactorMax);
			source.Play();
		}

		private void InitiateSources()
		{
			_allSources = new List<AudioSource>
			{
				_walkSource,
				_dodgeSource
			};
			//Configure all sources as loud one-shots
			foreach (var source in _allSources)
			{
				source.playOnAwake = false;
				source.loop = false;
				source.rolloffMode = AudioRolloffMode.Linear;
			}
			//Set clips and volumes
			_walkSource.clip = _walkClip;
			_walkSource.volume = _walkVolume;
			_dodgeSource.clip = _dodgeClip;
			_dodgeSource.volume = _dodgeVolume;
		}

	}
}