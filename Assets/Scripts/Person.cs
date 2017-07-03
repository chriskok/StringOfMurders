using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {

	public string pString; 
	public int minCharAmount;
	public int maxCharAmount;
	public bool isEnemy = false;
	public bool isQuest = false;

	//Variables involving textMesh 
	public TextMesh personText;
	private bool lengthShown = false;
	public List<bool> shownChars;
	public List<char> currentChars;

	//private Person[] people; //For implementing non similar strings and choosing one as murderer
	private Material personMat;
	private float personHeight = 1f;
	private float personWidth = 1f;
	private float personRed;
	private float personBlue;

	const string glyphs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; //add the characters you want

	// Use this for initialization
	void Awake () {
		//Set a random string for each character
		int charAmount = Random.Range(minCharAmount, maxCharAmount); //set those to the minimum and maximum length of your string
		for(int i=0; i<charAmount; i++)
		{
			pString += glyphs[Random.Range(0, glyphs.Length)];
		}

		personMat = transform.gameObject.GetComponent<Renderer> ().material;
		//personMat.color = new Color (Random.Range (0.1f, 0.9f), Random.Range (0.1f, 0.9f), Random.Range (0.1f, 0.9f), 1f); //Random colour generator

		UpdatePerson();

		if (GetComponent<QuestPerson> () != null) {
			isQuest = true;
		}

		if (!isQuest) {
			personText.text = "?";
		}

		shownChars = new List<bool>();
		currentChars = new List<char>();
		for (int i = 0; i < pString.Length; i++) {
			shownChars.Add(false);
			currentChars.Add('?');
			//Debug.Log (currentChars [i]);
		}
	}

	//Based on the current person's string, update the transform
	public void UpdatePerson(){
		
		//Height and width setting based on upper and lower case letters
		string lowerCase = "abcdefghijklmnopqrstuvwxyz";

		int lowerCount = 0;
		int upperCount = 0;

		for (int i = 0; i < pString.Length; i++) {
			for (int j = 0; j < lowerCase.Length; j++) {
				if (pString [i] == lowerCase [j]) {
					lowerCount++;
				}
			}
		}

		upperCount = pString.Length - lowerCount;
		personWidth = upperCount * 0.1f + 1f; 		//personWidth is the original plus 0.1 per upper case letter
		personHeight = pString.Length * 0.1f + 0.5f;	//personHeight is the original plus 0.1 for each letter

		transform.localScale = new Vector3 (personWidth,personHeight,personWidth); // Change height and width


		//Vowels and consonant colour grading 
		int vowelCount = 0;
		int consonantCount = 0;
		char [] charString = pString.ToCharArray ();

		for (int i = 0; i < pString.Length; i++) {
			if (charString [i] == 'a' || charString [i] == 'e' || charString [i] == 'i' || charString [i] == 'o' || charString [i] == 'u' ||
				charString [i] == 'A' || charString [i] == 'E' || charString [i] == 'I' || charString [i] == 'O' || charString [i] == 'U') {
				vowelCount++;
			} else {
				consonantCount++;
			}
		}

		personRed = (float)vowelCount *2 / (pString.Length);
		personBlue = (float)1 - personRed;
		//personBlue = (float)consonantCount / (pString.Length);
		//Debug.Log("Red: " + personRed +"\nBlue: " + personBlue);
		personMat.color = new Color (personRed, 0, personBlue, 1f); //Changes color based on vowel & consonant count

		if (isQuest) {
			transform.GetComponent<QuestPerson> ().SetAnswer(pString);
			//isQuest = false;
		}
	}

	public void UpdateText(){
		if (!isQuest) {
			string tempString = "";

			//Change the characters to be shown
			for (int i = 0; i < pString.Length; i++) {
				if (shownChars [i] == true) {
					currentChars [i] = pString [i];
				}
			}

			if (lengthShown) {
				foreach (char c in currentChars) {
					tempString = tempString + c;
				}
				//personText.text = new string ('?', pString.Length);
			} else {
				foreach (char c in currentChars) {
					if (c != '?') {
						tempString = tempString + c;
					}
				}
			}

			personText.text = tempString;
		}

		//

		//Create a new string to change personText.text to
	}

	/** 
	 * Whole chunk of code of functions that will change Person's characteristics based on user input
	 */

	public string pLowerCase (){
		pString = pString.ToLower();
		UpdateText();
		UpdatePerson ();
		return (transform.name + " is now in lower case");
	}

	public string pUpperCase (){
		pString = pString.ToUpper();
		UpdateText ();
		UpdatePerson ();
		return (transform.name + " is now in upper case");
	}

	public string pCharAt(int index){
		shownChars [index] = true;
		UpdateText ();
		return ("Char at " + transform.name + "[" + index + "] is " + pString [index]);
	}

	public string pConcat(string str){
		pString += str;
		for (int i = 0; i < str.Length; i++){
			shownChars.Add (true);
			currentChars.Add (str [i]);
		}
		UpdateText ();
		UpdatePerson ();
		return ("Adding " + str + " to " + transform.name);
	}

	public string pContains(string str){
		//TODO: UPDATE TEXT?
		return (transform.name + " contains " + str + "?" + " " + pString.Contains(str));
	}

	public void pEquals(GameObject anObject){
		
	}

	//For future use
	/*public void pIndexOf(char ch){

	}

	public void pIndexOf(char ch, int fromIndex){

	}

	public void pIndexOf(string str){

	}

	public void pIndexOf(string str, int fromIndex){

	}

	public void pLastIndexOf(char ch){

	}

	public void pLastIndexOf(char ch, int fromIndex){

	}

	public void pLastIndexOf(string str){

	}

	public void pLastIndexOf(string str, int fromIndex){

	}*/

	public string pLength(){
		lengthShown = true;
		UpdateText ();
		return (transform.name + "'s length is " + pString.Length);
	}

	public string pReplace(char oldChar, char newChar){
		pString = pString.Replace (oldChar, newChar);
		UpdateText ();
		UpdatePerson ();
		return ("Replaced " + oldChar + " with " + newChar + " in " + transform.name);
	}

	public string pSubstring(int startIndex, int endIndex){
		shownChars.RemoveRange (endIndex, pString.Length - endIndex);
		shownChars.RemoveRange (0, startIndex);
		currentChars.RemoveRange (endIndex, pString.Length - endIndex);
		currentChars.RemoveRange (0, startIndex);

		pString = pString.Substring (startIndex, (endIndex-startIndex));
		UpdateText ();
		UpdatePerson ();
		return ("Substring of " + transform.name + " from " + startIndex + " to " + endIndex);
	}
}
