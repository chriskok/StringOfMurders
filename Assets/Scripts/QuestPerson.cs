using UnityEngine;
using System.Collections;

public class QuestPerson : MonoBehaviour {

	public static int infoIndex = 0;
	private string answer;
	private Person personScript;

	public int answerIndex;
	public string prompt;
	public string murderInfo;
	public TextMesh questText;
	public TextMesh questPrompt;

	void Start(){
		infoIndex = 0;
		questText.text = "!";
		questText.fontSize = 45;

		personScript = transform.GetComponent<Person> ();
		string pStringTemp = personScript.pString;

		//FOR FUTURE: randomize the answers. Eg: not just last 3 or 'A'!
		switch (answerIndex) {
		case 1:
			prompt = "I'm too tall, \ncan you kick out \nmy last 3 characters?";
			answer = pStringTemp.Substring (0, pStringTemp.Length - 2);
			break;
		case 2: 
			prompt = "I don't like all my \n" + pStringTemp [2] + "'s, can you replace em\n with all with 'A'?";
			answer = pStringTemp.Replace (pStringTemp [2], 'A');
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

			//FOR FUTURE: Randomize most of the info. Like the character number
			switch (infoIndex) {
			case 1:
				murderInfo = "The murderer's\n length is " + Murderer.murderString.Length;
				Murderer.shownString = new string ('?', Murderer.murderString.Length);
				StartCoroutine(Answered());
				break;
			case 2: 
				murderInfo = "The murderer's third\n character is " + Murderer.murderString [2];
				Murderer.shownString = "??" + Murderer.murderString [2] + new string('?', Murderer.murderString.Length - 3);
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
			}
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
