using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;

public class CombatInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject cursor = GameObject.FindGameObjectWithTag ("Cursor");
		cursor.GetComponent<GameCursor> ().SetUpCombatInfoPanel (this);
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<AiController> ().SetUpCombatInfo (this);
		}
		gameObject.SetActive (false);
	}

	public int GetHitChance (AStats target, AStats attacker, Case terrain, E_AttackType attackType)
	{
		int dexterity = attacker.GetCharacterStats("Dexterity");
		int weaponPrecision = attacker.GetWeapon().precision;
		int agility = target.GetCharacterStats ("Agility");
		int caseProtection = terrain.getType ().cover_value;
		if (target.GetType () == typeof(RangerStats)) {
			RangerStats _target = (RangerStats)target;
			if (_target.IsBonusActive ()) {
				caseProtection = caseProtection * _target.GetCoverMultiplier();
			}
		}
		int congestion = target.GetArmor ().congestion;

		switch (attackType)
		{
			case E_AttackType.PIERCE_ARMOR:
				weaponPrecision -= 10;
				break;
			case E_AttackType.PRECISE_SHOT:
				weaponPrecision += 15;
				break;
			case E_AttackType.POWERFUL_STRIKE:
				weaponPrecision -= 10;
				break;
			case E_AttackType.INTIMIDATING_STRIKE:
				weaponPrecision -= 10;
				break;
			case E_AttackType.POWER_SURGE:
				weaponPrecision -= 15;
				break;
			default :
				break;
		}

		int hitChance = (dexterity + weaponPrecision) - (agility - (congestion * 2) + (caseProtection * 5));
		if (hitChance > 100)
			hitChance = 100;
		if (hitChance < 0)
			hitChance = 0;
		return (hitChance);
	}

	public int GetDamage (AStats target, AStats attacker, E_AttackType attackType)
	{
		int defensiveStat;
		int armor;
		int strength = attacker.GetCharacterStats("Strength");
		int weaponDamage = attacker.GetWeapon().damage;
		if (attacker.GetWeaponType () == Weapon.E_WeaponType.Magic) {
			defensiveStat = target.GetCharacterStats ("Resistance");
			armor = target.GetArmor ().magicalProtection;
		} else {
			defensiveStat = target.GetCharacterStats ("Defense");
			armor = target.GetArmor ().physicalProtection;
		}

		switch (attackType)
		{
			case E_AttackType.PIERCE_ARMOR:
				armor = armor - armor * 25 / 100;
				defensiveStat = defensiveStat - defensiveStat * 25 / 100;
				break;
			case E_AttackType.MAGE_HUNTER_STRIKE:
				if (target.GetWeaponType() == Weapon.E_WeaponType.Magic)
					strength += 5;
				break;
			case E_AttackType.POINT_BLANK_SHOT:
				strength += 2;
				break;
			case E_AttackType.POWERFUL_STRIKE:
				strength += 3;
				break;
			case E_AttackType.POWER_SURGE:
				strength += 8;
				break;
			case E_AttackType.LIFE_LEECH:
				strength -= 5;
				break;
			default :
				break;
		}
		int damage = (weaponDamage + strength) - (defensiveStat + armor);
		if (damage < 0)
			damage = 0;
		return damage;
	}

	private void SetLifeBar(AStats target)
	{
		int currentHealth = target.GetCurrentHealth ();
		int totalHealth = target.GetCharacterStats ("Life");
		float lifeBarProgression = ((float)currentHealth / (float)totalHealth) * 300;
		gameObject.transform.Find ("TargetLife/HealthBarText/CurrentHealth").GetComponent<Text>().text = currentHealth.ToString();
		gameObject.transform.Find ("TargetLife/HealthBarText/TotalHealth").GetComponent<Text>().text = totalHealth.ToString();
		Vector2 lifeBarFg = gameObject.transform.Find ("TargetLife/HealthBarBg/HealthBarFg").GetComponent<RectTransform> ().sizeDelta;
		lifeBarFg.x = lifeBarProgression;
		gameObject.transform.Find ("TargetLife/HealthBarBg/HealthBarFg").GetComponent<RectTransform> ().sizeDelta = lifeBarFg;
	}

	public void SetCombatPanel(AStats target, AStats attacker, Case terrain, E_AttackType attackType)
	{
		int hitChance = GetHitChance(target, attacker, terrain, attackType);
		int damage = GetDamage (target, attacker, attackType);

		gameObject.transform.Find ("TargetNameText").GetComponent<Text>().text = target.GetCharacterClass();
		SetLifeBar (target);
		gameObject.transform.Find ("HitChanceRow/HitChance").GetComponent<Text>().text = hitChance.ToString();
		gameObject.transform.Find ("DamageRow/Damage").GetComponent<Text>().text = damage.ToString();
	}
}
