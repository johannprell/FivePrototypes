using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class SimpleMonsterSpawner : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		public GameObject Monster;
		public GameObject DeathPresentationOriginal;
		public float Interval;
		public float IntervalModifier;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			StartCoroutine(KeepEmComingRoutine());
			DominationMeter.instance.OnDominationStateChange += ReceiveDominationStateChange;
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		private IEnumerator KeepEmComingRoutine()
		{
			while(DominationMeter.instance.CurrentValue > 0f)
			{
				var monster = Instantiate(Monster, transform.position, transform.rotation, transform);
				monster.GetComponent<EnemyHealth>().DeathPresentation = DeathPresentationOriginal;
				yield return new WaitForSeconds(Interval - (IntervalModifier * DominationMeter.instance.CurrentValue));
			}
		}

		private void ReceiveDominationStateChange(DominationState state)
		{
			if(state == DominationState.SOVEREIGN)
			{
				StopItNow();
			}
		}

		private void StopItNow()
		{
			StopAllCoroutines();
		}
	}
}