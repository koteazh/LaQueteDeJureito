using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class ACharacterStats : AStats
	{
		protected GameObject levelUpPanel;
		protected GameObject actionsPanel;
		protected GameObject skillTreePanel;
		protected bool bonusActive;
		protected Dictionary<string, int> characterStats;
		protected int xp;

		public abstract void LevelUp();
		public abstract void GainExperience(int xpValue, int levelDifference);
		public abstract int GetExperience();
		public abstract int GetStatsIncrease (string statKey);
		public abstract void SetLevelUpPanel(GameObject _levelUpPanel);
		public abstract void SetLevelUpPanelActive(bool active);
		public abstract void SetActionsPanel(GameObject _actionsPanel);
		public abstract void SetActionsPanelActive(bool active);
		public abstract void SetSkillTreePanel(GameObject _skillTree);
		public abstract void SetSkillTreePanelActive(bool active);
		public abstract void DisableAction(string action, bool _interactable);
		public abstract bool IsBonusActive ();
		public abstract void AddWaitBonus ();
		public abstract void RemoveWaitBonus ();
	}
}

