using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitter : MonoBehaviour 
{
	/* --- MEMBER VARIABLES --- */
	public KeyCode QuitKey = KeyCode.Escape;
	
	/* --- UNITY METHODS --- */
	void Start() 
	{
		
	}
	
	void Update() 
	{
		if(Input.GetKey(QuitKey))
		{
			Application.Quit();
		}
	}

	/* --- CUSTOM METHODS --- */
}
