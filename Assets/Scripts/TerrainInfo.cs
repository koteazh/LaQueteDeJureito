using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainInfo : MonoBehaviour {

	public void UpdateTerrainInfo (Case terrainCase)
	{
		this.gameObject.transform.Find ("TerrainTypeLabel").GetComponent<Text>().text = terrainCase.getType().ToString();
		this.gameObject.transform.Find ("CoverRow/CoverValue").GetComponent<Text>().text = terrainCase.getType().cover_value.ToString();
		this.gameObject.transform.Find ("CostRow/CostValue").GetComponent<Text>().text = terrainCase.getType().movement_cost.ToString();
	}

}
