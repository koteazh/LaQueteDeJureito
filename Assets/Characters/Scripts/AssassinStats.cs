﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Character {
	
	public class AssassinStats : ACharacterStats {
		private const string characterClass = "Assassin";
		private Armor armor = new Armor();
		private Weapon weapon = new Weapon();
		private Dictionary<string, int> characterStats = new Dictionary<string, int>();
		private static readonly Dictionary<string, int> statsIncrease = new Dictionary<string, int>
		{
			{ "Life", 45 },
			{ "Strength", 65 },
			{ "Intelligence", 0 },
			{ "Dexterity", 60 },
			{ "Defense", 10 },
			{ "Resistance", 25 },
			{ "Agility", 45 },
			{ "Movement", 10 },
		};			

		void Awake() {
			DontDestroyOnLoad(transform.gameObject);
		}

		public void Start()
		{
			characterStats ["Life"] = 30;
			characterStats ["Strength"] = 15;
			characterStats ["Intelligence"] = 0;
			characterStats ["Dexterity"] = 15;
			characterStats ["Defense"] = 7;
			characterStats ["Resistance"] = 13;
			characterStats ["Agility"] = 20;
			characterStats ["Movement"] = 6;
			level = 1;
		}

		public override void LevelUp ()
		{
			levelUpPanel.SetActive (true);
			level++;
			levelUpPanel.transform.Find ("ClassLevelRow/CharacterClass").GetComponent<Text>().text = characterClass;
			levelUpPanel.transform.Find ("ClassLevelRow/CharacerLevel").GetComponent<Text> ().text = level.ToString ();
			foreach (KeyValuePair<string, int> stats in characterStats) {
				levelUpPanel.transform.Find (stats.Key + "Row/StatsCurrentValue").GetComponent<Text>().text = stats.Value.ToString();
			}
			foreach (KeyValuePair<string, int> stats in statsIncrease) {
				int result = Random.Range (1, 101);
				if (result <= stats.Value) {
					levelUpPanel.transform.Find (stats.Key + "Row/StatsIncrease").GetComponent<Text> ().text = "+1";
					characterStats [stats.Key] += 1;
				} else {
					levelUpPanel.transform.Find (stats.Key + "Row/StatsIncrease").GetComponent<Text> ().text = "->";
				}
				levelUpPanel.transform.Find (stats.Key + "Row/StatsNewValue").GetComponent<Text>().text = characterStats [stats.Key].ToString();
			}
		}

		public override int GetCharacterStats(string statKey)
		{
			return characterStats [statKey];
		}

		public override int GetStatsIncrease(string statKey)
		{
			return statsIncrease [statKey];
		}

		public override void PrintStats()
		{
			foreach (KeyValuePair<string, int> stats in characterStats)
				Debug.Log(stats.Key + ": " + stats.Value);
		}

		public override void SetLevelUpPanel(GameObject _levelUpPanel)
		{
			levelUpPanel = _levelUpPanel;
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
			return (weapon.GetWeaponType());
		}
	}
}