using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRowLength : MonoBehaviour {
	[SerializeField] private int terrainTilesPerRow;

	public int GetTerrainTilesPerRow ()
	{
		return terrainTilesPerRow;
	}
}
