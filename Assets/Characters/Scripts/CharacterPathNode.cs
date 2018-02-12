using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Character
{
	public class CharacterPathNode
	{
		public List<CharacterPathNode> children = new List<CharacterPathNode>();
		private E_Direction parentDirection;
		public CharacterPathNode parent;
		public int caseId { get; private set; }
		public int pathValue { get; private set; }

		public CharacterPathNode(int _caseId, int _pathValue = 0)
		{
			caseId = _caseId;
			pathValue = _pathValue;
			parent = null;
		}

		public void Add (CharacterPathNode child)
		{
			children.Add(child);
		}

		public CharacterPathNode GetChild(int index)
		{
			return children.ElementAt(index);
		}

		public E_Direction GetParentDirection ()
		{
			return parentDirection;
		}

		public void SetParentDirection (E_Direction direction)
		{
			parentDirection = direction;
		}

		public CharacterPathNode GetFirstNode (CharacterPathNode path)
		{
			while (!Object.ReferenceEquals(null, path.parent))
				path = path.parent;
			return path;
		}

		public void GetLastsChildren(List<CharacterPathNode> lastNodes, CharacterPathNode path)
		{
			if (path.children.Count == 0) {
				lastNodes.Add (path);
				return;
			} else {
				foreach (CharacterPathNode child in path.children) {
					GetLastsChildren (lastNodes, child);
				}
				return;
			}
		}

		public void GetPathsToTarget(List<CharacterPathNode> targetPathsList, CharacterPathNode path, int target)
		{
			if (path.caseId == target) {
				targetPathsList.Add (path);
				return;
			} else {
				foreach (CharacterPathNode child in path.children) {
					GetPathsToTarget (targetPathsList,child, target);
				}
				return;
			}

		}

		public List<int> GetShortestPathToTarget(int target)
		{
			List<CharacterPathNode> targetPathsList = new List<CharacterPathNode> ();
			List<int> shortestPathCaseId = new List<int> ();

			GetPathsToTarget (targetPathsList, this, target);
			if (targetPathsList.Count > 0) {
				foreach (CharacterPathNode path in targetPathsList)
				targetPathsList = targetPathsList.OrderBy (x => x.pathValue).ToList ();
				CharacterPathNode shortestPath = targetPathsList [0];
				while (!Object.ReferenceEquals(null, shortestPath.parent)) {
					shortestPathCaseId.Add(shortestPath.caseId);
					shortestPath = shortestPath.parent;
				}
				shortestPathCaseId.Reverse ();
				foreach (int dir in shortestPathCaseId)
				return shortestPathCaseId;
			}
			return null;
		}

		public List<int> GetShortestPath(CharacterPathNode path)
		{
			List<CharacterPathNode> lastNodes = new List<CharacterPathNode>();
			List<int> shortestPathCaseId = new List<int> ();

			path = path.GetFirstNode (path);
			GetLastsChildren (lastNodes, path);
			lastNodes = lastNodes.OrderBy (x => x.pathValue).ToList ();
			CharacterPathNode shortestPath = lastNodes [0];
			while (!Object.ReferenceEquals(null, shortestPath.parent)) {
				shortestPathCaseId.Add(shortestPath.caseId);
				shortestPath = shortestPath.parent;
			}
			shortestPathCaseId.Reverse ();
			return shortestPathCaseId;
		}
	}
}

