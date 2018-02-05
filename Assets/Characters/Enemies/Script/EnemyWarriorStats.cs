using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character {
	
	public class EnemyWarriorStats : AEnemyStats {
		private const string characterClass = "Warrior";
		private Weapon.E_WeaponType weaponType = Weapon.E_WeaponType.Sword;
		public int Life = 45;
		public int Strength = 15;
		public int Dexterity = 12;
		public int Defense = 13;
		public int Resistance = 5;
		public int Agility = 11;
		public int Movement = 5;
		private Dictionary<string, int> characterStats = new Dictionary<string, int>();
		private static readonly Dictionary<string, int> statsIncrease = new Dictionary<string, int>
		{
			{ "Life", 75 },
			{ "Strength", 65 },
			{ "Dexterity", 30 },
			{ "Defense", 50 },
			{ "Resistance", 10 },
			{ "Agility", 20 },
			{ "Movement", 0 },
		};

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
			level = 1;
		}

		public override int GetCharacterStats(string statKey)
		{
			return characterStats [statKey];
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
	}
}