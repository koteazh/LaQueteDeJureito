using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseType : MonoBehaviour {

	public int cover_value { get; private set; }
	public int movement_cost { get; private set; }

    public enum E_Type { Plain, Forest, Rock, Water, Mountain, Bridge, Wall };
    public E_Type case_type;

    // Use this for initialization
    void Start () {
        switch (case_type)
        {
            case E_Type.Plain:
                cover_value = 0;
                movement_cost = 1;
                break;

            case E_Type.Forest:
                cover_value = 1;
                movement_cost = 2;
                break;

            case E_Type.Rock:
                cover_value = 3;
                movement_cost = 3;
                break;

            case E_Type.Water:
                cover_value = -1;
                movement_cost = 2;
                break;

            case E_Type.Mountain:
                cover_value = 100;
                movement_cost = 100;
                break;

            case E_Type.Bridge:
                cover_value = 0;
                movement_cost = 1;
                break;

            case E_Type.Wall:
                cover_value = 100;
                movement_cost = 100;
                break;

            default:
                cover_value = 0;
                movement_cost = 1;
                break;
        }
        this.gameObject.GetComponent<Case>().setType(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
