using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TutManager : MonoBehaviour {

	public GM GMScript;
	public int enemyIndex; 
	public List<int> sortedIndex;
	public int stage = 0;
	public TextMesh playerText;

	public void runTutorial(){
		sortedIndex = new List<int> ();

		//Run through all the people, change their shown chars and update their text
		//Change some specifically to show all characters
		int count = 0;
		for (int i = 0; i < GMScript.people.Length; i++) {
			Person currentScript = GMScript.people [i].GetComponent<Person> ();	
			//Skip this person if QuestPerson or Enemy
			if (currentScript.isQuest || i == enemyIndex) {
				continue;
			}

			//Choose 4 people to reveal strings
			if (count < 4) {
				for (int j = 0; j < currentScript.pString.Length; j++) {
					currentScript.shownChars [j] = true;
				}
				sortedIndex.Add (i);
				currentScript.UpdateText ();
				count++;
			} else {
				//Adding all the people who are just normal '?'
				sortedIndex.Add (i);
			}
		}

		stageOne ();
	}

	public void stageOne(){
		stage = 1;
		//TODO: tell user what's up for a couple of seconds and then change

		int xVal = -8;
		for (int i = 0; i < 4; i++) {
			GameObject tempPerson = GMScript.people [sortedIndex[i]];	
			if (xVal == 0) {
				xVal = 4;
			}
			tempPerson.transform.position = new Vector3 (xVal, 3, 7); 
			xVal += 4;
		}
	}

	public void stageTwo(){
		stage = 2;
		//TODO: tell user what's up 

		int xVal = -4;
		for (int i = 4; i < 7; i++) {
			GameObject tempPerson = GMScript.people [sortedIndex[i]];	
			tempPerson.transform.position = new Vector3 (xVal, 3, -5); 
			Quaternion rot = Quaternion.Euler (0, 180, 0);
			tempPerson.transform.rotation = rot;
			xVal += 4;
		}
	}

	public void stageChanger(){
		if (stage == 1 && GM.toLowerCaseUsed > 0 && GM.toUpperCaseUsed > 0 && GM.substringUsed > 0 && GM.replaceUsed > 0) {
			Debug.Log ("Stage two begun");
			stageTwo ();
		} 
		if (stage == 2 && GM.lengthUsed > 0 && GM.charAtUsed > 0 && GM.containsUsed > 0) {
			//TODO: add final tutorial words for a couple of seconds 
			//Finish tutorial and load the scene 
			SceneManager.LoadScene (2);
		}
	}

	public void instructions(int length, bool infinite){

	}
}
