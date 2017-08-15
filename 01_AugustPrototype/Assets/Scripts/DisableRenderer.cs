using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class DisableRenderer : MonoBehaviour 
	{
		/* --- UNITY METHODS --- */
		void Awake() 
		{
			MakeInvisible();
		}
		
		void OnEnable() 
		{
			MakeInvisible();
		}

		/* --- CUSTOM METHODS --- */
		private void MakeInvisible()
		{
			GetComponent<MeshRenderer>().enabled = false;
		}
	}
}