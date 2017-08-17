using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class WinPresentationManager : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Logo Parts")]
		[SerializeField]
		private GameObject _logoDo;
		[SerializeField]
		private GameObject _logoMi;
		[SerializeField]
		private GameObject _logoNa;
		[SerializeField]
		private GameObject _logoTion;
		private List<GameObject> _logoParts;
		[SerializeField]
		private GameObject _logoParent;

		[Header("Logo Anim")]
		[SerializeField]
		private Animator _logoAnimCTL;
		[SerializeField]
		private float _spinDuration = 2f;

		[Header("Big Logo")]
		[SerializeField]
		private GameObject _bigLogoParent;

		[Header("Environment")]
		[SerializeField]
		private GameObject _obstacles;
		[SerializeField]
		private GameObject _floor;
		[SerializeField]
		private GameObject _monstersParent;
		[SerializeField]
		private GameObject _player;
		[SerializeField]
		private GameObject _dominationMeter;

		[Header("Audio")]
		[SerializeField]
		private AudioSource _musicSource;
		[SerializeField]
		private AudioClip _crunchClip;
		[SerializeField]
		private float _crunchVolume = 0.7f;
		[SerializeField]
		private AudioClip _winMusicClip;
		[SerializeField]
		private float _musicVolume = 1f;

		[Header("Logo in variables")]
		[SerializeField]
		private float _logoInInterval = 1f;

		private MaterialColorSwitcher _colorSwitcher;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			
			
			//Logo parents init
			_logoParent.SetActive(true);
			_bigLogoParent.SetActive(false);
			//Color switching init
			_colorSwitcher = GetComponent<MaterialColorSwitcher>();
			//Collect and deactivate logo parts
			_logoParts = new List<GameObject>
			{
				_logoDo, _logoMi, _logoNa, _logoTion
			};
			foreach (var part in _logoParts)
			{
				part.SetActive(false);
			}

			//Music setup
			_musicSource.clip = _crunchClip;
			_musicSource.volume = _crunchVolume;
			_musicSource.loop = false;
			_musicSource.playOnAwake = false;
			_musicSource.rolloffMode = AudioRolloffMode.Linear;

			StartCoroutine(ShowLogoRoutine());
		}

		private void OnEnable()
		{
			
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		private IEnumerator ShowLogoRoutine()
		{
			_obstacles.SetActive(false);
			for (int i = 0; i < _logoParts.Count; i++)
			{
				_logoParts[i].SetActive(true);
				_musicSource.Play();
				_colorSwitcher.Iterate();
				yield return new WaitForSeconds(_logoInInterval);
			}
			_logoAnimCTL.SetBool("DoSpin", true);
			yield return new WaitForSeconds(_spinDuration);
			_logoParent.SetActive(false);
			_bigLogoParent.SetActive(true);
			_musicSource.clip = _winMusicClip;
			_musicSource.volume = _musicVolume;
			_musicSource.loop = true;
			_musicSource.Play();
			_colorSwitcher.Activate();
			//Disable most of the things
			_floor.SetActive(false);
			_monstersParent.SetActive(false);
			_player.SetActive(false);
			_dominationMeter.SetActive(false);
			StopCoroutine(ShowLogoRoutine());
		}
	}
}