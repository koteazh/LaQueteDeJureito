using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Character;

public class GameCursor : MonoBehaviour {
    private int borderTop;
    private int borderBottom;
    private GameObject currentCase;
	private ACharacterStats selectedCharacter;
	CharacterPathNode characterPath;
	private bool highlightActive = false;
	private bool levelUpPanelActive = false;
	private bool isCharacterSelected = false;
	private bool cursorLocked = false;
    public int caseId;
    public int maxWidthCase;
    public int maxHeightCase;

    // Use this for initialization
    void Start () {
        borderTop = maxWidthCase * (maxHeightCase - 1);
        borderBottom = maxWidthCase;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 cursor_pos = gameObject.transform.position;

		if (Input.GetButtonDown ("Up") && caseId < borderTop && cursorLocked == false)
        {
            gameObject.transform.position = new Vector3(cursor_pos.x, cursor_pos.y, cursor_pos.z + 10);
            caseId += maxWidthCase;
            currentCase = GameObject.Find("Case_" + caseId.ToString());
        }

		if (Input.GetButtonDown ("Down") && caseId >= borderBottom && cursorLocked == false)
        {
            gameObject.transform.position = new Vector3(cursor_pos.x, cursor_pos.y, cursor_pos.z - 10);
            caseId -= maxWidthCase;
            currentCase = GameObject.Find("Case_" + caseId.ToString());
        }

		if (Input.GetButtonDown ("Left") && caseId % maxWidthCase != 0 && cursorLocked == false)
        {
            gameObject.transform.position = new Vector3(cursor_pos.x - 10, cursor_pos.y, cursor_pos.z);
            caseId -= 1;
            currentCase = GameObject.Find("Case_" + caseId.ToString());
        }

		if (Input.GetButtonDown ("Right") && (caseId + 1) % maxWidthCase!= 0 && cursorLocked == false)
		{
			gameObject.transform.position = new Vector3(cursor_pos.x + 10, cursor_pos.y, cursor_pos.z);
			caseId += 1;
			currentCase = GameObject.Find("Case_" + caseId.ToString());
		}

		if (Input.GetButtonDown ("Enter"))
		{
			Collider[] colliders;

			if((colliders = Physics.OverlapSphere(transform.position, 1f)).Length > 1)
			{
				foreach(Collider collider in colliders)
				{
					GameObject go = collider.gameObject; 
					if (go.tag == "Character") {
						selectedCharacter = go.GetComponent<Character.ACharacterStats> ();
						isCharacterSelected = true;
						break;
					}
				}
			}

			if (isCharacterSelected) {
				if (selectedCharacter.GetStatus () == E_CharacterStatus.READY) {
					selectedCharacter.DisableMovement (true);
					selectedCharacter.SetActionsPanelActive(true);
					cursorLocked = true;
				}

				if (selectedCharacter.GetStatus () == E_CharacterStatus.IN_MOVEMENT) {
					MoveToTargetCase (caseId);
				}
				
			}

			/*
			//				HIGHLIGHT MOVEMENT CASE
			int characterCaseId = GetIdByCoord (selectedCharacter.transform.position.x, selectedCharacter.transform.position.z);
			CharacterPathNode characterPath = new CharacterPathNode (characterCaseId);
			List<CharacterPathNode> lastNodes = new List<CharacterPathNode> ();
			GetCharacterPath (characterPath, selectedCharacter.GetComponent<ACharacterStats> ().GetCharacterStats ("movement"));
			if (highlightActive) {
				SetActiveHighlight (characterPath, false);
				highlightActive = false;
			} else {
				SetActiveHighlight (characterPath, true);
				highlightActive = true;
			}
			*/


			//				LEVEL UP CALL
			//selectedCharacter.GetComponent<ACharacterStats> ().LevelUp ();

		}

		if (isCharacterSelected && selectedCharacter.GetStatus () == E_CharacterStatus.HAS_MOVED) {
			SetActiveHighlight (characterPath, false);
			highlightActive = false;
			selectedCharacter.SetActionsPanelActive (true);
			selectedCharacter.DisableMovement (false);
			cursorLocked = true;
		}

    }

	public void SetActiveHighlight (CharacterPathNode characterPath, bool activate) {
		if (GameObject.Find ("Case_" + characterPath.caseId).transform.Find ("MovementHighlight").gameObject.activeSelf != activate)
			GameObject.Find ("Case_" + characterPath.caseId).transform.Find ("MovementHighlight").gameObject.SetActive (activate);
		if (characterPath.children != null) {
			foreach (CharacterPathNode child in characterPath.children) {
				SetActiveHighlight (child, activate);	
			}
		}
		return;
	}

