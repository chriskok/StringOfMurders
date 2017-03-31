using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {
	public GameObject[] mainUI;
	public GameObject[] mainPeople;

	public GameObject[] instructions;
	public GameObject[] instructPeople;

	public void StartButton(){
		SceneManager.LoadScene (1);
	}

	public void InstructionsButton(){
		ResetScreen ();

		instructions [0].SetActive (true);
	}

	public void InstructNext1(){
		ResetScreen ();

		instructions [1].SetActive (true);
		instructPeople [0].SetActive (true);
	}

	public void InstructNext2(){
		ResetScreen ();

		instructions [2].SetActive (true);
		instructPeople [1].SetActive (true);
	}

	public void StartScreen(){
		ResetScreen ();

		foreach (GameObject button in mainUI) {
			if (!button.activeInHierarchy) {
				button.SetActive (true);
			}
		}

		foreach (GameObject person in mainPeople) {
			if (!person.activeInHierarchy) {
				person.SetActive (true);
			}
		}
	}

	public void ResetScreen(){
		foreach (GameObject button in mainUI) {
			if (button.activeInHierarchy) {
				button.SetActive (false);
			}
		}

		foreach (GameObject person in mainPeople) {
			if (person.activeInHierarchy) {
				person.SetActive (false);
			}
		}

		foreach (GameObject person in instructPeople) {
			if (person.activeInHierarchy) {
				person.SetActive (false);
			}
		}

		foreach (GameObject instruction in instructions) {
			if (instruction.activeInHierarchy) {
				instruction.SetActive (false);
			}
		}
	}
}
