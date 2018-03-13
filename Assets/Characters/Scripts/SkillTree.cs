using System;

using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class SkillTree : MonoBehaviour
	{
		public abstract E_AttackType GetSpecialAttackType ();
		public abstract E_WaitSkill GetWaitSkill ();
		public abstract E_PassiveSkill GetPassiveSkill ();
		public abstract string GetBranchName (int branchIndex);
		public abstract S_Skill GetSkill (int branchIndex, int skillIndex);
		public abstract int GetSelectedBranchIndex ();
		public abstract void AcquireSkill (int branchIndex, int skillIndex);
	}
}

