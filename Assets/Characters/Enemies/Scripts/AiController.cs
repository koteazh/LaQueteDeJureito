using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiController : MonoBehaviour {

	private List <GameObject> charactersInRange = new List<GameObject> ();
	private Dictionary<GameObject, AiPath> pathsToTarget = new Dictionary<GameObject, AiPath> ();
	private GameObject target;
	private Vector3 movementTarget;
	private GameObject closestTarget;
	private int smallestMovementCost = -1;
	private List<int> shortestPath;
	private bool isTargetSelected = false;
	private bool is_moving = false;
	private bool target_reached = false;
	int i = 0;
	private int mapRowLength;


	void Start()
	{
		mapRowLength = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainRowLength>().GetTerrainTilesPerRow();
	}

	public void SetIsTargetSelected (bool _isTargetSelected)
	{
		isTargetSelected = _isTargetSelected;
	}

	private int GetIdByCoord(float x, float y)
	{
		int _x = (int)Math.Round (x, 0);
		int _y = (int)Math.Round (y, 0);
		return (int) (((_x - 5) / 10)	+ (((_y - 5) / 10) * mapRowLength));
	}

	private Vector2 GetCoordById(int id)
	{
		Vector2 coord = new Vector2 ();
		coord.x = (id - ((id / mapRowLength) * mapRowLength)) * 10 + 5;
		coord.y = (id / mapRowLength) * 10 + 5;
		return coord;
	}

	private Vector3 Get3dCoordById(int id)
	{
		Vector3 coord = new Vector3 ();
		coord.x = (id - ((id / mapRowLength) * mapRowLength)) * 10 + 5;
		coord.y = 0.5f;
		coord.z = (id / mapRowLength) * 10 + 5;
		return coord;
	}
		
	private int GetTargetDistance (GameObject targetToReach)
	{
		int _x = (int)(Math.Round (gameObject.transform.position.x, 0) - Math.Round (targetToReach.transform.position.x, 0));
		int _y = (int)(Math.Round (gameObject.transform.position.z, 0) - Math.Round (targetToReach.transform.position.z, 0));
		int xDiff = (int) Math.Abs(_x) / 10;
		int yDiff = (int) Math.Abs(_y) / 10;
		int totalDiff = xDiff + yDiff;
		return (totalDiff);
	}

	public void GetCharactersInRange()
	{
		int totalMovementCost = 0;
		GameObject[] characterPresent = GameObject.FindGameObjectsWithTag ("Character");
		Collider[] colliders;

		foreach (GameObject potentialTarget in characterPresent) {
			AiPath path = new AiPath (GetIdByCoord(gameObject.transform.position.x, gameObject.transform.position.z));

			target = potentialTarget;
			GetPathsToTarget (0, path);
			shortestPath = path.GetShortestPath (path);
			totalMovementCost = 0;
			foreach (int caseId in shortestPath) {
				GameObject terrainCase = GameObject.Find ("Case_" + caseId);
				totalMovementCost += terrainCase.GetComponent<Case> ().getType().movement_cost;

				if((colliders = Physics.OverlapSphere(Get3dCoordById(caseId), 5f)).Length > 1)
				{
					foreach(Collider collider in colliders)
					{
						GameObject go = collider.gameObject;
						if (go.tag == "Character" || go.tag == "Enemy") {
							totalMovementCost += 100;
						}
					}
				}

			}
			if (smallestMovementCost == -1 || totalMovementCost < smallestMovementCost) {
				smallestMovementCost = totalMovementCost;
				closestTarget = potentialTarget;
			}
			if (totalMovementCost <= gameObject.GetComponent<Character.AEnemyStats> ().GetCharacterStats ("Movement")) {
				charactersInRange.Add (potentialTarget);
				pathsToTarget.Add (potentialTarget, path);
			}
		}
		if (charactersInRange.Count >= 2) {
			SelectTarget (charactersInRange);
		} else if (charactersInRange.Count == 1) {
			target = charactersInRange [0];
		} else {
			target = closestTarget;
			AiPath path = new AiPath (GetIdByCoord(gameObject.transform.position.x, gameObject.transform.position.z));
			GetPathsToTarget (0, path);
			pathsToTarget [target] = path;
		}
		isTargetSelected  = true;

	}

	private void SelectKillableTarget (List<GameObject> killableTargetList, string defenseStatName)
	{
		List<GameObject> targetsAgility = new List<GameObject>();
		List<GameObject> targetsDefense = new List<GameObject>();
		List<GameObject> targetsPower = new List<GameObject>();
		List<GameObject> targetsAim = new List<GameObject>();
		int valueToCompare;

		killableTargetList = killableTargetList.OrderBy (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats("Agility")).ToList ();
		valueToCompare = killableTargetList [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats("Agility");
		foreach (GameObject possibleTarget in killableTargetList)
		{
			if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Agility")) {
				targetsAgility.Add (possibleTarget);
			}
		}

		if (targetsAgility.Count >= 2)
		{
			targetsAgility = targetsAgility.OrderByDescending (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats (defenseStatName)).ToList ();
			valueToCompare = targetsAgility [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats (defenseStatName);
			foreach (GameObject possibleTarget in targetsAgility)
			{
				if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats (defenseStatName))
					targetsDefense.Add (possibleTarget);
			}

			if (targetsDefense.Count >= 2)
			{
				targetsDefense = targetsDefense.OrderByDescending (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Strength")).ToList ();
				valueToCompare = targetsDefense [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Strength");
				foreach (GameObject possibleTarget in targetsDefense)
				{
					if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Strength"))
						targetsPower.Add (possibleTarget);
				}

				if (targetsPower.Count >= 2)
				{
					targetsPower = targetsPower.OrderByDescending (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Precision")).ToList ();
					valueToCompare = targetsPower [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Precision");
					foreach (GameObject possibleTarget in targetsPower)
					{
						if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Precision"))
							targetsAim.Add (possibleTarget);
					}
					target = targetsAim [0];
				}
				else
					target = targetsPower [0];
			}
			else
				target = targetsDefense [0];
		}
		else
			target = targetsAgility [0];
	}

	private void SelectDamageableTarget (List<GameObject> targetArray, string defenseStatName)
	{
		List<GameObject> targetList = new List<GameObject>();
		List<GameObject> targetsLife = new List<GameObject>();
		List<GameObject> targetsDefense = new List<GameObject>();
		List<GameObject> targetsPower = new List<GameObject>();
		List<GameObject> targetsAim = new List<GameObject>();
		int valueToCompare;

		foreach (GameObject _target in targetArray)
		{
			targetList.Add (_target);
		}
		targetList = targetList.OrderBy (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats(defenseStatName)).ToList ();
		valueToCompare = targetList [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats(defenseStatName);
		foreach (GameObject possibleTarget in targetList)
		{
			if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats (defenseStatName))
				targetsDefense.Add (possibleTarget);
		}

		if (targetsDefense.Count >= 2)
		{
			targetsDefense = targetsDefense.OrderBy (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Life")).ToList ();
			valueToCompare = targetsDefense [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Life");
			foreach (GameObject possibleTarget in targetsDefense)
			{
				if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Life"))
					targetsLife.Add (possibleTarget);
			}

			if (targetsLife.Count >= 2)
			{
				targetsLife = targetsLife.OrderByDescending (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Strength")).ToList ();
				valueToCompare = targetsLife [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Strength");
				foreach (GameObject possibleTarget in targetsLife)
				{
					if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Strength"))
						targetsPower.Add (possibleTarget);
				}

				if (targetsPower.Count >= 2)
				{
					targetsPower = targetsPower.OrderByDescending (o => o.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Precision")).ToList ();
					valueToCompare = targetsPower [0].GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Precision");
					foreach (GameObject possibleTarget in targetsPower)
					{
						if (valueToCompare == possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Precision"))
							targetsAim.Add (possibleTarget);
					}
					target = targetsAim [0];
				}
				else
					target = targetsPower [0];
			}
			else
				target = targetsLife [0];
		}
		else
			target = targetsDefense [0];
	}

	public GameObject SelectTarget (List<GameObject> targetList)
	{
		List<GameObject> killableTargetList = new List<GameObject>();
		int nearestTarget;
		int targetDistance;
		int aiDamage;
		int targetEffectiveLife;
		string defenseStatName;
		Character.AEnemyStats aiStats = gameObject.GetComponent<Character.AEnemyStats> ();

		aiDamage = aiStats.GetCharacterStats("Strength");
		if (aiStats.GetWeaponType () == Weapon.E_WeaponType.Magic)
			defenseStatName = "Resistance";
		else
			defenseStatName = "Defense";
		foreach (GameObject possibleTarget in targetList)
		{
			targetEffectiveLife = possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats ("Life") + possibleTarget.GetComponent<Character.ACharacterStats> ().GetCharacterStats (defenseStatName);
			if (targetEffectiveLife - aiDamage <= 0)
				killableTargetList.Add(possibleTarget);
		}

		if (killableTargetList.Count >= 2)
			SelectKillableTarget (killableTargetList, defenseStatName);
		else if (killableTargetList.Count == 1)
			target = killableTargetList[0];
		else
			SelectDamageableTarget (targetList, defenseStatName);
		return (target);
/*			SELECTION DE LA CIBLE LA PLUS PROCHE
 		nearestTarget = -1;
		target = targetList [0];
		foreach (GameObject possibleTarget in targetList)
		{
			targetDistance = GetTargetDistance (possibleTarget);
			if (nearestTarget == -1 || targetDistance < nearestTarget)
			{
				nearestTarget = targetDistance;
				target = possibleTarget;
			}
		}
*/
	}

	private void AttackTarget ()
	{
	}

	private Vector2 GetDirection ()
	{
		Vector2 direction = new Vector2();
		direction.x = gameObject.transform.position.x > target.transform.position.x ? -1 : 1;
		direction.y = gameObject.transform.position.z > target.transform.position.z ? -1 : 1;
		return direction;
	}

	private void GetPathsToTarget (int movementCount, AiPath _parent)
	{
		Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
		Vector2 direction = GetDirection();
		int targetDistance = GetTargetDistance(target);
		Vector2 position = GetCoordById (_parent.caseId);
		Collider[] colliders;

		if (targetDistance > 6)
			targetDistance = 6;
		movementCount++;
		if (position.x != target.transform.position.x) {
			GameObject terrainCase = GameObject.Find ("Case_" + GetIdByCoord (position.x + 10 * direction.x, position.y));
			int id = terrainCase.GetComponent<Case> ().case_id;
			int pathValue = terrainCase.GetComponent<Case> ().getType().movement_cost + _parent.pathValue;
			if((colliders = Physics.OverlapSphere(Get3dCoordById(id), 5f)).Length > 1)
			{
				foreach(Collider collider in colliders)
				{
					GameObject go = collider.gameObject; 
					if (go.tag == "Character" || go.tag == "Enemy") {
						pathValue = 100+ _parent.pathValue;
					}
				}
			}
			AiPath child1 = new AiPath (id, pathValue);
			child1.parent = _parent;
			_parent.Add (0, child1);
			if (movementCount < targetDistance - 1) {
				GetPathsToTarget (movementCount, child1);
			}
		}
		if (position.y != target.transform.position.z) {

			GameObject terrainCase = GameObject.Find ("Case_" + GetIdByCoord (position.x, position.y + 10 * direction.y));
			int id = terrainCase.GetComponent<Case> ().case_id;
			int pathValue = terrainCase.GetComponent<Case> ().getType().movement_cost + _parent.pathValue;
			if((colliders = Physics.OverlapSphere(Get3dCoordById(id), 5f)).Length > 1)
			{
				foreach(Collider collider in colliders)
				{
					GameObject go = collider.gameObject;
					if (go.tag == "Character" || go.tag == "Enemy") {
						pathValue = 100+ _parent.pathValue;
					}
				}
			}
			AiPath child2 = new AiPath (id, pathValue);
			child2.parent = _parent;
			_parent.Add (1, child2);
			if (movementCount < targetDistance - 1) {
				GetPathsToTarget (movementCount, child2);
			} else {
				return;
			}
		}
	}

	void Update()
	{
		if (gameObject.GetComponent<Animator> ().GetBool ("hasDodge")) {
			gameObject.GetComponent<Animator> ().SetBool ("hasDodge", false);
			i++;
		}
		if (gameObject.GetComponent<Animator> ().GetBool ("isHit")) {
			gameObject.GetComponent<Animator> ().SetBool ("isHit", false);
			i++;
		}
		if (gameObject.GetComponent<Animator> ().GetBool ("isAttacking")) {
			gameObject.GetComponent<Animator> ().SetBool ("isAttacking", false);
			i++;
		}
		if (gameObject.GetComponent<Animator> ().GetBool ("isDead")) {
			gameObject.GetComponent<Animator> ().SetBool ("isDead", false);
			i++;
		}
		if (isTargetSelected == true) {
/*				TEST ANIMATIONS
			if (i == 0)
				gameObject.GetComponent<Animator> ().SetBool ("hasDodge", true);
			if (i == 1)
				gameObject.GetComponent<Animator>().SetBool("isHit", true);
			if (i == 2)
				gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
			if (i == 3)
				gameObject.GetComponent<Animator>().SetBool("isDead", true);
			if (i == 4){
				gameObject.GetComponent<Animator>().SetBool("isMoving", true);
				i++;
			}
*/
			AiPath Path = pathsToTarget[target];
			shortestPath = Path.GetMaxMovementPath(Path, gameObject.GetComponent<Character.AEnemyStats>().GetCharacterStats("Movement"));
			if (shortestPath.Count == 0) {
				gameObject.GetComponent<Character.AEnemyStats> ().SetStatus (Character.E_CharacterStatus.HAS_MOVED);
			} else {
				movementTarget = Get3dCoordById (shortestPath [0]);
				is_moving = true;
			}
			isTargetSelected = false;
		}

		if (target_reached == true)
		{
			if (shortestPath.Count == 0)
			{
				is_moving = false;
				target_reached = false;
				gameObject.GetComponent<Animator>().SetBool("isMoving", false);
				gameObject.GetComponent<Character.AEnemyStats>().SetStatus(Character.E_CharacterStatus.HAS_MOVED);
				transform.LookAt (target.transform.position);
			}
			else
			{
				shortestPath.Remove(shortestPath[0]);
				if (shortestPath.Count > 0)
				{
					movementTarget = Get3dCoordById (shortestPath[0]);
				}
			}
		}

		if (is_moving == true)
		{
			float step = 30 * Time.deltaTime;
			transform.position = Vector3.MoveTowards(gameObject.transform.position, movementTarget, step);
			if (transform.position != movementTarget)
				target_reached = false;
			else
				target_reached = true;
			gameObject.GetComponent<Animator>().SetBool("isMoving", true);
			if (shortestPath.Count > 0 && target_reached == false) {
				transform.LookAt (movementTarget);
			}
		}
		if (gameObject.GetComponent<Character.AEnemyStats> ().GetStatus () == Character.E_CharacterStatus.HAS_MOVED) {
			AttackTarget ();
			gameObject.GetComponent<Character.AEnemyStats>().SetStatus(Character.E_CharacterStatus.IS_WAITING);
		}
	}
}
