using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class AudioOneShotPool : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Dominator")]
		public AudioSource GunSource;
		[Range(0f, 1f)]
		public float GunVolume = 1f;
		public AudioClip SmallGunClip;
		public AudioClip BigGunClip;
		public AudioClip HugeGunClip;
		

		//Simpleton instance
		public static AudioOneShotPool instance;
		
		/* --- UNITY METHODS --- */
		private void Awake()
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

			InitiateGunSource();
		}

		void Start() 
		{
			
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		private void InitiateGunSource()
		{
			GunSource.volume = GunVolume;
			GunSource.playOnAwake = false;
			GunSource.loop = false;
			GunSource.rolloffMode = AudioRolloffMode.Linear;
		}

		public void LoadGunClip(WeaponType type)
		{
			switch(type)
			{
				case WeaponType.SmallGun:
					GunSource.clip = SmallGunClip;
					break;
				case WeaponType.BigGun:
					GunSource.clip = BigGunClip;
					break;
				case WeaponType.HugeGun:
					GunSource.clip = HugeGunClip;
					break;
				case WeaponType.None:
					break;
			}
		}

		public void PlayGunOneShot()
		{
			GunSource.Play();
		}
	}
}