using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour {
    private CaseType type;
    public int case_id;

    // Use this for initialization
    void Start () {
		
	}

    public CaseType getType()
    {
        return (type);
    }

    public void setType(CaseType _case_type)
    {
        type = _case_type;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
