using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Character {
	
	public class EnemyRangerStats : AEnemyStats {
		private const string characterClass = "Ranger";
		[SerializeField] private Weapon.E_WeaponType weaponType = Weapon.E_WeaponType.Bow;
		[SerializeField] private Weapon.E_WeaponQuality weaponQuality = Weapon.E_WeaponQuality.Poor;
		[SerializeField] private Armor.E_ArmorType armorType = Armor.E_ArmorType.Medium;
		[SerializeField] private Armor.E_ArmorQuality armorQuality = Armor.E_ArmorQuality.Poor;
		public int Life = 25;
		public int Strength = 13;
		public int Dexterity = 25;
		public int Defense = 7;
		public int Resistance = 7;
		public int Agility = 23;
		public int Movement = 6;
		private Dictionary<string, int> characterStats = new Dictionary<string, int>();

		void Awake() {
			DontDestroyOnLoad(transform.gameObject);
		}

		public void Start()
		{
			characterStats ["Life"] = Life;
			characterStats ["Strength"] = Strength;
			characterStats ["Dexterity"] = Dexterity;
			characterStats ["Defense"] = Defense;
			characterStats ["Resistance"] = Resistance;
			characterStats ["Agility"] = Agility;
			characterStats ["Movement"] = Movement;
			armor = gameObject.AddComponent<Armor> ();
			armor.GetArmor (armorType, armorQuality);
			weapon = gameObject.AddComponent<Weapon> ();
			weapon.GetWeapon (weaponType, weaponQuality);
			status = E_CharacterStatus.READY;
			level = 1;
			currentHealth = characterStats ["Life"];
		}

		public override void ModifyCurrentHealth (int damage)
		{
			currentHealth -= damage;
			if (currentHealth <= 0) {
				Destroy(gameObject);
			}
			if (currentHealth > characterStats["Life"])
				currentHealth = characterStats["Life"];
		}

		public override void ResetHealth ()
		{
			currentHealth = characterStats ["Life"];
		}

		public override int GetCurrentHealth ()
		{
			return currentHealth;
		}

		public override int GetLevel()
		{
			return (level);
		}

		public override int GetCharacterStats(string statKey)
		{
			return characterStats [statKey];
		}

		public override void ModifyCharacterStat (string statKey, int value)
		{
			characterStats [statKey] += value;
			if (characterStats [statKey] < 0)
				characterStats [statKey] = 0;
		}

		public override void PrintStats()
		{
			foreach (KeyValuePair<string, int> stats in characterStats)
				Debug.Log(stats.Key + ": " + stats.Value);
		}

		public override void SetStatus(E_CharacterStatus _status)
		{
			status = _status;
		}

		public override E_CharacterStatus GetStatus()
		{
			return (status);
		}

		public override Weapon.E_WeaponType GetWeaponType()
		{
			return (weaponType);
		}

		public override string GetCharacterClass ()
		{
			return characterClass;
		}

		public override Weapon GetWeapon ()
		{
			return (weapon);
		}

		public override Armor GetArmor ()
		{
			return (armor);
		}
	}
}