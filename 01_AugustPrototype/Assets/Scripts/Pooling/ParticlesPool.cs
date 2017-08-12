using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class ParticlesPool : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[SerializeField]
		private Vector3 _hiddenPos;

		public static ParticlesPool instance;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			//Simpleton
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		public void PlaceParticle(ParticleType type)
		{

		}
	}

	public enum ParticleType
	{
		SmallGunMuzzle,
		SmallGunImpact,
		BigGunMuzzle,
		BigGunImpact,
		HugeGunMuzzle,
		HugeGunImpact
	}
}