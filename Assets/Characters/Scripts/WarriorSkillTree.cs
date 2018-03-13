using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

namespace Character
{
	public class WarriorSkillTree : SkillTree {
		private List<List<S_Skill>> skillTree;
		private List<S_Skill> berserkerBranch;
		private List<S_Skill> guardianBranch;
		private const string leftBranchName = "Berserker";
		private const string rightBranchName = "Guardian";
		private int selectedBranchIndex;


		// Use this for initialization
		void Start () {
			berserkerBranch = new List<S_Skill> ();
			S_Skill powerfulStrike = new S_Skill("Powerful strike","Damage : +3 . Precision : -5");
			berserkerBranch.Add (powerfulStrike);
			S_Skill whetstone = new S_Skill("Whetstone","Add 2 weapon damage upon waiting (not cumulative)");
			berserkerBranch.Add (whetstone);
			S_Skill berserker = new S_Skill("Berserker","Add 10% of missing health as strength");
			berserkerBranch.Add (berserker);
			guardianBranch = new List<S_Skill> ();
			S_Skill WarriorRest = new S_Skill("Warrior's rest","Restore 5 health upon waiting");
			guardianBranch.Add (WarriorRest);
			S_Skill intimidatingStrike = new S_Skill("Intimidating strike","Target's strength -2 upon hitting. Precision : -10");
			guardianBranch.Add (intimidatingStrike);
			S_Skill mindFortress = new S_Skill("Mind Fortress","Resistance increase by 5");
			guardianBranch.Add (mindFortress);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (berserkerBranch);
			skillTree.Add (guardianBranch);
			selectedBranchIndex = -1;
		}

		void Awake() {
			berserkerBranch = new List<S_Skill> ();
			S_Skill powerfulStrike = new S_Skill("Powerful strike","Damage : +3 . Precision : -5");
			berserkerBranch.Add (powerfulStrike);
			S_Skill whetstone = new S_Skill("Whetstone","Add 2 weapon damage upon waiting (not cumulative)");
			berserkerBranch.Add (whetstone);
			S_Skill berserker = new S_Skill("Berserker","Add 10% of missing health as strength");
			berserkerBranch.Add (berserker);
			guardianBranch = new List<S_Skill> ();
			S_Skill WarriorRest = new S_Skill("Warrior's rest","Restore 5 health upon waiting");
			guardianBranch.Add (WarriorRest);
			S_Skill intimidatingStrike = new S_Skill("Intimidating strike","Target's strength -2 upon hitting. Precision : -10");
			guardianBranch.Add (intimidatingStrike);
			S_Skill mindFortress = new S_Skill("Mind Fortress","Resistance increase by 5");
			guardianBranch.Add (mindFortress);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (berserkerBranch);
			skillTree.Add (guardianBranch);
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
				if (branchIndex == 1 && skillIndex == 2)
					this.gameObject.GetComponent<WarriorStats> ().MindFortressStatsIncrease ();
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
				return E_AttackType.POWERFUL_STRIKE;
			else if (skillTree[1][1].isSkillAcquired())
				return E_AttackType.INTIMIDATING_STRIKE;
			else
				return E_AttackType.NORMAL;
		}

		public override E_WaitSkill GetWaitSkill()
		{
			if (skillTree[0][1].isSkillAcquired())
				return E_WaitSkill.WHETSTONE;
			else if (skillTree[1][0].isSkillAcquired())
				return E_WaitSkill.WARRIOR_REST;
			else
				return E_WaitSkill.NONE;
		}

		public override E_PassiveSkill GetPassiveSkill ()
		{
			if (skillTree[0][2].isSkillAcquired())
				return E_PassiveSkill.BERSERKER;
			else if (skillTree[1][2].isSkillAcquired())
				return E_PassiveSkill.MIND_FORTRESS;
			else
				return E_PassiveSkill.NONE;
		}
	}
}