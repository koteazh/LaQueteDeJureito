using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Character {
	
	public class EnemyRangerStats : AEnemyStats {
		private const string characterClass = "Ranger";
		private Weapon.E_WeaponType weaponType = Weapon.E_WeaponType.Bow;
		public int Life = 25;
		public int Strength = 13;
		public int Dexterity = 25;
		public int Defense = 7;
		public int Resistance = 7;
		public int Agility = 23;
		public int Movement = 6;
		private Dictionary<string, int> characterStats = new Dictionary<string, int>();
		private static readonly Dictionary<string, int> statsIncrease = new Dictionary<string, int>
		{
			{ "Life", 40 },
			{ "Strength", 65 },
			{ "Intelligence", 0 },
			{ "Dexterity", 70 },
			{ "Defense", 25 },
			{ "Resistance", 10 },
			{ "Agility", 35 },
			{ "Movement", 5 },
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