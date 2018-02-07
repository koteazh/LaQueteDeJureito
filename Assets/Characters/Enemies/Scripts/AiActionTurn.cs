using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AiActionTurn : MonoBehaviour {

	private GameObject[] existingAi;
	private List<GameObject> activeAi;
	private GameObject controlledAi;
	private bool initAiTurn = false;
	private bool isControllingAi = false;
	private bool enemyIsWaiting = true;

	void Start () {
		existingAi = GameObject.FindGameObjectsWithTag ("Enemy");
		activeAi = new List<GameObject> ();
	}

	public void SetInitAiTurn(bool _initAiTurn)
	{
		initAiTurn = _initAiTurn;
	}

	private void GetActiveAi ()
	{
		foreach (GameObject enemy in existingAi) {
			if (enemy.GetComponent<Character.AEnemyStats> ().GetStatus () == Character.E_CharacterStatus.READY) {
				activeAi.Add (enemy);
			}
		}
	}

	public void ControlAi()
	{
		GetActiveAi ();
		controlledAi = activeAi [0];
		initAiTurn = false;
		isControllingAi = true;
		controlledAi.GetComponent<AiController> ().GetCharactersInRange ();
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			ControlAi ();
		}
			
	/*	if (initAiTurn == true) {
			GetActiveAi ();
			controlledAi = activeAi [0];
			initAiTurn = false;
			isControllingAi = true;
			controlledAi.GetComponent<AiController> ().GetCharactersInRange ();
		}*/
		if (isControllingAi == true) {
			if (controlledAi.GetComponent<Character.AEnemyStats> ().GetStatus () == Character.E_CharacterStatus.IS_WAITING) {
				activeAi.RemoveAt (0);
				if (activeAi.Count > 0) {
					controlledAi = activeAi [0];
					controlledAi.GetComponent<AiController> ().GetCharactersInRange ();
				}
				else
					isControllingAi = false;
			}
		}
	}
}
