using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class WeaponSwitcher : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Equipped")]
		public WeaponType CurrentWeapon;
		[Header("Gun Objects")]
		public GameObject SmallGunObj;
		public GameObject BigGunObj;
		public GameObject HugeGunObj;
		private Dictionary<WeaponType, GameObject> _weapons;
	
		private DominationMeter _dominationMeter;
		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			_dominationMeter = DominationMeter.instance;
			InitiateWeapons();
		}
		
		void Update() 
		{
			CheckDominationState();
		}

		/* --- CUSTOM METHODS --- */
		private void InitiateWeapons()
		{
			//Assemble references in dictionary
			_weapons = new Dictionary<WeaponType, GameObject>();
			_weapons.Add(WeaponType.SmallGun, SmallGunObj);
			_weapons.Add(WeaponType.BigGun, BigGunObj);
			_weapons.Add(WeaponType.HugeGun, HugeGunObj);
			//Disable all at start
			foreach(var weapon in _weapons)
			{
				weapon.Value.SetActive(false);
			}
		}

		private void CheckDominationState()
		{
			if(_dominationMeter.State == DominationState.PUNY && CurrentWeapon != WeaponType.SmallGun)
			{
				Equip(WeaponType.SmallGun);
				return;
			}
			if(_dominationMeter.State == DominationState.IMPRESSIVE && CurrentWeapon != WeaponType.BigGun)
			{
				Equip(WeaponType.BigGun);
				return;
			}
			if(_dominationMeter.State == DominationState.DOMINANT && CurrentWeapon != WeaponType.HugeGun)
			{
				Equip(WeaponType.HugeGun);
				return;
			}
		}

		private void Equip(WeaponType type)
		{
			//Handle gameobjects
			switch(type)
			{
				case WeaponType.SmallGun:
					_weapons[WeaponType.SmallGun].SetActive(true);
					_weapons[WeaponType.BigGun].SetActive(false);
					_weapons[WeaponType.HugeGun].SetActive(false);
					break;
				case WeaponType.BigGun:
					_weapons[WeaponType.SmallGun].SetActive(false);
					_weapons[WeaponType.BigGun].SetActive(true);
					_weapons[WeaponType.HugeGun].SetActive(false);
					break;
				case WeaponType.HugeGun:
					_weapons[WeaponType.SmallGun].SetActive(false);
					_weapons[WeaponType.BigGun].SetActive(false);
					_weapons[WeaponType.HugeGun].SetActive(true);
					break;
			}
			//Switch one shot audio clip
			AudioOneShotPool.instance.LoadGunClip(type);
		}
	}
}