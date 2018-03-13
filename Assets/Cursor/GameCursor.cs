using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Character;

public class GameCursor : MonoBehaviour {
    private int borderTop;
	private int borderBottom;
	private int targetIndex;
	private CombatInfo combatInfoPanel;
	private GameObject target;
	private GameObject currentCase;
	private ACharacterStats selectedCharacter;
	private CharacterPathNode characterPath;
	private List<GameObject> enemiesInAttackRange;
	private List<int> caseInWeaponRange;
	private List<GameObject> enemiesInBowRange;
	private List<int> caseInPointBlankRange;
	private bool highlightActive = false;
	private bool isCharacterSelected = false;
	private bool cursorLocked = false;
	private bool specialAttack = false;
	public GameObject characterInfoPanel;
	public GameObject terrainInfoPanel;
	public bool characterPlacement = true;
    public int caseId;
    public int maxWidthCase;
    public int maxHeightCase;
	public bool levelUpPanelActive = false;

    // Use this for initialization
    void Start () {
        borderTop = maxWidthCase * (maxHeightCase - 1);
        borderBottom = maxWidthCase;
		characterPlacement = true;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 cursor_pos = gameObject.transform.position;

		if (!characterPlacement) {
			if (Input.GetButtonDown ("Up")) {
				if (caseId < borderTop && cursorLocked == false) {
					gameObject.transform.position = new Vector3 (cursor_pos.x, cursor_pos.y, cursor_pos.z + 10);
					caseId += maxWidthCase;
					currentCase = GameObject.Find ("Case_" + caseId.ToString ());
					terrainInfoPanel.GetComponent<TerrainInfo> ().UpdateTerrainInfo (currentCase.GetComponent<Case> ());
					characterInfoPanel.GetComponent<CharacterInfo> ().UpdateCharacterInfo (gameObject.transform.position);
				}
				if (cursorLocked == true && selectedCharacter.GetStatus () == E_CharacterStatus.SELECT_TARGET) {
					E_AttackType attackType;
					if (targetIndex == 0)
						targetIndex = enemiesInAttackRange.Count - 1;
					else
						targetIndex--;
					if (specialAttack)
						attackType = selectedCharacter.gameObject.GetComponent<SkillTree> ().GetSpecialAttackType ();
					else
						attackType = E_AttackType.NORMAL;
					target = enemiesInAttackRange [targetIndex];
					gameObject.transform.position = target.transform.position + new Vector3 (0f, 0.5f, 0f);
					caseId = GetIdByCoord (target.transform.position.x, target.transform.position.z);
					Case enemyCase = GameObject.Find ("Case_" + caseId).GetComponent<Case> ();
					combatInfoPanel.SetCombatPanel (target.GetComponent<AEnemyStats> (), selectedCharacter, enemyCase, attackType);
				}
			}

			if (Input.GetButtonDown ("Down")) {
				if (caseId >= borderBottom && cursorLocked == false) {
					gameObject.transform.position = new Vector3 (cursor_pos.x, cursor_pos.y, cursor_pos.z - 10);
					caseId -= maxWidthCase;
					currentCase = GameObject.Find ("Case_" + caseId.ToString ());
					terrainInfoPanel.GetComponent<TerrainInfo> ().UpdateTerrainInfo (currentCase.GetComponent<Case> ());
					characterInfoPanel.GetComponent<CharacterInfo> ().UpdateCharacterInfo (gameObject.transform.position);
				}
				if (cursorLocked == true && selectedCharacter.GetStatus () == E_CharacterStatus.SELECT_TARGET) {
					E_AttackType attackType;
					if (targetIndex == enemiesInAttackRange.Count - 1)
						targetIndex = 0;
					else
						targetIndex++;
					if (specialAttack)
						attackType = selectedCharacter.gameObject.GetComponent<SkillTree> ().GetSpecialAttackType ();
					else
						attackType = E_AttackType.NORMAL;
					target = enemiesInAttackRange [targetIndex];
					gameObject.transform.position = target.transform.position + new Vector3 (0f, 0.5f, 0f);
					caseId = GetIdByCoord (target.transform.position.x, target.transform.position.z);
					Case enemyCase = GameObject.Find ("Case_" + caseId).GetComponent<Case> ();
					combatInfoPanel.SetCombatPanel (target.GetComponent<AEnemyStats> (), selectedCharacter, enemyCase, attackType);
				}
			}

			if (Input.GetButtonDown ("Left") && caseId % maxWidthCase != 0 && cursorLocked == false) {
				gameObject.transform.position = new Vector3 (cursor_pos.x - 10, cursor_pos.y, cursor_pos.z);
				caseId -= 1;
				currentCase = GameObject.Find ("Case_" + caseId.ToString ());
				terrainInfoPanel.GetComponent<TerrainInfo> ().UpdateTerrainInfo (currentCase.GetComponent<Case> ());
				characterInfoPanel.GetComponent<CharacterInfo> ().UpdateCharacterInfo (gameObject.transform.position);
			}

			if (Input.GetButtonDown ("Right") && (caseId + 1) % maxWidthCase != 0 && cursorLocked == false) {
				gameObject.transform.position = new Vector3 (cursor_pos.x + 10, cursor_pos.y, cursor_pos.z);
				caseId += 1;
				currentCase = GameObject.Find ("Case_" + caseId.ToString ());
				terrainInfoPanel.GetComponent<TerrainInfo> ().UpdateTerrainInfo (currentCase.GetComponent<Case> ());
				characterInfoPanel.GetComponent<CharacterInfo> ().UpdateCharacterInfo (gameObject.transform.position);
			}

			if (Input.GetButtonDown ("Enter")) {
				if (levelUpPanelActive) {
					selectedCharacter.SetLevelUpPanelActive (false);
					levelUpPanelActive = false;
				}
				if (!isCharacterSelected) {
					Collider[] colliders;

					if ((colliders = Physics.OverlapSphere (transform.position, 1f)).Length > 1) {
						foreach (Collider collider in colliders) {
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
							selectedCharacter.DisableAction ("Move", true);
							Vector2 weaponRange = selectedCharacter.GetWeapon ().GetRange ();
							enemiesInAttackRange = new List<GameObject> ();
							caseInWeaponRange = new List<int> ();
							CharacterPathNode weaponRangeArea = new CharacterPathNode (GetIdByCoord (gameObject.transform.position.x, gameObject.transform.position.z));
							GetweaponRangeArea (weaponRangeArea, weaponRange, 0, caseInWeaponRange);
							if (enemiesInAttackRange.Count == 0) {
								selectedCharacter.DisableAction ("Attack", false);
							} else {
								selectedCharacter.DisableAction ("Attack", true);
								SetAttackHightlight (caseInWeaponRange, true);
							}
							if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetSpecialAttackType () == E_AttackType.POINT_BLANK_SHOT) {
								enemiesInBowRange = enemiesInAttackRange;
								enemiesInAttackRange = new List<GameObject> ();
								caseInPointBlankRange = new List<int> ();
								CharacterPathNode pointBlankRangeArea = new CharacterPathNode (GetIdByCoord (gameObject.transform.position.x, gameObject.transform.position.z));
								GetweaponRangeArea (pointBlankRangeArea, new Vector2 (1, 1), 0, caseInPointBlankRange);
								if (enemiesInAttackRange.Count == 0) {
									selectedCharacter.DisableAction ("PointBlankShot", false);
								} else {
									selectedCharacter.DisableAction ("PointBlankShot", true);
									SetAttackHightlight (caseInPointBlankRange, true);
								}
							}
							selectedCharacter.SetActionsPanelActive (true);
							cursorLocked = true;
						}
					}
				} else {
					if (selectedCharacter.GetStatus () == E_CharacterStatus.IN_MOVEMENT) {
						MoveToTargetCase (caseId);
					}

					if (selectedCharacter.GetStatus () == E_CharacterStatus.SELECT_TARGET) {
						if (targetIndex == -1)
							targetIndex = 0;
						else
							Attack ();
					}
				}
			}

			if (isCharacterSelected && selectedCharacter.GetStatus () == E_CharacterStatus.HAS_MOVED) {
				SetMovementHighlight (characterPath, false);
				highlightActive = false;
				selectedCharacter.SetActionsPanelActive (true);
				selectedCharacter.DisableAction ("Move", false);
				cursorLocked = true;
				Vector2 weaponRange = selectedCharacter.GetWeapon ().GetRange ();
				enemiesInAttackRange = new List<GameObject> ();
				caseInWeaponRange = new List<int> ();
				CharacterPathNode weaponRangeArea = new CharacterPathNode (GetIdByCoord (gameObject.transform.position.x, gameObject.transform.position.z));
				GetweaponRangeArea (weaponRangeArea, weaponRange, 0, caseInWeaponRange);
				if (enemiesInAttackRange.Count == 0) {
					selectedCharacter.DisableAction ("Attack", false);
				} else {
					selectedCharacter.DisableAction ("Attack", true);
					SetAttackHightlight (caseInWeaponRange, true);
				}
				if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetSpecialAttackType () == E_AttackType.POINT_BLANK_SHOT) {
					enemiesInBowRange = enemiesInAttackRange;
					enemiesInAttackRange = new List<GameObject> ();
					caseInPointBlankRange = new List<int> ();
					CharacterPathNode pointBlankRangeArea = new CharacterPathNode (GetIdByCoord (gameObject.transform.position.x, gameObject.transform.position.z));
					GetweaponRangeArea (pointBlankRangeArea, new Vector2 (1, 1), 0, caseInPointBlankRange);
					if (enemiesInAttackRange.Count == 0) {
						selectedCharacter.DisableAction ("PointBlankShot", false);
					} else {
						selectedCharacter.DisableAction ("PointBlankShot", true);
						SetAttackHightlight (caseInPointBlankRange, true);
					}
				}


			}

