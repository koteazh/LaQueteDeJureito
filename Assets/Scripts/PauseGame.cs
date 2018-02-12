using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Escape")) {
			if (!pauseMenu.activeInHierarchy) {
				pauseMenu.SetActive (true);
				Time.timeScale = 0;
			} else {
				pauseMenu.SetActive (false);
				Time.timeScale = 1;
			}

		}
	}
}
