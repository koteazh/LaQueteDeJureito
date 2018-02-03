using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public enum WeaponType { Sword, Dagger, Bow, Magic }
	public enum WeaponQuality { Poor, Common, Excellent }

	public WeaponType type;
	public WeaponQuality quality;
	public int damage { get; private set; }
	public int precision { get; private set; }
	public string statRequirementName  { get; private set; }
	public int statRequirementValue  { get; private set; }

	void Start()
	{
		switch (type)
		{
		case WeaponType.Sword:
			switch (quality)
			{
			case WeaponQuality.Poor:
				damage = 5;
				precision = 80;
				statRequirementName = "Strength";
				statRequirementValue = 15;
				break;

			case WeaponQuality.Common:
				damage = 7;
				precision = 77;
				statRequirementName = "Strength";
				statRequirementValue = 20;
				break;

			case WeaponQuality.Excellent:
				damage = 10;
				precision = 73;
				statRequirementName = "Strength";
				statRequirementValue = 25;
				break;
			}
			break;

		case WeaponType.Dagger:
			switch (quality)
			{
			case WeaponQuality.Poor:
				damage = 3;
				precision = 90;
				statRequirementName = "Dexterity";
				statRequirementValue = 20;
				break;

			case WeaponQuality.Common:
				damage = 6;
				precision = 87;
				statRequirementName = "Dexterity";
				statRequirementValue = 25;
				break;

			case WeaponQuality.Excellent:
				damage = 9;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 30;
				break;
			}
			break;

		case WeaponType.Bow:
			switch (quality)
			{
			case WeaponQuality.Poor:
				damage = 4;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 25;
				break;

			case WeaponQuality.Common:
				damage = 8;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 30;
				break;

			case WeaponQuality.Excellent:
				damage = 12;
				precision = 85;
				statRequirementName = "Dexterity";
				statRequirementValue = 35;
				break;
			}
			break;

		case WeaponType.Magic:
			switch (quality)
			{
			case WeaponQuality.Poor:
				damage = 5;
				precision = 80;
				statRequirementName = "Strength";
				statRequirementValue = 20;
				break;

			case WeaponQuality.Common:
				damage = 7;
				precision = 77;
				statRequirementName = "Strength";
				statRequirementValue = 25;
				break;

			case WeaponQuality.Excellent:
				damage = 10;
				precision = 73;
				statRequirementName = "Strength";
				statRequirementValue = 30;
				break;
			}
			break;
		}
	}
}
