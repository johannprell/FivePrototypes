using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class LaserSight : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Sight Rendering")]
		[SerializeField]
		private LineRenderer _line;
		[SerializeField]
		private Transform _lineOrigin;

		public Vector3 Origin;
		public Vector3 AimPoint;

		private RaycastHit _hit;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_hit = new RaycastHit();	
		}
		
		void Update() 
		{
			
		}

		void FixedUpdate()
		{
			Origin = _lineOrigin.position;

			
			if(Physics.Raycast(_lineOrigin.position, _lineOrigin.forward, out _hit)) //TODO mask to boundaries, penetrate everything else
			{
				AimPoint = _hit.point;
			}
			else
			{
				AimPoint = _lineOrigin.forward * 100;
			}

			_line.SetPosition(0, Origin);
			_line.SetPosition(1, AimPoint);

			//TODO visuals when shooting
		}

		/* --- CUSTOM METHODS --- */
	}
}