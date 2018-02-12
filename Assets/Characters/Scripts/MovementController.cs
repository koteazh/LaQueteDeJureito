using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementController : MonoBehaviour {

    List<int> path;
    bool is_moving = false;
    bool target_reached = false;
    Vector3 target;
    float speed = 10;
	int casePerRow;

    // Use this for initialization
    void Start () {
        path = new List<int>();
        target = transform.position;
		casePerRow = GameObject.FindGameObjectWithTag ("Terrain").GetComponent<TerrainRowLength> ().GetTerrainTilesPerRow();
    }

	public void MoveToTarget(List<int> pathToTarget)
	{
		path = pathToTarget;
		if (path.Count > 0) {
			target = Get3dCoordById (path[0]);
		}
		is_moving = true;
	}

    // Update is called once per frame
    void Update() {
        if (target_reached == true)
        {
            if (path.Count == 0)
            {
                is_moving = false;
                target_reached = false;
                gameObject.GetComponent<Animator>().SetBool("isMoving", false);
				gameObject.GetComponent<Character.ACharacterStats> ().SetStatus (Character.E_CharacterStatus.HAS_MOVED);
            }
            else
            {
                path.Remove(path[0]);
                if (path.Count > 0)
                {
					target = Get3dCoordById(path[0]);
                }
            }
        }

        if (is_moving == true)
        {
            float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (transform.position != target)
                target_reached = false;
            else
                target_reached = true;
            gameObject.GetComponent<Animator>().SetBool("isMoving", true);
			if (path.Count > 0 && target_reached == false) {
				transform.LookAt (target);
			}
		}
    }


	private Vector3 Get3dCoordById(int id)
	{
		Vector3 coord = new Vector3 ();
		coord.x = (id - ((id / casePerRow) * casePerRow)) * 10 + 5;
		coord.y = 0.5f;
		coord.z = (id / casePerRow) * 10 + 5;
		return coord;
	}
}
