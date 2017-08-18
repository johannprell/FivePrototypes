using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

namespace Domination
{
	public class DominationMeter : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("State")]
		public float CurrentValue;
		public DominationState State;
		[Header("Value Settings")]
		[SerializeField]
		private float _startingValue;
		[SerializeField]
		private float _impressiveThreshold = .333f;
		[SerializeField]
		private float _dominantThreshold = .666f;
		[SerializeField]
		[Range(0f, 0.05f)]
		private float _hitReward = 0.01f;
		[SerializeField]
		[Range(0f, 0.05f)]
		private float _missPunishment = 0.01f;
		[Header("UI")]
		[SerializeField]
		private Slider _meterUI;
		[SerializeField]
		private Text _stateText;
		[Header("DebugInput")]
		[SerializeField]
		private KeyCode _plusKey = KeyCode.Period;
		[SerializeField]
		private KeyCode _minusKey = KeyCode.Comma;
		[Header("Dominator")]
		[SerializeField]
		private DominatorMovement _dominatorMovement;
		public static DominationMeter instance;
		[Header("Post Processing")]
		[SerializeField]
		private PostProcessingProfile _gameProfile;
		[SerializeField]
		private PostProcessingProfile _unworthyProfile;
		[SerializeField]
		private PostProcessingProfile _winProfile;
		[SerializeField]
		private Camera _mainCamera;
		[Header("Game outcome presentations")]
		[SerializeField]
		private GameObject _gameOverPresentation;
		[SerializeField]
		private float _gameOverTimeScale = 0.33f;
		[SerializeField]
		private GameObject _winPresentation;
		[Header("Cheat?")]
		[SerializeField]
		private bool _isCheatingBuild;

		public event System.Action OnDominationChange;
		public event System.Action<DominationState> OnDominationStateChange;

		/* --- UNITY METHODS --- */
		void Awake() 
		{
			//Simpleton
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
			}

			CurrentValue = _startingValue;

			_mainCamera = Camera.main;
			_mainCamera.GetComponent<PostProcessingBehaviour>().profile = _gameProfile;

			//Disable outcomes until needed
			_gameOverPresentation.SetActive(false);
			_winPresentation.SetActive(false);

			Time.timeScale = 1f;
		}
		
		void Update() 
		{
			#if UNITY_EDITOR
			//No cheating in built version :)
			EvaluateDebugInput();
			#endif
			#if UNITY_STANDALONE
			if(_isCheatingBuild)
			{
				EvaluateDebugInput();
			}
			#endif
			
			if(CurrentValue <= 0f)
			{
				ApplyState(DominationState.UNWORTHY);
				return;
			}
			if(CurrentValue < _impressiveThreshold)
			{
				if(State != DominationState.PUNY)
				{
					ApplyState(DominationState.PUNY);
				}
				return;
			}
			if(CurrentValue < _dominantThreshold)
			{
				if(State != DominationState.IMPRESSIVE)
				{
					ApplyState(DominationState.IMPRESSIVE);
				}
				return;
			}
			if(CurrentValue < 1f)
			{
				if(State != DominationState.DOMINANT)
				{
					ApplyState(DominationState.DOMINANT);
				}
				return;
			}
			if(CurrentValue >= 1f)
			{
				if(State != DominationState.SOVEREIGN)
				{
					ApplyState(DominationState.SOVEREIGN);
				}
			}
		}

		/* --- CUSTOM METHODS --- */
		private void EvaluateDebugInput()
		{
			if(Input.GetKeyDown(_minusKey))
			{
				ApplyValue(-0.1f);
			}
			if(Input.GetKeyDown(_plusKey))
			{
				ApplyValue(0.1f);
			}
		}

		private void ApplyState(DominationState state)
		{
			State = state;
			_stateText.text = State.ToString();
			//Other feedback stuff
			switch (state)
			{
				case DominationState.UNWORTHY:
					TriggerGameOver();
					break;
				case DominationState.PUNY:
					break;
				case DominationState.IMPRESSIVE:
					break;
				case DominationState.DOMINANT:
					break;
				case DominationState.SOVEREIGN:
					TriggerGameWin();
					break;
				default:
					break;
			}
			//Broadcast state change through event
			if(OnDominationStateChange != null)
			{
				OnDominationStateChange(state);
			}
		}

		private void ApplyValue(float value)
		{
			if(CurrentValue > 0f)
			{
				CurrentValue += value;
			}
			_meterUI.value = CurrentValue;

			if(OnDominationChange != null)
			{
				OnDominationChange();
			}
		}

		public void ApplyHitReward()
		{
			ApplyValue(_hitReward);
		}

		public void ApplyMissPunishment()
		{
			float punishment = (_missPunishment * CurrentValue);
			punishment = Mathf.Clamp(punishment, 0.0333f, 0.666f);
			ApplyValue(-punishment);
		}

		public void ApplyEnemyKill(float value)
		{
			//Scale to current difficulty

			//Apply
			ApplyValue(value);
		}

		public void ApplyDominatorHit(float value)
		{
			ApplyValue(-value);
		}

		private void TriggerGameOver()
		{
			//Trigger some stuff that happens at GameOverState
			_dominatorMovement.MovementState = DominatorMovementState.Blocked;
			_mainCamera.GetComponent<PostProcessingBehaviour>().profile = _unworthyProfile;
			_gameOverPresentation.SetActive(true);
			HideUI();
			Time.timeScale = _gameOverTimeScale;
		}

		private void HideUI()
		{
			_meterUI.gameObject.SetActive(false);
			_stateText.gameObject.SetActive(false);
		}

		private void TriggerGameWin()
		{
			//OPTION 1: trigger stuff in scene
			//_winPresentation.SetActive(true);
			//_mainCamera.GetComponent<PostProcessingBehaviour>().profile = _winProfile;
			//OPTION 2: go to different scene on win
			//SceneManager.LoadScene(0);
		}
	}

	public enum DominationState
	{
		UNWORTHY,
		PUNY,
		IMPRESSIVE,
		DOMINANT,
		SOVEREIGN
	}
}