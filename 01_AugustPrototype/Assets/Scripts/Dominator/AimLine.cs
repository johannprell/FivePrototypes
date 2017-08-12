using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class AimLine : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		private LineRenderer _line;
		private RaycastHit _hit;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_line = GetComponent<LineRenderer>();
			_hit = new RaycastHit();
		}
		
		void FixedUpdate() 
		{
			if(Physics.Raycast(transform.position, transform.forward, out _hit))
			{
				_line.SetPosition(0, transform.position);
				_line.SetPosition(1, _hit.point);
			}
		}

		/* --- CUSTOM METHODS --- */
	}
}