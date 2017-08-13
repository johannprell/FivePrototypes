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

		[SerializeField]
		private GameObject _smallGunMuzzleObject;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			//Simpleton pattern
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
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