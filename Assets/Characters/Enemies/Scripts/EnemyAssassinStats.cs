using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Character {
	
	public class EnemyAssassinStats : AEnemyStats {
		private const string characterClass = "Assassin";
		private Weapon.E_WeaponType weaponType = Weapon.E_WeaponType.Dagger;
		public int Life = 30;
		public int Strength = 15;
		public int Dexterity = 15;
		public int Defense = 7;
		public int Resistance = 13;
		public int Agility = 20;
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