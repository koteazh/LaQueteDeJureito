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
}
