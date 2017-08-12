using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

		public static DominationMeter instance;
		
		/* --- UNITY METHODS --- */
		void Start() 
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

			ApplyValue(_startingValue);
		}
		
		void Update() 
		{
			#if UNITY_EDITOR
			EvaluateDebugInput();
			#endif
			
			if(CurrentValue <= 0f)
			{
				ApplyState(DominationState.UNWORTHY);
				TriggerGameOver();
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
		}

		private void ApplyValue(float value)
		{
			//TODO will need some logic here to make it interesting!
			CurrentValue += value;
			_meterUI.value = CurrentValue;
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

		private void TriggerGameOver()
		{
			//Trigger some stuff that happens at GameOverState

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