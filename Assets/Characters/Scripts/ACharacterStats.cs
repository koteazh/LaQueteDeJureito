﻿using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class ACharacterStats : AStats
	{
		protected GameObject levelUpPanel;
		protected GameObject actionsPanel;
		private Dictionary<string, int> characterStats;
		private static readonly Dictionary<string, int> statsIncrease = null;
		private const string characterClass = "";

		public abstract void LevelUp();
		public abstract int GetStatsIncrease (string statKey);
		public abstract void SetLevelUpPanel(GameObject _levelUpPanel);
		public abstract void SetActionsPanel(GameObject _actionsPanel);
		public abstract void SetActionsPanelActive(bool active);
		public abstract void DisableMovement (bool disable);
	}
}

