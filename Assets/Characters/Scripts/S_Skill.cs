using System;
using UnityEngine;

public struct S_Skill
{
	string skillName;
	string skillDescription;
	public bool skillAcquired;

	public S_Skill(string name, string desc)
	{
		skillName = name;
		skillDescription = desc;
		skillAcquired = false;
	}

	public void acquireSkill()
	{
		skillAcquired = true;
	}

	public bool isSkillAcquired()
	{
		return (skillAcquired);
	}

	public string GetName()
	{
		return (skillName);
	}

	public string GetDescription()
	{
		return (skillDescription);
	}
}

