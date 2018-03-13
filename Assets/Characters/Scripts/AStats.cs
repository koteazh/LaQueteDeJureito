using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public abstract class AStats : MonoBehaviour
	{
		[SerializeField] protected int level;
		protected Armor armor;
		protected Weapon weapon;
		protected E_CharacterStatus status;
		protected int currentHealth;

		public abstract int GetCurrentHealth ();
		public abstract void ResetHealth ();
		public abstract void ModifyCurrentHealth (int damage);
		public abstract int GetCharacterStats (string statKey);
		public abstract void ModifyCharacterStat (string statKey, int value);
		public abstract void PrintStats ();
		public abstract void SetStatus(E_CharacterStatus _status);
		public abstract E_CharacterStatus GetStatus();
		public abstract Weapon.E_WeaponType GetWeaponType ();
		public abstract Weapon GetWeapon ();
		public abstract Armor GetArmor ();
		public abstract string GetCharacterClass ();
		public abstract int GetLevel();
	}
}

