using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domination
{
	public class GameOverScreen : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		public float RestartDelay = 1f;
		public bool CanRestart = false;
		public Canvas GameOverCanvas;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			GameOverCanvas.enabled = false;
		}

		void OnEnable()
		{
			Invoke("EnableRestart", RestartDelay);
		}

		void Update() 
		{
			if(Input.anyKey && CanRestart)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}

		/* --- CUSTOM METHODS --- */
		private void EnableRestart()
		{
			CanRestart = true;
			GameOverCanvas.enabled = true;
		}
	}
}