using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementController : MonoBehaviour {

    List<E_Direction> path;
    bool is_moving = false;
    bool target_reached = false;
    Vector3 target;
    float speed = 10;

    // Use this for initialization
    void Start () {
        path = new List<E_Direction>();
        target = transform.position;
    }

    // Update is called once per frame
    void Update() {
		if (!GameObject.FindGameObjectWithTag ("PauseMenu")) {
			if (Input.GetButtonDown ("Up")) {
				if (path.Count != 0 && path[path.Count-1] == E_Direction.DOWN)
					path.Remove (path[path.Count-1]);
				else
					path.Add (E_Direction.UP);
			}
			if (Input.GetButtonDown ("Down")) {
				if (path.Count != 0 && path[path.Count-1] == E_Direction.UP)
					path.Remove (path[path.Count-1]);
				else
					path.Add (E_Direction.DOWN);
			}
			if (Input.GetButtonDown ("Left")) {
				if (path.Count != 0 && path[path.Count-1] == E_Direction.RIGHT)
					path.Remove (path[path.Count-1]);
				else
					path.Add (E_Direction.LEFT);
			}
			if (Input.GetButtonDown ("Right")) {
				if (path.Count != 0 && path[path.Count-1] == E_Direction.LEFT)
					path.Remove (path[path.Count-1]);
				else
					path.Add (E_Direction.RIGHT);
			}
			if (Input.GetMouseButtonDown(1)) {
				if (path.Count > 0) {
					target = GetTarget (path[0]);
				}
				is_moving = true;
			}
		}

        if (target_reached == true)
        {
            if (path.Count == 0)
            {
                is_moving = false;
                target_reached = false;
                gameObject.GetComponent<Animator>().SetBool("isMoving", false);
            }
            else
            {
                path.Remove(path[0]);
                if (path.Count > 0)
                {
                    target = GetTarget(path[0]);
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

    Vector3 GetTarget(E_Direction Direction)
    {
        switch (Direction)
        {
           case E_Direction.UP :
                return (transform.position + new Vector3(0, 0, 10));
            case E_Direction.DOWN:
                return (transform.position + new Vector3(0, 0, -10));
            case E_Direction.LEFT:
                return (transform.position + new Vector3(-10, 0, 0));
            case E_Direction.RIGHT:
                return (transform.position + new Vector3(10, 0, 0));
        }
        return (transform.position);
    }
}
