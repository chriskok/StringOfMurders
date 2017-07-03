using UnityEngine;
using System.Collections;

public class QuestPerson : MonoBehaviour {

	public static int infoIndex = 0;
	private string answer;
	private Person personScript;
	public static bool[] revealed; 
	private string pStringTemp;

	public int answerIndex;
	public string prompt;
	public string murderInfo;
	public TextMesh questText;
	public TextMesh questPrompt;

	void Start(){
		infoIndex = 0;
		questText.text = "!";
		questText.fontSize = 30;

		//Get the particular QuestPerson's associated string
		personScript = transform.GetComponent<Person> ();
		pStringTemp = personScript.pString;

		//Create array of booleans for each character to be revealed
		revealed = new bool[pStringTemp.Length];

		switch (answerIndex) {
		case 1:
			int newLength = Random.Range (2, 5);
			prompt = string.Format("I'm too tall, \ncan you kick out \nmy last {0} characters?", newLength);
			answer = pStringTemp.Substring (0, pStringTemp.Length - newLength + 1);
			break;
		case 2: 
			int index = Random.Range (0, pStringTemp.Length - 1); //Pick random character from person's string to change
			const string glyphs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			char newChar = glyphs[Random.Range(0, glyphs.Length)]; //Pick random character to char to
			prompt = string.Format("I don't like all my \n {0}'s, can you replace em" +
				"\n all with '{1}'?", pStringTemp [index], newChar);
			answer = pStringTemp.Replace (pStringTemp [index], newChar);
			break;
		case 3: 
			prompt = "I wanna be fatter!!!";
			answer = pStringTemp.ToUpper ();
			break;
		case 4: 
			prompt = "I wanna be thinner!!!";
			answer = pStringTemp.ToLower ();
			break;
		default:
			Debug.Log ("Something went wrong");
			break;
		}
	}

	public void GetQuest () {
		questPrompt.text = prompt;
	}

	public void SetAnswer(string pString){
		if (pString.Equals (answer)) {
			++infoIndex;
			int index = 0;

			if (infoIndex == 1) {
				murderInfo = "The murderer's\n length is " + Murderer.murderString.Length;
				Murderer.shownString = new string ('?', Murderer.murderString.Length);
				StartCoroutine(Answered());
			} else {
				//Pick random character from person's string to change
				do {
					index = Random.Range (0, pStringTemp.Length - 1);
					//Debug.Log ("Trying - " + index); 
				} while (revealed [index] == true);
				//Debug.Log ("You should reveal " + index);
				revealed [index] = true;

				murderInfo = string.Format ("The murderer's character\n at index {0} is {1}", index, Murderer.murderString [index]);
				Murderer.shownString = Murderer.shownString.Remove (index, 1);
				Murderer.shownString = Murderer.shownString.Insert (index, char.ToString (Murderer.murderString [index]));
				StartCoroutine (Answered ());
			}

			//Previous implementation
			/*switch (infoIndex) {
			case 1:
				murderInfo = "The murderer's\n length is " + Murderer.murderString.Length;
				Murderer.shownString = new string ('?', Murderer.murderString.Length);
				StartCoroutine(Answered());
				break;
			case 2: 
				murderInfo = string.Format ("The murderer's character\n at index {0} is {1}", index, Murderer.murderString [index]);
				Murderer.shownString = Murderer.shownString.Remove (index, 1);
				Murderer.shownString = Murderer.shownString.Insert (index, char.ToString (Murderer.murderString [index]));
				StartCoroutine(Answered());
				break;
			case 3: 
				murderInfo = "The murderer's last\n character is " + Murderer.murderString [Murderer.murderString.Length - 1];
				Murderer.shownString = "??" + Murderer.murderString [2] + new string ('?', Murderer.murderString.Length - 2) + Murderer.murderString [Murderer.murderString.Length - 1];
				StartCoroutine(Answered());
				break;
			case 4: 
				murderInfo = "The murderer's first\n character is " + Murderer.murderString [0];
				Murderer.shownString = Murderer.murderString[0] + "?" + Murderer.murderString [2] + new string ('?', Murderer.murderString.Length - 2) + Murderer.murderString [Murderer.murderString.Length - 1];
				StartCoroutine(Answered());
				break;
			default:
				Debug.Log ("Something went wrong");
				break;
			}*/
		} else {
			OutOfEdit ();
		}
	}

	public void OutOfEdit(){
		if (murderInfo.Equals ("")) {
			transform.GetComponent<Person> ().isQuest = false;
			questText.text = "!";
			questPrompt.text = "";
		}
	}

	IEnumerator Answered(){
		questPrompt.text = murderInfo;
		yield return new WaitForSeconds (3);
		questText.text = "";
		questPrompt.text = "";
		transform.GetComponent<QuestPerson> ().enabled = false;
	}


}
