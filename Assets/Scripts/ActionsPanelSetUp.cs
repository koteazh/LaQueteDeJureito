using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsPanelSetUp : MonoBehaviour 
{
	public GameObject specialAttackButton;

	void Start () {
		GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in characters) {
			character.GetComponent<Character.ACharacterStats> ().SetActionsPanel (gameObject);
		}
		gameObject.SetActive (false);
		specialAttackButton.SetActive (false);
	}

	public void ActivateSpecialAttackButton(bool active)
	{
		specialAttackButton.SetActive (active);
	}
}

