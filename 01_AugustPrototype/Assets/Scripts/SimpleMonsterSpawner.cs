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
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		private IEnumerator KeepEmComingRoutine()
		{
			while(DominationMeter.instance.CurrentValue > 0f)
			{
				var monster = Instantiate(Monster, transform.position, transform.rotation);
				monster.GetComponent<EnemyHealth>().DeathPresentation = DeathPresentationOriginal;
				yield return new WaitForSeconds(Interval - (IntervalModifier * DominationMeter.instance.CurrentValue));
			}
		}
	}
}