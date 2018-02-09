using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AiPath {

	private AiPath[] children = new AiPath[2];
	public AiPath parent;
	public int caseId { get; private set; }
	public int pathValue { get; private set; }

	public AiPath(int _caseId, int _pathValue = 0)
	{
		caseId = _caseId;
		pathValue = _pathValue;
		children [0] = null;
		children [1] = null;
		parent = null;
	}

	public void Add (int index, AiPath _child)
	{
		children [index] = _child;
	}

	public AiPath GetChild(int index)
	{
		return children [index];
	}

	public AiPath GetFirstNode (AiPath path)
	{
		while (!Object.ReferenceEquals(null, path.parent))
			path = path.parent;
		return path;
	}

	private void GetLastsChildren(List<AiPath> lastNodes, AiPath path)
	{
		if (Object.ReferenceEquals(null, path.GetChild (0)) && Object.ReferenceEquals(null, path.GetChild (1))) {
			lastNodes.Add (path);
			return;
		} else {
			if (!Object.ReferenceEquals(null, path.GetChild (0)))
				GetLastsChildren (lastNodes, path.GetChild (0));
			if (!Object.ReferenceEquals(null, path.GetChild (1)))
				GetLastsChildren (lastNodes, path.GetChild (1));
			return;
		}
	}

	public List<int> GetShortestPath(AiPath path)
	{
		List<AiPath> lastNodes = new List<AiPath>();
		List<int> shortestPathCaseId = new List<int> ();

		path = path.GetFirstNode (path);
		GetLastsChildren (lastNodes, path);
		lastNodes = lastNodes.OrderBy (x => x.pathValue).ToList ();
		AiPath shortestPath = lastNodes [0];
		while (!Object.ReferenceEquals(null, shortestPath.parent)) {
			shortestPathCaseId.Add(shortestPath.caseId);
			shortestPath = shortestPath.parent;
		}
		shortestPathCaseId.Reverse ();
		return shortestPathCaseId;
	}
		
	private void GetMaxMovementNodes(List<AiPath> maxMovementNodes, AiPath path, int movementValue)
	{
		Collider[] colliders;
		GameObject terrainCase = GameObject.Find ("Case_" + path.caseId);
		if ((colliders = Physics.OverlapSphere (terrainCase.transform.position, 1f)).Length > 1) {
			foreach (Collider collider in colliders) {
				GameObject go = collider.gameObject; 
				if (go.tag == "Character" || go.tag == "PauseMenu") {
					path.pathValue += 100;
					break;
				}
			}
		}

		if (path.pathValue == movementValue) {
			maxMovementNodes.Add (path);
			return;
		} else if (path.pathValue > movementValue) {
			return;
		} else {
			if (!Object.ReferenceEquals(null, path.GetChild (0)))
				GetMaxMovementNodes (maxMovementNodes, path.GetChild (0), movementValue);
			if (!Object.ReferenceEquals(null, path.GetChild (1)))
				GetMaxMovementNodes (maxMovementNodes, path.GetChild (1), movementValue);
			return;
		}

	}

	public List<int> GetMaxMovementPath(AiPath path, int movementValue)
	{
		List<AiPath> maxMovementNodes = new List<AiPath>();
		List<int> maxMovementPathCaseId = new List<int> ();
		int? maxCover = null;
		AiPath maxCoverNode = new AiPath(path.caseId, 0);

		while (maxMovementNodes.Count == 0 && movementValue > 0) {
			GetMaxMovementNodes (maxMovementNodes, path, movementValue);
			movementValue--;
		}
	
		foreach (AiPath pathnode in maxMovementNodes) {
			GameObject terrainCase = GameObject.Find ("Case_" + pathnode.caseId);
			if (maxCover == null || maxCover < terrainCase.GetComponent<CaseType> ().cover_value) {
				maxCover = terrainCase.GetComponent<CaseType> ().cover_value;
				maxCoverNode = pathnode;
			}
		}

		while (!Object.ReferenceEquals(null, maxCoverNode.parent)) {
			maxMovementPathCaseId.Add(maxCoverNode.caseId);
			maxCoverNode = maxCoverNode.parent;
		}
		maxMovementPathCaseId.Reverse ();

		return maxMovementPathCaseId;
	}
}
