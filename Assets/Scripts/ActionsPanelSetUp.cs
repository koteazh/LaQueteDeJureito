using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsPanelSetUp : MonoBehaviour 
	{
		void Start () {
			GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
			foreach (GameObject character in characters) {
				character.GetComponent<Character.ACharacterStats> ().SetActionsPanel (gameObject);
			}
			gameObject.SetActive (false);
		}
	}

