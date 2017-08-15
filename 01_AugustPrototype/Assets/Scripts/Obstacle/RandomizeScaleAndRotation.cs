using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class RandomizeScaleAndRotation : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private float _scaleMin = 0.6f;
		[SerializeField]
		private float _scaleMax = 6f;
		[SerializeField]
		private float _rotationMaxY = 180f;
		[SerializeField] 
		private float _rotationMaxXZ = 10f;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			float x = Random.Range(0f, _rotationMaxXZ);
			float y = Random.Range(0f, _rotationMaxY);
			float z = Random.Range(0f, _rotationMaxXZ);
			transform.rotation = Quaternion.Euler(new Vector3(x, y, z));

			float scale = Random.Range(_scaleMin, _scaleMax);
			transform.localScale = new Vector3(scale, scale, scale);

			transform.position = new Vector3(transform.position.x, 1 * scale, transform.position.z);
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
	}
}