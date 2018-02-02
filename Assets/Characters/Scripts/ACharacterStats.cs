using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class ACharacterStats : AStats
	{
		protected GameObject levelUpPanel;
		private Dictionary<string, int> characterStats;
		private static readonly Dictionary<string, int> statsIncrease = null;
		protected int level;
		protected E_CharacterStatus status;
		private const string characterClass = "";

		public abstract void LevelUp();
		public abstract int GetStatsIncrease (string statKey);
		public abstract void SetLevelUpPanel(GameObject _levelUpPanel);
	}
}