			if (isCharacterSelected && selectedCharacter.GetStatus () == E_CharacterStatus.IS_WAITING) {
				isCharacterSelected = false;
			}
		}
		if (characterPlacement && gameObject.GetComponent<CharacterPlacement> ().enabled == false) {
			characterPlacement = false;
			gameObject.GetComponent<AiActionTurn> ().enabled = true;
			currentCase = GameObject.Find ("Case_" + caseId);
			terrainInfoPanel.GetComponent<TerrainInfo> ().UpdateTerrainInfo (currentCase.GetComponent<Case> ());
			terrainInfoPanel.SetActive (true);
		}
    }

	public void SetMovementHighlight (CharacterPathNode characterPath, bool activate) {
		if (GameObject.Find ("Case_" + characterPath.caseId).transform.Find ("MovementHighlight").gameObject.activeSelf != activate)
			GameObject.Find ("Case_" + characterPath.caseId).transform.Find ("MovementHighlight").gameObject.SetActive (activate);
		if (characterPath.children != null) {
			foreach (CharacterPathNode child in characterPath.children) {
				SetMovementHighlight (child, activate);	
			}
		}
		return;
	}

	private void SetAttackHightlight (List<int> caseList, bool activate)
	{
		foreach (int _caseId in caseList) {
			if (GameObject.Find ("Case_" + _caseId).transform.Find ("AttackHighlight").gameObject.activeSelf != activate)
				GameObject.Find ("Case_" + _caseId).transform.Find ("AttackHighlight").gameObject.SetActive (activate);
		}
	}

	public void GetCharacterPath(CharacterPathNode characterPath, int movement)
	{
		GameObject nextCase;
		int nextMovement;

		if (movement > 0)
		{
			foreach (E_Direction value in Enum.GetValues(typeof(E_Direction))) {
				nextCase = GetCaseByDirection (characterPath.caseId, value);
				if (nextCase != null) {
					if (System.Object.ReferenceEquals (null, characterPath.parent) || nextCase.GetComponent<Case> ().case_id != characterPath.parent.caseId) {
						if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetPassiveSkill () == E_PassiveSkill.ADVENTURER)
							nextMovement = movement - 1;
						else
							nextMovement = movement - nextCase.GetComponent<Case> ().getType ().movement_cost;
						if (nextMovement >= 0) {
							CharacterPathNode newCharacterPathNode = new CharacterPathNode (nextCase.GetComponent<Case> ().case_id, characterPath.pathValue + nextCase.GetComponent<Case> ().getType ().movement_cost);
							newCharacterPathNode.parent = characterPath;
							characterPath.Add (newCharacterPathNode);
							GetCharacterPath (newCharacterPathNode, nextMovement);
						}
					}
				}
			}
			return;
		}
		return;
	}

	private void MoveToTargetCase(int caseId)
	{
		SetAttackHightlight(caseInWeaponRange, false);
		List<int> pathToTarget =  characterPath.GetShortestPathToTarget (caseId);
		if (!System.Object.ReferenceEquals (null, pathToTarget)) {
			selectedCharacter.GetComponentInParent<MovementController> ().MoveToTarget (pathToTarget);
			cursorLocked = true;
		}
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
		SetMovementHighlight (characterPath, true);
		highlightActive = true;
	}

	public void Wait ()
	{
		selectedCharacter.AddWaitBonus ();
		selectedCharacter.SetActionsPanelActive (false);
		selectedCharacter.SetStatus (E_CharacterStatus.IS_WAITING);
		if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetWaitSkill() == E_WaitSkill.LAYTRAP) {
			foreach (E_Direction direction in Enum.GetValues(typeof(E_Direction))) {
				GameObject adjacentCase = GetCaseByDirection (caseId, direction);
				Instantiate (selectedCharacter.GetComponent<RangerSkillTree> ().trap, adjacentCase.transform);
			}
		}
		if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetPassiveSkill() == E_PassiveSkill.REGENERATINON_AURA) {
			Collider[] colliders;
			foreach (E_Direction direction in Enum.GetValues(typeof(E_Direction)))
			{
				GameObject adjacentCase = GetCaseByDirection (caseId, direction);
				if ((colliders = Physics.OverlapSphere (adjacentCase.transform.position, 1f)).Length > 1) {
					foreach (Collider collider in colliders) {
						GameObject go = collider.gameObject; 
						if (go.tag == "Character") {
							go.GetComponent<AStats> ().ModifyCurrentHealth(-5);
							break;
						}
					}
				}
			}
		}
		cursorLocked = false;
		SetAttackHightlight (caseInWeaponRange, false);
		GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in characters) {
			if (character.GetComponent<ACharacterStats> ().GetStatus () == E_CharacterStatus.READY)
				return;
		}
		gameObject.GetComponent<AiActionTurn> ().SetInitAiTurn(true);
	}

	private bool CaseToExclude(int caseId, E_Direction direction)
	{
		if (caseId == GetIdByCoord (gameObject.transform.position.x, gameObject.transform.position.z))
			return (true);
		if (caseId % maxWidthCase == 0 && direction == E_Direction.RIGHT)
			return (true);
		if ((caseId + 1) % maxWidthCase == 0 && direction == E_Direction.LEFT)
			return (true);
		return (false);
	}

	private void GetweaponRangeArea (CharacterPathNode weaponRangeArea, Vector2 weaponRange, int currentRange, List<int> caseInWeaponRange)
	{
		GameObject nextCase;

		currentRange++;
		foreach (E_Direction direction in Enum.GetValues(typeof(E_Direction))) {
			nextCase = GetCaseByDirection (weaponRangeArea.caseId, direction);
			if (nextCase != null) {
				CharacterPathNode nextNode = new CharacterPathNode (nextCase.GetComponent<Case> ().case_id);
				if (currentRange < weaponRange.x) {
					GetweaponRangeArea (nextNode, weaponRange, currentRange, caseInWeaponRange);
				} else if (currentRange >= weaponRange.x && currentRange <= weaponRange.y) {
					bool inExclusionCase = CaseToExclude (nextCase.GetComponent<Case> ().case_id, direction);
					if (!caseInWeaponRange.Contains (nextCase.GetComponent<Case> ().case_id) && !inExclusionCase) {
						caseInWeaponRange.Add (nextCase.GetComponent<Case> ().case_id);
						AddEnemiesInAttackRange (nextCase.GetComponent<Case> ().case_id);
					}
					if (!inExclusionCase)
						GetweaponRangeArea (nextNode, weaponRange, currentRange, caseInWeaponRange);
				}
			}
		}
	}

	private void AddEnemiesInAttackRange(int _caseId)
	{
		Collider[] colliders;
		if ((colliders = Physics.OverlapSphere (Get3dCoordById (_caseId), 1f)).Length > 1) {
			foreach (Collider collider in colliders) {
				GameObject go = collider.gameObject; 
				if (go.tag == "Enemy") {
					enemiesInAttackRange.Add (go);
					break;
				}
			}
		}
	}

	private void Attack()
	{
		int hitChance;
		int damage;
		int hitScore = UnityEngine.Random.Range (0, 100);
		E_AttackType attackType;

		if (specialAttack)
			attackType = selectedCharacter.gameObject.GetComponent<SkillTree> ().GetSpecialAttackType ();
		else
			attackType = E_AttackType.NORMAL;
		hitChance = combatInfoPanel.GetHitChance(target.GetComponent<AStats>(), selectedCharacter, GameObject.Find("Case_" + GetIdByCoord(target.transform.position.x, target.transform.position.z)).GetComponent<Case>(), attackType);
		damage = combatInfoPanel.GetDamage (target.GetComponent<AStats>(), selectedCharacter, attackType);
		selectedCharacter.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
		if (hitScore <= hitChance) {
			if (attackType == E_AttackType.LIFE_LEECH)
				selectedCharacter.ModifyCurrentHealth (damage / -2);
			if (attackType == E_AttackType.INTIMIDATING_STRIKE)
				target.GetComponent<AStats> ().ModifyCharacterStat("Strength", -2);
			if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetPassiveSkill() == E_PassiveSkill.ERRATIC_BOUNCE) {
				GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
				int randomTargetIndex = UnityEngine.Random.Range (0, enemies.Length - 1);
				enemies [randomTargetIndex].GetComponent<AStats> ().ModifyCurrentHealth (5);
			}
			target.GetComponent<AStats> ().ModifyCurrentHealth (damage);
			target.GetComponent<Animator> ().SetBool ("isHit", true);
			selectedCharacter.gameObject.GetComponent<ACharacterStats> ().GainExperience(damage, target.GetComponent<AEnemyStats> ().GetLevel() - selectedCharacter.gameObject.GetComponent<ACharacterStats> ().GetLevel());
		} else {
			target.GetComponent<Animator> ().SetBool ("hasDodge", true);
			selectedCharacter.gameObject.GetComponent<ACharacterStats> ().GainExperience(5, target.GetComponent<AEnemyStats> ().GetLevel() - selectedCharacter.gameObject.GetComponent<ACharacterStats> ().GetLevel());
		}
		combatInfoPanel.gameObject.SetActive(false);
		SetAttackHightlight (caseInWeaponRange, false);
		selectedCharacter.SetStatus (E_CharacterStatus.IS_WAITING);
		if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetPassiveSkill() == E_PassiveSkill.REGENERATINON_AURA) {
			Collider[] colliders;
			foreach (E_Direction direction in Enum.GetValues(typeof(E_Direction)))
			{
				GameObject adjacentCase = GetCaseByDirection (caseId, direction);
				if ((colliders = Physics.OverlapSphere (adjacentCase.transform.position, 1f)).Length > 1) {
					foreach (Collider collider in colliders) {
						GameObject go = collider.gameObject; 
						if (go.tag == "Character") {
							go.GetComponent<AStats> ().ModifyCurrentHealth(-5);
							break;
						}
					}
				}
			}
		}
		cursorLocked = false;
		isCharacterSelected = false;

		GameObject[] characters = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in characters) {
			if (character.GetComponent<ACharacterStats> ().GetStatus () == E_CharacterStatus.READY)
				return;
		}
		gameObject.GetComponent<AiActionTurn> ().SetInitAiTurn(true);
	}

	public void InitAttack ()
	{
		selectedCharacter.SetActionsPanelActive (false);
		selectedCharacter.SetStatus (E_CharacterStatus.SELECT_TARGET);
		cursorLocked = true;
		targetIndex = -1;
		if (selectedCharacter.gameObject.GetComponent<SkillTree> ().GetSpecialAttackType () == E_AttackType.POINT_BLANK_SHOT && (!UnityEngine.Object.ReferenceEquals(null, enemiesInBowRange) || enemiesInBowRange.Count == 0))
			target = enemiesInBowRange [0];
		else
			target = enemiesInAttackRange [0];
		gameObject.transform.position = target.transform.position + new Vector3 (0f, 0.5f, 0f);
		caseId = GetIdByCoord (target.transform.position.x, target.transform.position.z);
		Case enemyCase = GameObject.Find ("Case_" + caseId).GetComponent<Case> ();
		combatInfoPanel.SetCombatPanel (target.GetComponent<AEnemyStats>(), selectedCharacter, enemyCase, E_AttackType.NORMAL);
		combatInfoPanel.gameObject.SetActive(true);
	}

	public void InitSpecialAttack ()
	{
		specialAttack = true;
		selectedCharacter.SetActionsPanelActive (false);
		selectedCharacter.SetStatus (E_CharacterStatus.SELECT_TARGET);
		cursorLocked = true;
		targetIndex = -1;
		target = enemiesInAttackRange [0];
		gameObject.transform.position = target.transform.position + new Vector3 (0f, 0.5f, 0f);
		caseId = GetIdByCoord (target.transform.position.x, target.transform.position.z);
		Case enemyCase = GameObject.Find ("Case_" + caseId).GetComponent<Case> ();
		combatInfoPanel.SetCombatPanel (target.GetComponent<AEnemyStats>(), selectedCharacter, enemyCase, selectedCharacter.gameObject.GetComponent<SkillTree>().GetSpecialAttackType());
		combatInfoPanel.gameObject.SetActive(true);
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

	public int GetIdByCoord(float x, float y)
	{
		int _x = (int)Math.Round (x, 0);
		int _y = (int)Math.Round (y, 0);
		return (int) (((_x - 5) / 10)	+ (((_y - 5) / 10) * maxWidthCase));
	}

	public Vector2 GetCoordById(int id)
	{
		Vector2 coord = new Vector2 ();
		coord.x = (id - ((id / maxWidthCase) * maxWidthCase)) * 10 + 5;
		coord.y = (id / maxWidthCase) * 10 + 5;
		return coord;
	}

	public Vector3 Get3dCoordById(int id)
	{
		Vector3 coord = new Vector3 ();
		coord.x = (id - ((id / maxWidthCase) * maxWidthCase)) * 10 + 5;
		coord.y = 0.5f;
		coord.z = (id / maxWidthCase) * 10 + 5;
		return coord;
	}

	public void SetUpCombatInfoPanel (CombatInfo _combatInfoPanel)
	{
		combatInfoPanel = _combatInfoPanel;
	}

	public void EndCharacterPlacement()
	{
		characterPlacement = false;
		gameObject.GetComponent<AiActionTurn> ().enabled = true;
		print(gameObject.GetComponent<AiActionTurn> ().isActiveAndEnabled);
	}
}
