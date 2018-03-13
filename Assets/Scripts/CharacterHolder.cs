using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour {
	public GameObject[] characters;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}

	public void HideCharacters()
	{
		foreach (GameObject character in characters)
			character.SetActive (false);
	}
}
