using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour {
	public enum E_ArmorType { Light, Medium, Heavy }
	public enum E_ArmorQuality { Poor, Common, Excellent }

	public E_ArmorType type;
	public E_ArmorQuality quality;
	public int physicalProtection { get; private set; }
	public int magicalProtection { get; private set; }
	public int congestion { get; private set; }
	public string statRequirementName  { get; private set; }
	public int statRequirementValue  { get; private set; }

	public Armor GetArmor(E_ArmorType _type, E_ArmorQuality _quality)
	{
		type = _type;
		quality = _quality;
		switch (type)
		{
			case E_ArmorType.Light:
				switch (quality)
				{
					case E_ArmorQuality.Poor:
						physicalProtection = 1;
						magicalProtection = 3;
						congestion = 0;
						statRequirementName = "Resistance";
						statRequirementValue = 15;
						break;

					case E_ArmorQuality.Common:
						physicalProtection = 2;
						magicalProtection = 4;
						congestion = 0;
						statRequirementName = "Resistance";
						statRequirementValue = 20;
						break;

					case E_ArmorQuality.Excellent:
						physicalProtection = 3;
						magicalProtection = 5;
						congestion = 1;
						statRequirementName = "Resistance";
						statRequirementValue = 25;
						break;
				}
				break;

			case E_ArmorType.Medium:
				switch (quality)
				{
					case E_ArmorQuality.Poor:
						physicalProtection = 2;
						magicalProtection = 2;
						congestion = 1;
						statRequirementName = "Agility";
						statRequirementValue = 20;
						break;

					case E_ArmorQuality.Common:
						physicalProtection = 3;
						magicalProtection = 3;
						congestion = 1;
						statRequirementName = "Agility";
						statRequirementValue = 25;
						break;

					case E_ArmorQuality.Excellent:
						physicalProtection = 5;
						magicalProtection = 5;
						congestion = 2;
						statRequirementName = "Agility";
						statRequirementValue = 30;
						break;
				}
				break;

			case E_ArmorType.Heavy:
				switch (quality)
				{
					case E_ArmorQuality.Poor:
						physicalProtection = 3;
						magicalProtection = 2;
						congestion = 3;
						statRequirementName = "Defense";
						statRequirementValue = 10;
						break;

					case E_ArmorQuality.Common:
						physicalProtection = 5;
						magicalProtection = 3;
						congestion = 3;
						statRequirementName = "Defense";
						statRequirementValue = 15;
						break;

					case E_ArmorQuality.Excellent:
						physicalProtection = 8;
						magicalProtection = 5;
						congestion = 4;
						statRequirementName = "Defense";
						statRequirementValue = 20;
						break;
				}
				break;
		}
		return (this);
	}
}


