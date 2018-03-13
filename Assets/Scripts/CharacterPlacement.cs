using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPlacement : MonoBehaviour {
	public int[] starterCaseId;
	public GameObject endTurnButton;
	public GameObject characterSelectionPanel;
	public GameObject levelUpPanel;
	public GameObject actionPanel;
	public GameObject skillTreePanel;
	private GameObject[] characters;
	public bool isAssassinPlaced = false;
	public bool isMagePlaced = false;
	public bool isRangerPlaced = false;
	public bool isWarriorPlaced = false;
	private int caseIndex = 0;
	private bool selectingCharacter = false;
	private bool caseOccupied = false;
	private GameObject placedCharacter;

	void Start()
	{
		characters = GameObject.Find ("CharacterHolder").GetComponent<CharacterHolder> ().characters;
	}

	void Update()
	{
		if (!selectingCharacter) {
			if (Input.GetButtonDown ("Up")) {
				if (caseIndex == starterCaseId.Length - 1)
					caseIndex = 0;
				else
					caseIndex += 1; 
				gameObject.transform.position = gameObject.GetComponent<GameCursor> ().Get3dCoordById (starterCaseId [caseIndex]) + new Vector3 (0f, 0.5f, 0f);
			}

			if (Input.GetButtonDown ("Down")) {
				if (caseIndex == 0)
					caseIndex = starterCaseId.Length - 1;
				else
					caseIndex -= 1; 
				gameObject.transform.position = gameObject.GetComponent<GameCursor> ().Get3dCoordById (starterCaseId [caseIndex]) + new Vector3 (0f, 0.5f, 0f);
			}

			if (Input.GetButtonDown ("Enter")) {
				Collider[] colliders;

				caseOccupied = false;
				if ((colliders = Physics.OverlapSphere (transform.position, 5f)).Length > 1) {
					foreach (Collider collider in colliders) {
						GameObject go = collider.gameObject; 
						if (go.tag == "Character") {
							placedCharacter = go;
							caseOccupied= true;
							break;
						}
					}
				}
				if (!caseOccupied) {
					selectingCharacter = true;
					characterSelectionPanel.SetActive (true);
				} else {
					characterSelectionPanel.transform.Find (placedCharacter.GetComponent<Character.ACharacterStats> ().GetCharacterClass () + "Button").GetComponent<Button> ().interactable = true;
					if (placedCharacter.name.Contains ("Assassin"))
						isAssassinPlaced = false;
					else if (placedCharacter.name.Contains ("Mage"))
						isMagePlaced = false;
					else if (placedCharacter.name.Contains ("Ranger"))
						isRangerPlaced = false;
					else
						isWarriorPlaced = false;
					placedCharacter.SetActive(false);
				}
			}
		}
		if (!characterSelectionPanel.activeInHierarchy && selectingCharacter)
			selectingCharacter = false;
		if (isAssassinPlaced && isMagePlaced && isRangerPlaced && isWarriorPlaced)
			GameObject.Find ("FightButton").GetComponent<Button> ().interactable = true;
		else
			GameObject.Find ("FightButton").GetComponent<Button> ().interactable = false;
	}

	public void BeginFight()
	{
		foreach (int caseId in starterCaseId) {
			gameObject.GetComponent<GameCursor> ().caseId = gameObject.GetComponent<GameCursor> ().GetIdByCoord (gameObject.transform.position.x, gameObject.transform.position.z);
			GameObject.Find("Case_" + caseId).transform.Find("StarterHighlight").gameObject.SetActive(false);
			this.enabled = false;
		}
		endTurnButton.SetActive (true);
		GameObject.Find ("FightButton").SetActive(false);
	}
	public void PlaceCharacter()
	{
		GameObject button = EventSystem.current.currentSelectedGameObject;
		characterSelectionPanel.SetActive (false);
		if (button.name.Contains ("Assassin")) {
			characters [0].transform.position = gameObject.transform.position;
			characters [0].transform.rotation = new Quaternion(0f,45f,0f,0f);
			characters [0].SetActive (true);
			characters [0].GetComponent<Character.ACharacterStats> ().SetLevelUpPanel (levelUpPanel);
			characters [0].GetComponent<Character.ACharacterStats> ().SetActionsPanel (actionPanel);
			characters [0].GetComponent<Character.ACharacterStats> ().SetSkillTreePanel (skillTreePanel);
			isAssassinPlaced = true;
			characterSelectionPanel.transform.Find ("AssassinButton").GetComponent<Button> ().interactable = false;
		} else if (button.name.Contains ("Mage")) {
			characters [1].transform.position = gameObject.transform.position;
			characters [1].transform.rotation = new Quaternion(0f,45f,0f,0f);
			characters [1].SetActive (true);
			characters [1].GetComponent<Character.ACharacterStats> ().SetLevelUpPanel (levelUpPanel);
			characters [1].GetComponent<Character.ACharacterStats> ().SetActionsPanel (actionPanel);
			characters [1].GetComponent<Character.ACharacterStats> ().SetSkillTreePanel (skillTreePanel);
			isMagePlaced = true;
			characterSelectionPanel.transform.Find ("MageButton").GetComponent<Button> ().interactable = false;
		} else if (button.name.Contains ("Ranger")) {
			characters [2].transform.position = gameObject.transform.position;
			characters [2].transform.rotation = new Quaternion(0f,10f,0f,0f);
			characters [2].SetActive (true);
			characters [2].GetComponent<Character.ACharacterStats> ().SetLevelUpPanel (levelUpPanel);
			characters [2].GetComponent<Character.ACharacterStats> ().SetActionsPanel (actionPanel);
			characters [2].GetComponent<Character.ACharacterStats> ().SetSkillTreePanel (skillTreePanel);
			isRangerPlaced = true;
			characterSelectionPanel.transform.Find ("RangerButton").GetComponent<Button> ().interactable = false;
		} else {
			characters [3].transform.position = gameObject.transform.position;
			characters [3].transform.rotation = new Quaternion(0f,45f,0f,0f);
			characters [3].SetActive (true);
			characters [3].GetComponent<Character.ACharacterStats> ().SetLevelUpPanel (levelUpPanel);
			characters [3].GetComponent<Character.ACharacterStats> ().SetActionsPanel (actionPanel);
			characters [3].GetComponent<Character.ACharacterStats> ().SetSkillTreePanel (skillTreePanel);
			isWarriorPlaced = true;
			characterSelectionPanel.transform.Find ("WarriorButton").GetComponent<Button> ().interactable = false;
		}
	}
}