	public void GetCharacterPath(CharacterPathNode characterPath, int movement)
	{
		GameObject nextCase;
		int nextMovement;

		if (movement > 0)
		{
			foreach (E_Direction value in Enum.GetValues(typeof(E_Direction))) {
				nextCase = GetCaseByDirection (characterPath.caseId, value);
				if (System.Object.ReferenceEquals (null, characterPath.parent) || nextCase.GetComponent<Case> ().case_id != characterPath.parent.caseId) {
					nextMovement = movement - nextCase.GetComponent<Case> ().getType ().movement_cost;
					if (nextMovement >= 0) {
						CharacterPathNode newCharacterPathNode = new CharacterPathNode (nextCase.GetComponent<Case> ().case_id, characterPath.pathValue + nextCase.GetComponent<Case> ().getType ().movement_cost);
						newCharacterPathNode.parent = characterPath;
						characterPath.Add (newCharacterPathNode);
						GetCharacterPath (newCharacterPathNode, nextMovement);
					}
				}
			}
			return;
		}
		return;
	}

	private void MoveToTargetCase(int caseId)
	{
		List<int> pathToTarget =  characterPath.GetShortestPathToTarget (caseId);
		if (!System.Object.ReferenceEquals(null, pathToTarget))
			selectedCharacter.GetComponentInParent<MovementController> ().MoveToTarget(pathToTarget);
	}

	public void InitCharacterMovement ()
	{
		int characterCaseId = GetIdByCoord (selectedCharacter.transform.position.x, selectedCharacter.transform.position.z);
		characterPath = new CharacterPathNode (characterCaseId);
		List<CharacterPathNode> lastNodes = new List<CharacterPathNode> ();
		selectedCharacter.SetStatus (E_CharacterStatus.IN_MOVEMENT);
		cursorLocked = false;
		GetCharacterPath (characterPath, selectedCharacter.GetComponent<ACharacterStats> ().GetCharacterStats ("Movement"));
		selectedCharacter.SetActionsPanelActive (false);
		SetActiveHighlight (characterPath, true);
		highlightActive = true;
	}

	public void Wait ()
	{
		selectedCharacter.SetActionsPanelActive (false);
		selectedCharacter.SetStatus (E_CharacterStatus.IS_WAITING);
		cursorLocked = false;
		GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in characters) {
			if (character.GetComponent<ACharacterStats> ().GetStatus () == E_CharacterStatus.READY)
				return;
		}
		gameObject.GetComponent<AiActionTurn> ().SetInitAiTurn(true);
	}

	public void Attack ()
	{
		selectedCharacter.SetActionsPanelActive (false);
		selectedCharacter.SetStatus (E_CharacterStatus.IS_WAITING);
		cursorLocked = false;
	}

	private GameObject GetCaseByDirection(int caseId, E_Direction direction)
	{
		GameObject wantedCase;
		int wantedCaseId;

		switch (direction) {
		case E_Direction.UP:
			wantedCaseId = caseId + maxWidthCase;
			wantedCase = GameObject.Find ("Case_" + wantedCaseId);
			return wantedCase;

		case E_Direction.DOWN:
			wantedCaseId = caseId - maxWidthCase;
			wantedCase = GameObject.Find ("Case_" + wantedCaseId);
			return wantedCase;

		case E_Direction.LEFT:
			wantedCaseId = caseId - 1;
			wantedCase = GameObject.Find ("Case_" + wantedCaseId);
			return wantedCase;

		case E_Direction.RIGHT:
			wantedCaseId = caseId + 1;
			wantedCase = GameObject.Find ("Case_" + wantedCaseId);
			return wantedCase;

		default :
			return null;
		}
	}

	private int GetIdByCoord(float x, float y)
	{
		int _x = (int)Math.Round (x, 0);
		int _y = (int)Math.Round (y, 0);
		return (int) (((_x - 5) / 10)	+ (((_y - 5) / 10) * maxWidthCase));
	}

	private Vector2 GetCoordById(int id)
	{
		Vector2 coord = new Vector2 ();
		coord.x = (id - ((id / maxWidthCase) * maxWidthCase)) * 10 + 5;
		coord.y = (id / maxWidthCase) * 10 + 5;
		return coord;
	}

	private Vector3 Get3dCoordById(int id)
	{
		Vector3 coord = new Vector3 ();
		coord.x = (id - ((id / maxWidthCase) * maxWidthCase)) * 10 + 5;
		coord.y = 0.5f;
		coord.z = (id / maxWidthCase) * 10 + 5;
		return coord;
	}
}
