using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character {
	
	public class WarriorStats : ACharacterStats {
		private const string characterClass = "Warrior";
		private bool whetsoneActive = false;
		private static readonly Dictionary<string, int> statsIncrease = new Dictionary<string, int>
		{
			{ "Life", 75 },
			{ "Strength", 65 },
			{ "Dexterity", 30 },
			{ "Defense", 50 },
			{ "Resistance", 10 },
			{ "Agility", 20 },
			{ "Movement", 5 },
		};

		void Awake() {
			DontDestroyOnLoad(transform.gameObject);
			characterStats = new Dictionary<string, int> {
				{ "Life", 45 },
				{ "Strength", 15 },
				{ "Dexterity", 12 },
				{ "Defense", 13 },
				{ "Resistance", 5 },
				{ "Agility", 11 },
				{ "Movement", 5 }
			};
			status = E_CharacterStatus.READY;
			level = 1;
			armor = gameObject.AddComponent<Armor> ();
			armor.GetArmor (Armor.E_ArmorType.Heavy, Armor.E_ArmorQuality.Poor);
			weapon = gameObject.AddComponent<Weapon> ();
			weapon.GetWeapon (Weapon.E_WeaponType.Sword, Weapon.E_WeaponQuality.Poor);
			bonusActive = false;
			currentHealth = characterStats ["Life"];		}

		public void Start()
		{
			characterStats = new Dictionary<string, int> {
				{ "Life", 45 },
				{ "Strength", 15 },
				{ "Dexterity", 12 },
				{ "Defense", 13 },
				{ "Resistance", 5 },
				{ "Agility", 11 },
				{ "Movement", 5 }
			};
			status = E_CharacterStatus.READY;
			level = 1;
			armor = gameObject.AddComponent<Armor> ();
			armor.GetArmor (Armor.E_ArmorType.Heavy, Armor.E_ArmorQuality.Poor);
			weapon = gameObject.AddComponent<Weapon> ();
			weapon.GetWeapon (Weapon.E_WeaponType.Sword, Weapon.E_WeaponQuality.Poor);
			bonusActive = false;
			currentHealth = characterStats ["Life"];
		}

		public override void ModifyCurrentHealth (int damage)
		{
			currentHealth -= damage;
			if (currentHealth <= 0) {
				levelUpPanel.SetActive (false);
				gameObject.SetActive(false);
				GameObject.Find ("Cursor").GetComponent<AiActionTurn>().checkDefeat();
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

		public override void GainExperience(int xpValue, int levelDifference)
		{
			int xpGained = xpValue + (2 * levelDifference);

			if (xpGained < 0)
				xpGained = 0;
			xp += xpGained;
			if (xp >= 100) {
				xp -= 100;
				LevelUp ();
			}
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
			GameObject.FindGameObjectWithTag ("Cursor").GetComponent<GameCursor>().levelUpPanelActive = true;
			if (characterStats ["Defense"] == 15) {
				armor.GetArmor (Armor.E_ArmorType.Light, Armor.E_ArmorQuality.Common);
			}
			if (characterStats ["Defense"] == 20) {
				armor.GetArmor (Armor.E_ArmorType.Light, Armor.E_ArmorQuality.Excellent);
			}
			if (characterStats ["Strength"] == 20) {
				weapon.GetWeapon (Weapon.E_WeaponType.Sword, Weapon.E_WeaponQuality.Common);
			}
			if (characterStats ["Strength"] == 25) {
				weapon.GetWeapon (Weapon.E_WeaponType.Sword, Weapon.E_WeaponQuality.Excellent);
			}
		}

		public override int GetExperience()
		{
			return (xp);
		}

		public override int GetCharacterStats(string statKey)
		{
			if (gameObject.GetComponent<WarriorSkillTree> ().GetPassiveSkill() == E_PassiveSkill.BERSERKER && statKey == "Strength") {
				int bonusMissingHealth = (characterStats ["Life"] - currentHealth) * 10 / 100;
				print (characterStats [statKey] + bonusMissingHealth);
				return (characterStats [statKey] + bonusMissingHealth);
			}
			return characterStats [statKey];
		}

		public override void ModifyCharacterStat (string statKey, int value)
		{
			characterStats [statKey] += value;
			if (characterStats [statKey] < 0)
				characterStats [statKey] = 0;
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

		public override void SetLevelUpPanelActive(bool active)
		{
			levelUpPanel.SetActive (active);
		}

		public override void SetActionsPanel(GameObject _actionsPanel)
		{
			actionsPanel = _actionsPanel;
		}

		public override void SetActionsPanelActive(bool active)
		{
			if (gameObject.GetComponent<WarriorSkillTree> ().GetSpecialAttackType () != E_AttackType.NORMAL)
				actionsPanel.GetComponent<ActionsPanelSetUp> ().ActivateSpecialAttackButton(true);
			else
				actionsPanel.GetComponent<ActionsPanelSetUp> ().ActivateSpecialAttackButton(false);
			actionsPanel.SetActive (active);
		}

		public override void SetSkillTreePanel(GameObject _skillTreePanel)
		{
			skillTreePanel = _skillTreePanel;
		}

		public override void SetSkillTreePanelActive(bool active)
		{
			if (active)
				skillTreePanel.GetComponent<SkillTreePanelSetUp> ().FillTree (characterClass, level, gameObject.GetComponent<WarriorSkillTree>());
			skillTreePanel.SetActive (active);
		}

		public override void DisableAction(string action, bool _interactable)
		{
			GameObject actionButton = actionsPanel.transform.Find (action + "Button").gameObject;
			actionButton.GetComponent<Button> ().interactable = _interactable;
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

		public override bool IsBonusActive ()
		{
			return (bonusActive);
		}

		public override void AddWaitBonus()
		{
			if (gameObject.GetComponent<WarriorSkillTree> ().GetWaitSkill () == E_WaitSkill.WHETSTONE && !whetsoneActive) {
				characterStats ["Strength"] = +2;
				whetsoneActive = true;
			}
			if (gameObject.GetComponent<WarriorSkillTree> ().GetWaitSkill () == E_WaitSkill.WARRIOR_REST)
				ModifyCurrentHealth (-5);
			characterStats ["Defense"] += 5;
			bonusActive = true;
		}

		public override void RemoveWaitBonus()
		{
			if (bonusActive) {
				characterStats ["Defense"] -= 5;
				bonusActive = false;
			}
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

		public void MindFortressStatsIncrease()
		{
			characterStats ["Resistance"] += 5; 
		}

		void Update()
		{
			if (Input.GetMouseButtonDown (0) && skillTreePanel != null && skillTreePanel.activeInHierarchy) {
				Time.timeScale = 1;
				SetLevelUpPanelActive (false);
			}
			if (levelUpPanel.activeInHierarchy && status == E_CharacterStatus.IS_WAITING) {
				if (level >= 2 && level <= 4) {
					SetSkillTreePanelActive (true);
					Time.timeScale = 0;
				}
			}
		}
	}
}