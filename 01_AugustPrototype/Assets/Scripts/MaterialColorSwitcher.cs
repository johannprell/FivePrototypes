using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	///<summary>
	/// All the colors
	///</summary>
	public class MaterialColorSwitcher : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private Material _material;
		[SerializeField]
		private List<Color> _colors;
		[SerializeField]
		private float _interval = 0.1f;
		[SerializeField]
		private bool _isActive;

		/* --- UNITY METHODS --- */
		void Start() 
		{
			
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		public void OneShot()
		{
			_material.color = _colors[Random.Range(0, _colors.Count)];
		}

		public void Activate()
		{
			_isActive = true;
			StartCoroutine(SwitchRoutine());
		}

		private IEnumerator SwitchRoutine()
		{
			while(_isActive)
			{
				OneShot();
				yield return new WaitForSeconds(_interval);
			}
			StopCoroutine(SwitchRoutine());
		}
	}
}