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
		private Quaternion _originalRotation; //...and original rotation
		[Header("Rotate")]
		[SerializeField]
		private KeyCode _rotateLeftKey = KeyCode.Q;
		[SerializeField]
		private KeyCode _rotateRightKey = KeyCode.E;
		
		/* --- UNITY METHODS --- */
		void Start () 
		{
			_offset = transform.position - _target.position;
			_originalRotation = transform.rotation;
		}
		
		void FixedUpdate () 
		{
			//New target position is target position with offset
			Vector3 targetCameraPosition = _target.position + _offset;
			
			//Check for manual rotation, else lerp-follow
			if(Input.GetKey(_rotateLeftKey))
			{
				transform.RotateAround(_target.position, Vector3.up, -50 * Time.deltaTime);
			}
			else if(Input.GetKey(_rotateRightKey))
			{
				transform.RotateAround(_target.position, Vector3.up, 50 * Time.deltaTime);
			}
			else
			{
				//Smoothly interpolate
				transform.position = Vector3.Lerp(transform.position, targetCameraPosition, _smoothing * Time.deltaTime);
				transform.rotation = Quaternion.Lerp(transform.rotation, _originalRotation, _smoothing * Time.deltaTime);
			}
		}

		/* --- CUSTOM METHODS --- */
	}
}