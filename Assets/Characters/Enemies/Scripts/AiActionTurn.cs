using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AiActionTurn : MonoBehaviour {

	private GameObject[] existingAi;
	private GameObject[] existingCharacters;
	private List<GameObject> activeAi;
	private GameObject controlledAi;
	private bool initAiTurn = false;
	private bool playerTurn = true;
	private bool isControllingAi = false;
	private bool enemyIsWaiting = true;
	public GameObject victoryScreen;
	public GameObject defeatScreen;

	void Start () {
		activeAi = new List<GameObject> ();
		existingAi = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	void Awake () {
		this.enabled = false;
	}

	public bool GetPlayerTurn()
	{
		return (playerTurn);
	}

	public void SetInitAiTurn(bool _initAiTurn)
	{
		initAiTurn = _initAiTurn;
		if (_initAiTurn == true)
			playerTurn = false;
		else
			playerTurn = true;
	}

	private void GetActiveAi ()
	{
		activeAi = new List<GameObject>();
		existingAi = GameObject.FindGameObjectsWithTag ("Enemy");
		if (existingAi.Length == 0) {
			isControllingAi = false;
			print ("VICTORY");
		}
		foreach (GameObject enemy in existingAi) {
			if (enemy != null) {
				if (enemy.GetComponent<Character.AEnemyStats> ().GetStatus () == Character.E_CharacterStatus.READY) {
					activeAi.Add (enemy);
				}
			}
		}
	}

	private void ResetStatus(GameObject[] charactersArray)
	{
		foreach (GameObject character in charactersArray) {
			character.GetComponent<Character.AStats> ().SetStatus (Character.E_CharacterStatus.READY);
			if (playerTurn == true)
				character.GetComponent<Character.ACharacterStats> ().RemoveWaitBonus ();
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

	private bool CharacterStillActive ()
	{
		foreach (GameObject character in existingCharacters) {
			if (character.GetComponent<Character.AStats> ().GetStatus () != Character.E_CharacterStatus.IS_WAITING){
				return true;
			}
		}
		return false;
	}

	public void checkDefeat()
	{
		if (GameObject.FindGameObjectsWithTag ("Character").Length == 0)
			DisplayDefeatScreen ();
	}

	private void DisplayVictoryScreen()
	{
		victoryScreen.SetActive (true);
	}

	private void DisplayDefeatScreen()
	{
		defeatScreen.SetActive (true);
	}

	// Update is called once per frame
	void Update () {

		if (playerTurn == true) {
			existingCharacters = GameObject.FindGameObjectsWithTag ("Character");
			if (existingCharacters.Length == 0) {
				playerTurn = false;
				DisplayDefeatScreen ();
				print ("GAME OVER");
			} else {
				if (CharacterStillActive () == false) {
					ResetStatus (GameObject.FindGameObjectsWithTag ("Character"));
					playerTurn = false;
				}
			}
		}

		if (initAiTurn == true) {
			ResetStatus (GameObject.FindGameObjectsWithTag ("Enemy"));
			GetActiveAi ();
			if (activeAi.Count != 0) {
				controlledAi = activeAi [0];
				initAiTurn = false;
				isControllingAi = true;
				controlledAi.GetComponent<AiController> ().GetCharactersInRange ();
			} else {
				DisplayVictoryScreen ();
			}
		}

		if (isControllingAi == true) {
			if (controlledAi != null) {
				if (controlledAi.GetComponent<Character.AEnemyStats> ().GetStatus () == Character.E_CharacterStatus.IS_WAITING) {
					activeAi.RemoveAt (0);
					GetActiveAi ();
					if (activeAi.Count != 0) {
						controlledAi = activeAi [0];
						controlledAi.GetComponent<AiController> ().GetCharactersInRange ();
					} else {
						isControllingAi = false;
						playerTurn = true;
					}
				}
			} else {
				initAiTurn = true;
			}
		}
	}
}
