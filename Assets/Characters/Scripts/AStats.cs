using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class AStats : MonoBehaviour
	{
		private Dictionary<string, int> characterStats;
		protected int level;
		protected E_CharacterStatus status;
		private const string characterClass = "";

		public abstract int GetCharacterStats (string statKey);
		public abstract void PrintStats ();
		public abstract void SetStatus(E_CharacterStatus _status);
		public abstract E_CharacterStatus GetStatus();
	}
}

