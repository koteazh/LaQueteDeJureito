using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

namespace Character
{
	public class RangerSkillTree : SkillTree {
		private List<List<S_Skill>> skillTree;
		private List<S_Skill> adventurerBranch;
		private List<S_Skill> sniperBranch;
		private const string leftBranchName = "Adventurer";
		private const string rightBranchName = "Sniper";
		private int selectedBranchIndex;
		public GameObject trap;

		// Use this for initialization
		void Start () {
			adventurerBranch = new List<S_Skill> ();
			S_Skill preciseShot = new S_Skill("Precise shot","Precision : +15");
			adventurerBranch.Add (preciseShot);
			S_Skill occultation = new S_Skill("Occultation","Triple the cover value of the terrain upon waiting");
			adventurerBranch.Add (occultation);
			S_Skill adventurer = new S_Skill("Adventurer","Terrain no longer hinder the character");
			adventurerBranch.Add (adventurer);
			sniperBranch = new List<S_Skill> ();
			S_Skill layTrap = new S_Skill("Lay a trap","Damage enemies that come nearby the character");
			sniperBranch.Add (layTrap);
			S_Skill pointBlankShot = new S_Skill("Point blank shot","Damage : +2. Range : 1");
			sniperBranch.Add (pointBlankShot);
			S_Skill sniper = new S_Skill("Sniper","Increase the maximum range of the weapon by 1");
			sniperBranch.Add (sniper);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (adventurerBranch);
			skillTree.Add (sniperBranch);
			selectedBranchIndex = -1;
		}

		void Awake () {
			adventurerBranch = new List<S_Skill> ();
			S_Skill preciseShot = new S_Skill("Precise shot","Precision : +15");
			adventurerBranch.Add (preciseShot);
			S_Skill occultation = new S_Skill("Occultation","Triple the cover value of the terrain upon waiting");
			adventurerBranch.Add (occultation);
			S_Skill adventurer = new S_Skill("Adventurer","Terrain no longer hinder the character");
			adventurerBranch.Add (adventurer);
			sniperBranch = new List<S_Skill> ();
			S_Skill layTrap = new S_Skill("Lay a trap","Damage enemies that come nearby the character");
			sniperBranch.Add (layTrap);
			S_Skill pointBlankShot = new S_Skill("Point blank shot","Damage : +2. Range : 1");
			sniperBranch.Add (pointBlankShot);
			S_Skill sniper = new S_Skill("Sniper","Increase the maximum range of the weapon by 1");
			sniperBranch.Add (sniper);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (adventurerBranch);
			skillTree.Add (sniperBranch);
			selectedBranchIndex = -1;
		}

		public override void AcquireSkill(int branchIndex, int skillIndex)
		{
			if (selectedBranchIndex == -1)
				selectedBranchIndex = branchIndex;
			if (selectedBranchIndex == branchIndex)
			{
				if (skillIndex == 0) {
					S_Skill newSkill = skillTree [branchIndex] [skillIndex];
					newSkill.skillAcquired = true;
					skillTree [branchIndex] [skillIndex] = newSkill;
				} else if (skillTree [branchIndex] [skillIndex - 1].skillAcquired == true) {
					S_Skill newSkill = skillTree [branchIndex] [skillIndex];
					newSkill.skillAcquired = true;
					skillTree [branchIndex] [skillIndex] = newSkill;
				} else {
					return;
				}
				if (branchIndex == 0 && skillIndex == 1)
					this.gameObject.GetComponent<RangerStats> ().OccultationCoverIncrease();
				if (branchIndex == 1 && skillIndex == 2)
					this.gameObject.GetComponent<RangerStats> ().SniperRangeIncrease();
			}
		}

		public override S_Skill GetSkill (int branchIndex, int skillIndex)
		{
			return (skillTree [branchIndex] [skillIndex]);
		}

		public override string GetBranchName (int branchIndex)
		{
			if (branchIndex == 0)
				return (leftBranchName);
			else
				return (rightBranchName);
		}

		public override int GetSelectedBranchIndex()
		{
			return (selectedBranchIndex);
		}

		public void SetSelectedBranchIndex(int index)
		{
			if (selectedBranchIndex == -1)
				selectedBranchIndex = index;
		}

		public override E_AttackType GetSpecialAttackType()
		{
			if (skillTree[0][0].isSkillAcquired())
				return E_AttackType.PRECISE_SHOT;
			else if (skillTree[1][1].isSkillAcquired())
				return E_AttackType.POINT_BLANK_SHOT;
			else
				return E_AttackType.NORMAL;
		}

		public override E_WaitSkill GetWaitSkill()
		{
			if (skillTree[0][1].isSkillAcquired())
				return E_WaitSkill.OCCULTATION;
			else if (skillTree[1][0].isSkillAcquired())
				return E_WaitSkill.LAYTRAP;
			else
				return E_WaitSkill.NONE;
		}

		public override E_PassiveSkill GetPassiveSkill ()
		{
			if (skillTree[0][2].isSkillAcquired())
				return E_PassiveSkill.ADVENTURER;
			else if (skillTree[1][2].isSkillAcquired())
				return E_PassiveSkill.SNIPER;
			else
				return E_PassiveSkill.NONE;
		}
	}
}