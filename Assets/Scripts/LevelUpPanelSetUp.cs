using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanelSetUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in characters) {
			character.GetComponent<Character.ACharacterStats> ().SetLevelUpPanel (gameObject);
		}
		gameObject.SetActive (false);
	}
}
