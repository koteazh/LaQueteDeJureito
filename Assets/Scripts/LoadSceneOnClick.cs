using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	private void CleanScene()
	{
		GameObject[] characters = GameObject.Find ("CharacterHolder").GetComponent<CharacterHolder>().characters;
		foreach (GameObject character in characters)
			character.GetComponent<Character.AStats>().ResetHealth();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
			Destroy (enemy);
	}

	public void LoadByIndex (int sceneIndex)
	{
		CleanScene ();
		if (SceneManager.GetActiveScene ().buildIndex != 0)
			GameObject.Find ("CharacterHolder").GetComponent<CharacterHolder> ().HideCharacters ();
		SceneManager.LoadScene (sceneIndex);
		Time.timeScale = 1;
	}
}
