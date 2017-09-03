using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class LaserSight : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("General Line settings")]
		[SerializeField]
		private LineRenderer _line;
		[SerializeField]
		private Transform _lineOrigin;

		public Vector3 Origin;
		public Vector3 AimPoint;

		[Header("Visualize Sight")]
		[SerializeField]
		private float _sightWidth;
		[SerializeField]
		private Color _sightColor;

		[Header("Visualize Ray shot")]
		[SerializeField]
		private float _rayWidth;
		[SerializeField]
		private Color _rayStartColor;
		[SerializeField]
		private Color _rayEndColor;
		[SerializeField]
		private float _duration;

		[Header("Weapon Script")]
		[SerializeField]
		private DominatorWeapon _weapon;

		private RaycastHit _hit;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_hit = new RaycastHit();	
			VisualizeSight();
		}

		void OnEnable()
		{
			_weapon.OnShoot += VisualizeShot;
		}

		void OnDisable()
		{
			_weapon.OnShoot -= VisualizeShot;
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
		private void VisualizeShot()
		{
			SetLineProperties(_rayStartColor, _rayEndColor, _rayWidth, _rayWidth);
			Invoke("VisualizeSight", _duration);
		}

		private void VisualizeSight()
		{
			SetLineProperties(_sightColor, _sightColor, _sightWidth, _sightWidth);
		}

		private void SetLineProperties(Color startColor, Color endColor, float startWidth, float endWidth)
		{
			_line.startColor = startColor;
			_line.endColor = endColor;
			_line.startWidth = startWidth;
			_line.endWidth = endWidth;
		}

	}
}