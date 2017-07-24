using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class CameraFollow : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private Transform _target;			//Position the camera will follow
		[SerializeField]
		private float _smoothing = 5f;		//Camera speed
		private Vector3 _offset;			//Store initial offset from target
		
		/* --- UNITY METHODS --- */
		void Start () 
		{
			_offset = transform.position - _target.position;
		}
		
		void FixedUpdate () 
		{
			//New target position is target position with offset
			Vector3 targetCameraPosition = _target.position + _offset;
			//Smoothly interpolate
			transform.position = Vector3.Lerp(transform.position, targetCameraPosition, _smoothing * Time.deltaTime);
		}

		/* --- CUSTOM METHODS --- */
	}
}