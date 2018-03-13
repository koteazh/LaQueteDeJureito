using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour {
	public void EndPlayerTurn()
	{
		GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
		foreach( GameObject character in characters)
		{
			character.GetComponent<Character.AStats>().SetStatus(Character.E_CharacterStatus.IS_WAITING);
		}
		GameObject.Find ("Cursor").GetComponent<AiActionTurn> ().SetInitAiTurn (true);
	}
}
