using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public enum E_WeaponType { Sword, Dagger, Bow, Magic }
	public enum E_WeaponQuality { Poor, Common, Excellent }

	public E_WeaponType weaponType;
	public E_WeaponQuality quality;
	public int damage { get; private set; }
	public int precision { get; private set; }
	public string statRequirementName  { get; private set; }
	public int statRequirementValue  { get; private set; }

	public Weapon GetWeapon(E_WeaponType _weaponType, E_WeaponQuality _quality)
	{
		weaponType = _weaponType;
		quality = _quality;
		switch (weaponType)
		{
		case E_WeaponType.Sword:
			switch (quality)
			{
			case E_WeaponQuality.Poor:
				damage = 5;
				precision = 80;
				statRequirementName = "Strength";
				statRequirementValue = 15;
				break;

			case E_WeaponQuality.Common:
				damage = 7;
				precision = 77;
				statRequirementName = "Strength";
				statRequirementValue = 20;
				break;

			case E_WeaponQuality.Excellent:
				damage = 10;
				precision = 73;
				statRequirementName = "Strength";
				statRequirementValue = 25;
				break;
			}
			break;

		case E_WeaponType.Dagger:
			switch (quality)
			{
			case E_WeaponQuality.Poor:
				damage = 3;
				precision = 90;
				statRequirementName = "Dexterity";
				statRequirementValue = 20;
				break;

			case E_WeaponQuality.Common:
				damage = 6;
				precision = 87;
				statRequirementName = "Dexterity";
				statRequirementValue = 25;
				break;

			case E_WeaponQuality.Excellent:
				damage = 9;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 30;
				break;
			}
			break;

		case E_WeaponType.Bow:
			switch (quality)
			{
			case E_WeaponQuality.Poor:
				damage = 4;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 25;
				break;

			case E_WeaponQuality.Common:
				damage = 8;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 30;
				break;

			case E_WeaponQuality.Excellent:
				damage = 12;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 35;
				break;
			}
			break;

		case E_WeaponType.Magic:
			switch (quality)
			{
			case E_WeaponQuality.Poor:
				damage = 5;
				precision = 80;
				statRequirementName = "Strength";
				statRequirementValue = 20;
				break;

			case E_WeaponQuality.Common:
				damage = 7;
				precision = 77;
				statRequirementName = "Strength";
				statRequirementValue = 25;
				break;

			case E_WeaponQuality.Excellent:
				damage = 10;
				precision = 73;
				statRequirementName = "Strength";
				statRequirementValue = 30;
				break;
			}
			break;
		}
		return (this);
	}

	public E_WeaponType GetWeaponType()
	{
		return (weaponType);
	}

	public E_WeaponQuality GetWeaponQuality()
	{
		return (quality);
	}
}
