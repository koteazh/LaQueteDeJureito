using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character {
	
	public class WarriorStats : ACharacterStats {
		private const string characterClass = "Warrior";
		private Armor armor = new Armor();
		private Weapon weapon = new Weapon();
		private Dictionary<string, int> characterStats = new Dictionary<string, int>();
		private static readonly Dictionary<string, int> statsIncrease = new Dictionary<string, int>
		{
			{ "Life", 75 },
			{ "Strength", 65 },
			{ "Intelligence", 0 },
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
			characterStats ["Life"] = 45;
			characterStats ["Strength"] = 15;
			characterStats ["Intelligence"] = 0;
			characterStats ["Dexterity"] = 12;
			characterStats ["Defense"] = 13;
			characterStats ["Resistance"] = 5;
			characterStats ["Agility"] = 11;
			characterStats ["Movement"] = 5;
			status = E_CharacterStatus.READY;
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

		public override void SetActionsPanel(GameObject _actionsPanel)
		{
			actionsPanel = _actionsPanel;
		}

		public override void SetActionsPanelActive(bool active)
		{
			actionsPanel.SetActive (active);
		}

		public override void DisableMovement(bool _interactable)
		{
			GameObject moveButton = actionsPanel.transform.Find ("MoveButton").gameObject;
			moveButton.GetComponent<Button> ().interactable = _interactable;
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