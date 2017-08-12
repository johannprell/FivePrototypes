﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Domination
{
	///<summary>
	/// Intro logo and start prompt
	///</summary>
	public class IntroManager : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Logo Objects")]
		[SerializeField]
		private GameObject _logoParent;
		[SerializeField]
		private GameObject _logoDo;
		[SerializeField]
		private GameObject _logoMi;
		[SerializeField]
		private GameObject _logoNa;
		[SerializeField]
		private GameObject _logoTion;

		private List<GameObject> _logoParts;

		[Header("LogoVariables")]
		[SerializeField]
		private float _logoScaleMin = 1f;
		[SerializeField]
		private float _logoScaleMax = 1.4f;
		[SerializeField]
		private float _logoRevealInterval = 0.5f;
		[Header("Prompt")]
		[SerializeField]
		private Text _promptText;
		[SerializeField]
		private string _promptCopy = "Press any key to start";

		private bool _isPromptVisible;

		[Header("Logo colors")]
		[SerializeField]
		private MaterialColorSwitcher _colorSwitcher;

		[Header("Instructions")]
		[SerializeField]
		private GameObject _instructions;
		[SerializeField]
		private float _instructionsDuration = 3f;

		[Header("Audio")]
		public AudioClip CrunchClip;
		[Range(0f, 1f)]
		public float CrunchVolume = 0.8f;
		public AudioClip RiffClip;
		[Range(0f, 1f)]
		public float RiffVolume = 1f;
		public AudioClip ShtapClip;
		[Range(0f, 1f)]
		public float ShtapVolume = 1f;
		public AudioSource MusicSource;

		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			//Initiate stuff we'll need later
			_isPromptVisible = false;
			_promptText.text = "";
			_instructions.SetActive(false);
			MusicSource.playOnAwake = false;
			//Make sure logo parent is active
			_logoParent.SetActive(true);
			//Set logo to min scale
			_logoParent.transform.localScale = new Vector3(_logoScaleMin, _logoScaleMin, _logoScaleMin);
			//Collect and disable logo part objects
			_logoParts = new List<GameObject>
			{
				_logoDo, _logoMi, _logoNa, _logoTion
			};
			foreach(var part in _logoParts)
			{
				part.SetActive(false);
			}
			//Begin
			StartCoroutine(LogoSequence());
		}
		
		void Update() 
		{
			if(_isPromptVisible && Input.anyKey)
			{
				StartGame();
			}	
		}

		/* --- CUSTOM METHODS --- */
		private IEnumerator LogoSequence()
		{
			//Prepare audio
			MusicSource.volume = CrunchVolume;
			MusicSource.clip = CrunchClip;
			MusicSource.loop = false;
			//Reveal each logo part
			foreach(var part in _logoParts)
			{
				yield return new WaitForSeconds(_logoRevealInterval);
				part.SetActive(true);
				//Switch color once
				_colorSwitcher.OneShot();
				MusicSource.Play();
			}
			yield return new WaitForSeconds(_logoRevealInterval * 1.6f); //Extra pause before impact
			//Prepare audio
			MusicSource.volume = RiffVolume;
			MusicSource.clip = RiffClip;
			MusicSource.loop = true;
			//Scale up logo
			_logoParent.transform.localScale = new Vector3(_logoScaleMax, _logoScaleMax, _logoScaleMax);
			//Start autoswitching
			_colorSwitcher.Activate();
			//Play Riff
			MusicSource.Play();
			//Prompt and done
			ShowPrompt();
			StopCoroutine(LogoSequence());
		}

		private void ShowPrompt()
		{
			_isPromptVisible = true;
			_promptText.text = _promptCopy;
		}

		private void StartGame()
		{
			_isPromptVisible = false;
			_promptText.text = "";
			StartCoroutine(StartGameSequence());
		}

		private IEnumerator StartGameSequence()
		{
			_logoParent.SetActive(false);
			//Handle audio
			MusicSource.Stop();
			MusicSource.volume = ShtapVolume;
			MusicSource.clip = ShtapClip;
			MusicSource.loop = true;
			MusicSource.Play();
			//Show intstructions and begin countdown
			_instructions.SetActive(true);
			int countdown = 3;
			while(countdown > 0)
			{
				_promptText.text = countdown.ToString();
				yield return new WaitForSeconds(1f);
				countdown--;
			}

			SceneManager.LoadScene(1);
			StopCoroutine(StartGameSequence());
		}
	}
}