using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class ACharacterStats : MonoBehaviour
	{
		protected GameObject levelUpPanel;
		private Dictionary<string, int> characterStats;
		private static readonly Dictionary<string, int> statsIncrease = null;
		protected int level;
		protected E_CharacterStatus status;
		private const string characterClass = "";

		public abstract void LevelUp();
		public abstract int GetCharacterStats (string statKey);
		public abstract int GetStatsIncrease (string statKey);
		public abstract void PrintStats ();
		public abstract void SetLevelUpPanel(GameObject _levelUpPanel);
		public abstract void SetStatus(E_CharacterStatus _status);
		public abstract E_CharacterStatus GetStatus();
	}
}

