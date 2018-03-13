using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillTreePanelSetUp : MonoBehaviour {

	Character.SkillTree skillTree;

	// Use this for initialization
	void Start () {
		GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in characters) {
			character.GetComponent<Character.ACharacterStats> ().SetSkillTreePanel (gameObject);
		}
		gameObject.SetActive (false);
	}

	public void FillTree (string characterClass, int level, Character.SkillTree _skillTree)
	{
		S_Skill skill;

		skillTree = _skillTree;
		gameObject.transform.Find ("CharacterClass").GetComponent<Text>().text = characterClass;
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/BranchName").GetComponent<Text>().text = skillTree.GetBranchName(0);
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/BranchName").GetComponent<Text>().text = skillTree.GetBranchName(1);
		skill = skillTree.GetSkill (0, 0);
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_1/SkillName").GetComponent<Text> ().text = skill.GetName();
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_1/SkillDesc").GetComponent<Text> ().text = skill.GetDescription ();
		skill = skillTree.GetSkill (0, 1);
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_2/SkillName").GetComponent<Text> ().text = skill.GetName();
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_2/SkillDesc").GetComponent<Text> ().text = skill.GetDescription ();
		skill = skillTree.GetSkill (0, 2);
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_3/SkillDesc").GetComponent<Text> ().text = skill.GetDescription ();
		gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_3/SkillName").GetComponent<Text> ().text = skill.GetName();
		skill = skillTree.GetSkill (1, 0);
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_1/SkillName").GetComponent<Text> ().text = skill.GetName();
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_1/SkillDesc").GetComponent<Text> ().text = skill.GetDescription ();
		skill = skillTree.GetSkill (1, 1);
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_2/SkillName").GetComponent<Text> ().text = skill.GetName();
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_2/SkillDesc").GetComponent<Text> ().text = skill.GetDescription ();
		skill = skillTree.GetSkill (1, 2);
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_3/SkillDesc").GetComponent<Text> ().text = skill.GetDescription ();
		gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_3/SkillName").GetComponent<Text> ().text = skill.GetName();
		if (level == 2) {
			gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_1").GetComponent<Button> ().interactable = true;
			gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_2").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_1/Skill_3").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_1").GetComponent<Button> ().interactable = true;
			gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_2").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_2/Skill_3").GetComponent<Button> ().interactable = false;
		}
		if (level == 3) {
			int branchIndex = skillTree.GetSelectedBranchIndex () + 1;
			int disabledBranch = branchIndex == 1 ? 2 : 1;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + branchIndex + "/Skill_1").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + branchIndex + "/Skill_2").GetComponent<Button> ().interactable = true;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + branchIndex + "/Skill_3").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + disabledBranch + "/Skill_1").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + disabledBranch + "/Skill_2").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + disabledBranch + "/Skill_3").GetComponent<Button> ().interactable = false;
		}
		if (level == 4) {
			int branchIndex = skillTree.GetSelectedBranchIndex () + 1;
			int disabledBranch = branchIndex == 1 ? 2 : 1;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + branchIndex + "/Skill_1").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + branchIndex + "/Skill_2").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + branchIndex + "/Skill_3").GetComponent<Button> ().interactable = true;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + disabledBranch + "/Skill_1").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + disabledBranch + "/Skill_2").GetComponent<Button> ().interactable = false;
			gameObject.transform.Find ("SkillPanel/BranchPanel_" + disabledBranch + "/Skill_3").GetComponent<Button> ().interactable = false;
		}
	}

	public void AcquireSkill()
	{
		GameObject button = EventSystem.current.currentSelectedGameObject;
		GameObject branch = button.transform.parent.gameObject;

		int skillIndex = int.Parse(button.name.Substring(6)) - 1;
		int branchIndex = int.Parse(branch.name.Substring (12)) - 1;
		skillTree.AcquireSkill (branchIndex, skillIndex);
		gameObject.SetActive (false);
	}
}
