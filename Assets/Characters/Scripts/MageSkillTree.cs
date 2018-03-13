using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

namespace Character
{
	public class MageSkillTree : SkillTree {
		private List<List<S_Skill>> skillTree;
		private List<S_Skill> archmageBranch;
		private List<S_Skill> necromancerBranch;
		private const string leftBranchName = "Archmage";
		private const string rightBranchName = "Necromancer";
		private int selectedBranchIndex;


		// Use this for initialization
		void Start () {
			archmageBranch = new List<S_Skill> ();
			S_Skill powerSurge = new S_Skill("Power surge","Damage : +8 . Precision : -15");
			archmageBranch.Add (powerSurge);
			S_Skill reactiveShield = new S_Skill("Reactive shield","Return damage dealt to the shield to the opponent upon waiting");
			archmageBranch.Add (reactiveShield);
			S_Skill erraticBounce = new S_Skill("Erratic bounce","Deal 5 damage to a random enemy upon hitting");
			archmageBranch.Add (erraticBounce);
			necromancerBranch = new List<S_Skill> ();
			S_Skill intangibility = new S_Skill("Intangibility","Increase agility by 15 upon waiting");
			necromancerBranch.Add (intangibility);
			S_Skill lifeLeech = new S_Skill("Life leech","50% of damage get back as life. Damage : -5");
			necromancerBranch.Add (lifeLeech);
			S_Skill regenerationAura = new S_Skill("Regeneration aura","Heal all nearby charater by 5");
			necromancerBranch.Add (regenerationAura);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (archmageBranch);
			skillTree.Add (necromancerBranch);
			selectedBranchIndex = -1;
		}

		void Awake () {
			archmageBranch = new List<S_Skill> ();
			S_Skill powerSurge = new S_Skill("Power surge","Damage : +8 . Precision : -15");
			archmageBranch.Add (powerSurge);
			S_Skill reactiveShield = new S_Skill("Reactive shield","Return damage dealt to the shield to the opponent upon waiting");
			archmageBranch.Add (reactiveShield);
			S_Skill erraticBounce = new S_Skill("Erratic bounce","Deal 5 damage to a random enemy upon hitting");
			archmageBranch.Add (erraticBounce);
			necromancerBranch = new List<S_Skill> ();
			S_Skill intangibility = new S_Skill("Intangibility","Increase agility by 15 upon waiting");
			necromancerBranch.Add (intangibility);
			S_Skill lifeLeech = new S_Skill("Life leech","50% of damage get back as life. Damage : -5");
			necromancerBranch.Add (lifeLeech);
			S_Skill regenerationAura = new S_Skill("Regeneration aura","Heal all nearby charater by 5");
			necromancerBranch.Add (regenerationAura);
			skillTree = new List<List<S_Skill>> ();
			skillTree.Add (archmageBranch);
			skillTree.Add (necromancerBranch);
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
				return E_AttackType.POWER_SURGE;
			else if (skillTree[1][1].isSkillAcquired())
				return E_AttackType.LIFE_LEECH;
			else
				return E_AttackType.NORMAL;
		}

		public override E_WaitSkill GetWaitSkill()
		{
			if (skillTree[0][1].isSkillAcquired())
				return E_WaitSkill.REACTIVE_SHIELD;
			else if (skillTree[1][0].isSkillAcquired())
				return E_WaitSkill.INTANGIBILITY;
			else
				return E_WaitSkill.NONE;
		}

		public override E_PassiveSkill GetPassiveSkill ()
		{
			if (skillTree[0][2].isSkillAcquired())
				return E_PassiveSkill.ERRATIC_BOUNCE;
			else if (skillTree[1][2].isSkillAcquired())
				return E_PassiveSkill.REGENERATINON_AURA;
			else
				return E_PassiveSkill.NONE;
		}
	}
}