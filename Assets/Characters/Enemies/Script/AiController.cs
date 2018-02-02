using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

	private GameObject[] mapToTarget;
	private GameObject target;
	private Vector3 movementTarget;
	private List<int> shortestPath;
	private bool is_moving = false;
	private bool target_reached = false;
	int i = 0;

	private int GetIdByCoord(float x, float y)
	{
		return (int) (((x - 5) / 10)	+ (((y - 5) / 10) * 25));
	}

	private Vector2 GetCoordById(int id)
	{
		Vector2 coord = new Vector2 ();
		coord.x = (id - ((id / 25) * 25)) * 10 + 5;
		coord.y = (id / 25) * 10 + 5;
		return coord;
	}

	private Vector3 Get3dCoordById(int id)
	{
		Vector3 coord = new Vector3 ();
		coord.x = (id - ((id / 25) * 25)) * 10 + 5;
		coord.y = 0.5f;
		coord.z = (id / 25) * 10 + 5;
		return coord;
	}
		
	private int GetTargetDistance (GameObject targetToReach)
	{
		int xDiff = (int) Math.Abs(gameObject.transform.position.x - targetToReach.transform.position.x) / 10;
		int yDiff = (int) Math.Abs(gameObject.transform.position.z - targetToReach.transform.position.z) / 10;
		int totalDiff = xDiff + yDiff;
		return (totalDiff);
	}

	private void SelectTarget ()
	{
		GameObject[] targetList;
		int nearestTarget;
		int targetDistance;

		targetList = GameObject.FindGameObjectsWithTag("Character");
		nearestTarget = -1;
		target = targetList [0];
		foreach (GameObject possibleTarget in targetList) {
			targetDistance = GetTargetDistance (possibleTarget);
			if (nearestTarget == -1 || targetDistance < nearestTarget) {
				nearestTarget = targetDistance;
				target = possibleTarget;
			}
		}
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

		movementCount++;
		if (position.x != target.transform.position.x) {
			GameObject terrainCase = GameObject.Find ("Case_" + GetIdByCoord (position.x + 10 * direction.x, position.y));
			int id = terrainCase.GetComponent<Case> ().case_id;
			int pathValue = terrainCase.GetComponent<Case> ().getType().movement_cost + _parent.pathValue;
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
		if (Input.GetMouseButtonDown(0)) {
			if (i == 0)
				gameObject.GetComponent<Animator>().SetBool("hasDodge", true);
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
		/*			SelectTarget ();
			AiPath Path = new AiPath (GetIdByCoord(gameObject.transform.position.x, gameObject.transform.position.z));
			GetPathsToTarget (0, Path);
			shortestPath = Path.GetShortestPath(Path);
			movementTarget = Get3dCoordById (shortestPath[0]);
			is_moving = true;
		}

		if (target_reached == true)
		{
			if (shortestPath.Count == 0)
			{
				is_moving = false;
				target_reached = false;
				gameObject.GetComponent<Animator>().SetBool("isMoving", false);
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
			}*/
		}
	}
}
