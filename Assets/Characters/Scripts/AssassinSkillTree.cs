using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

namespace Character
{
	public class AssassinSkillTree : SkillTree {
		private List<List<S_Skill>> skillTree;
		private List<S_Skill> mercenaryBranch;
		private List<S_Skill> mageHunterBranch;
		private const string leftBranchName = "Mercenary";
		private const string rightBranchName = "Mage-Hunter";
		private int selectedBranchIndex;


		// Use this for initialization
		void Start () {
			mercenaryBranch = new List<S_Skill> ();
			S_Skill armorPiercing = new S_Skill("Armor piercing","Ignore 25% of the enemy protection. Precision : -10");
			mercenaryBranch.Add (armorPiercing);
			S_Skill vigilance = new S_Skill("Vigilance","Double the agility bonus upon waiting");
			mercenaryBranch.Add (vigilance);
			S_Skill parryRiposte = new S_Skill("Parry and riposte","Deals 5 damage to the opponent when dodging an attack");
			mercenaryBranch.Add (parryRiposte);
			mageHunterBranch = new List<S_Skill> ();
			S_Skill magicDispell = new S_Skill("Magic dispell","Magic attack deals 50% less damage upon waiting");
			mageHunterBranch.Add (magicDispell);
			S_Skill mageHunterStrike = new S_Skill("Mage-Hunter strike","Damage +5 on mages");
			mageHunterBranch.Add (mageHunterStrike);
			S_Skill ambush = new S_Skill("Ambush","Strength increased by 2 and precision increased by 5");
			mageHunterBranch.Add (ambush);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (mercenaryBranch);
			skillTree.Add (mageHunterBranch);
			selectedBranchIndex = -1;
		}

		void Awake () {
			mercenaryBranch = new List<S_Skill> ();
			S_Skill armorPiercing = new S_Skill("Armor piercing","Ignore 25% of the enemy protection. Precision : -10");
			mercenaryBranch.Add (armorPiercing);
			S_Skill vigilance = new S_Skill("Vigilance","Double the agility bonus upon waiting");
			mercenaryBranch.Add (vigilance);
			S_Skill parryRiposte = new S_Skill("Parry and riposte","Deals 5 damage to the opponent when dodging an attack");
			mercenaryBranch.Add (parryRiposte);
			mageHunterBranch = new List<S_Skill> ();
			S_Skill magicDispell = new S_Skill("Magic dispell","Magic attack deals 50% less damage upon waiting");
			mageHunterBranch.Add (magicDispell);
			S_Skill mageHunterStrike = new S_Skill("Mage-Hunter strike","Damage +5 on mages");
			mageHunterBranch.Add (mageHunterStrike);
			S_Skill ambush = new S_Skill("Ambush","Strength increased by 2 and precision increased by 5");
			mageHunterBranch.Add (ambush);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (mercenaryBranch);
			skillTree.Add (mageHunterBranch);
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
				} else if (skillTree [branchIndex] [skillIndex - 1].isSkillAcquired()) {
					S_Skill newSkill = skillTree [branchIndex] [skillIndex];
					newSkill.skillAcquired = true;
					skillTree [branchIndex] [skillIndex] = newSkill;
				} else {
					return;
				}
				if (branchIndex == 1 && skillIndex == 2)
					this.gameObject.GetComponent<AssassinStats> ().AmbushStatsIncrease ();
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
				return E_AttackType.PIERCE_ARMOR;
			else if (skillTree[1][1].isSkillAcquired())
				return E_AttackType.MAGE_HUNTER_STRIKE;
			else
				return E_AttackType.NORMAL;
		}

		public override E_WaitSkill GetWaitSkill()
		{
			if (skillTree[0][1].isSkillAcquired())
				return E_WaitSkill.VIGILANCE;
			else if (skillTree[1][0].isSkillAcquired())
				return E_WaitSkill.DISPELL_MAGIC;
			else
				return E_WaitSkill.NONE;
		}

		public override E_PassiveSkill GetPassiveSkill ()
		{
			if (skillTree[0][2].isSkillAcquired())
				return E_PassiveSkill.PARRY_RIPOSTE;
			else if (skillTree[1][2].isSkillAcquired())
				return E_PassiveSkill.AMBUSH;
			else
				return E_PassiveSkill.NONE;
		}
	}
}