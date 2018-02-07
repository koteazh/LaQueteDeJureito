using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Character {
	
	public class EnemyMageStats : AEnemyStats {
		private const string characterClass = "Mage";
		private Weapon.E_WeaponType weaponType = Weapon.E_WeaponType.Magic;
		public int Life = 25;
		public int Strength = 20;
		public int Dexterity = 20;
		public int Defense = 5;
		public int Resistance = 17;
		public int Agility = 13;
		public int Movement = 5;
		private Dictionary<string, int> characterStats = new Dictionary<string, int>();
		private static readonly Dictionary<string, int> statsIncrease = new Dictionary<string, int>
		{
			{ "Life", 35 },
			{ "Strength", 0 },
			{ "Intelligence", 75 },
			{ "Dexterity", 40 },
			{ "Defense", 15 },
			{ "Resistance", 65 },
			{ "Agility", 30 },
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
			status = E_CharacterStatus.READY;
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