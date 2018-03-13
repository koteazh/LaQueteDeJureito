using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;

public class CharacterInfo : MonoBehaviour {

	public void UpdateCharacterInfo (Vector3 position)
	{
		Collider[] colliders;
		GameObject character = null;
		bool isEnemy = false;

		if ((colliders = Physics.OverlapSphere (position, 1f)).Length > 1) {
			foreach (Collider collider in colliders) {
			GameObject go = collider.gameObject; 
				if (go.tag == "Character") {
					character = go;
					isEnemy = false;
					break;
				}
				if (go.tag == "Enemy") {
					character = go;
					isEnemy = true;
					break;
				}
			}
		}

		if (character == null) {
			this.gameObject.SetActive (false);
		} else {
			if (!this.gameObject.activeInHierarchy)
				this.gameObject.SetActive (true);
			string characterClass = character.GetComponent<AStats> ().GetCharacterClass ();
			if (isEnemy)
				characterClass = "Enemy " + character.GetComponent<AStats> ().GetCharacterClass ();
			this.gameObject.transform.Find ("CharacterClassLabel").GetComponent<Text> ().text = characterClass;

			//Lifebar setup
			int currentHealth = character.GetComponent<AStats> ().GetCurrentHealth ();
			int totalHealth = character.GetComponent<AStats> ().GetCharacterStats ("Life");
			float lifeBarProgression = ((float)currentHealth / (float)totalHealth) * 300;
			this.gameObject.transform.Find ("LifeXpRow/LifeBar/HealthBarText/CurrentHealth").GetComponent<Text> ().text = currentHealth.ToString();
			this.gameObject.transform.Find ("LifeXpRow/LifeBar/HealthBarText/TotalHealth").GetComponent<Text> ().text = totalHealth.ToString();
			Vector2 lifeBarFg = gameObject.transform.Find ("LifeXpRow/LifeBar/HealthBarBg/HealthBarFg").GetComponent<RectTransform> ().sizeDelta;
			lifeBarFg.x = lifeBarProgression;
			gameObject.transform.Find ("LifeXpRow/LifeBar/HealthBarBg/HealthBarFg").GetComponent<RectTransform> ().sizeDelta = lifeBarFg;

			//Xpbar setup
			if (isEnemy) {
				this.gameObject.transform.Find ("LifeXpRow/XpBar/XpBarText/CurrentXp").GetComponent<Text> ().text = "X";
				this.gameObject.transform.Find ("LifeXpRow/XpBar/XpBarText/TotalXp").GetComponent<Text> ().text = "X";
				Vector2 xpBarFg = gameObject.transform.Find ("LifeXpRow/XpBar/XpBarBg/XpBarFg").GetComponent<RectTransform> ().sizeDelta;
				xpBarFg.x = 0;
				gameObject.transform.Find ("LifeXpRow/XpBar/XpBarBg/XpBarFg").GetComponent<RectTransform> ().sizeDelta = xpBarFg;
			} else {
				int currentXp = character.GetComponent<ACharacterStats> ().GetExperience();
				float xpBarProgression = ((float)currentXp / 100f) * 300;
				this.gameObject.transform.Find ("LifeXpRow/XpBar/XpBarText/CurrentXp").GetComponent<Text> ().text = currentXp.ToString();
				this.gameObject.transform.Find ("LifeXpRow/XpBar/XpBarText/TotalXp").GetComponent<Text> ().text = "100";
				Vector2 xpBarFg = gameObject.transform.Find ("LifeXpRow/XpBar/XpBarBg/XpBarFg").GetComponent<RectTransform> ().sizeDelta;
				xpBarFg.x = xpBarProgression;
				gameObject.transform.Find ("LifeXpRow/XpBar/XpBarBg/XpBarFg").GetComponent<RectTransform> ().sizeDelta = xpBarFg;
			}

			//Stats setup
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/LifeRow/LifeValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Life").ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/StrengthRow/StrengthValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Strength").ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/DexterityRow/DexterityValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Dexterity").ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/DefenseRow/DefenseValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Defense").ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/ResistanceRow/ResistanceValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Resistance").ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/AgilityRow/AgilityValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Agility").ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/StatsPanel/MovementRow/MovementValue").GetComponent<Text> ().text = character.GetComponent<AStats> ().GetCharacterStats("Movement").ToString();

			//Weapon setup
			Weapon characterWeapon = character.GetComponent<AStats>().GetWeapon();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/WeaponPanel/WeaponLabel").GetComponent<Text> ().text = characterWeapon.GetWeaponType().ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/WeaponPanel/QualityRow/QualityValue").GetComponent<Text> ().text = characterWeapon.GetWeaponQuality().ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/WeaponPanel/DamageRow/DamageValue").GetComponent<Text> ().text = characterWeapon.damage.ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/WeaponPanel/PrecisionRow/PrecisionValue").GetComponent<Text> ().text = characterWeapon.precision.ToString();

			//Armor setup
			Armor characterArmor = character.GetComponent<AStats>().GetArmor();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/ArmorPanel/ArmorLabel").GetComponent<Text> ().text = characterArmor.type.ToString() + " armor";
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/ArmorPanel/QualityRow/QualityValue").GetComponent<Text> ().text = characterArmor.quality.ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/ArmorPanel/PhysicalRow/PhysicalValue").GetComponent<Text> ().text = characterArmor.physicalProtection.ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/ArmorPanel/MagicalRow/MagicalValue").GetComponent<Text> ().text = characterArmor.magicalProtection.ToString();
			gameObject.transform.Find ("StatsEquipmentPanels/EquipementColumn/ArmorPanel/CongestionRow/CongestionValue").GetComponent<Text> ().text = characterArmor.congestion.ToString();

		}
	}
}
