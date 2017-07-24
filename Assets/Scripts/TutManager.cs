using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutManager : MonoBehaviour {

	public GM GMScript;
	public int enemyIndex; 
	public List<int> sortedIndex;
	public int stage = 0;
	public GameObject player;
	public TextMesh playerText;
	private PlayerMovement playerMovementScript;

	//To show instructions
	[TextArea(3, 10)]
	public string[] sentences;
	private Queue<string> sentenceQueue;
	public Text tutText; 
	public GameObject[] tutUI;
	public int[] textIndex; 
	public float typingSpeed;

	public void runTutorial(){
		sentenceQueue = new Queue<string>();
		sortedIndex = new List<int> ();
		playerMovementScript = player.GetComponent<PlayerMovement> ();

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

		foreach (GameObject obj in tutUI) {
			obj.SetActive (false);
		}

		StartInstructions (textIndex[0], textIndex[1]-1);
	}

	public void stageOne(){
		stage = 1;

		//Teleport the people with revealed names into the players range of sight
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

		//Teleport the rest of the people behind players line of sight
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
			StartInstructions (textIndex[1], textIndex[2]-1);
		} 
		if (stage == 2 && GM.lengthUsed > 0 && GM.charAtUsed > 0 && GM.containsUsed > 0) {
			StartInstructions (textIndex[2], textIndex[3]); 
		}
	}

	public void StartInstructions (int startIndex, int endIndex)
	{
		//Turn on all necessary UI for the tutorial instructions 
		foreach (GameObject obj in tutUI) {
			obj.SetActive (true);
		}
			
		//Turn player movement off
		playerMovementScript.enabled = false;

		//Clear the sentence queue
		sentenceQueue.Clear();

		for (int i = startIndex; i < endIndex + 1; i++)
		{
			sentenceQueue.Enqueue(sentences[i]);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{

		if (sentenceQueue.Count == 0)
		{
			EndInstructions();
			return;
		}

		string sentence = sentenceQueue.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		tutText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			tutText.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
	}

	void EndInstructions()
	{
		playerMovementScript.enabled = true;

		foreach (GameObject obj in tutUI) {
			obj.SetActive (false);
		}

		switch (stage) {
		case 0: 
			stageOne ();
			break;
		case 1: 
			stageTwo ();
			break;
		case 2:
			SceneManager.LoadScene (2);
			break;
		}
	}
}
