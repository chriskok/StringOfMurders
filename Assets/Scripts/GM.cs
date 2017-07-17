using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	[SerializeField]
	public static GameObject tempPerson;
	public static int currentScene = 0;

	private bool isEditMode = false;
	private bool enemyChosen = false;
	private TutManager TM;

	public GameObject player;
	public Camera cam;
	public Text mainText;
	public Text inputLog;
	public InputField mainInput;
	public GameObject cheatSheet;
	public GameObject[] people;

	//Variables for function use
	public static int charAtUsed = 0;
	public static int containsUsed = 0;
	public static int concatUsed = 0;
	public static int toLowerCaseUsed = 0;
	public static int toUpperCaseUsed = 0;
	public static int lengthUsed = 0;
	public static int replaceUsed = 0;
	public static int substringUsed = 0;

	// Use this for initialization
	void Start () {
		mainInput.gameObject.SetActive(false);
		mainText.text = "";
		inputLog.text = "";
		Cursor.lockState = CursorLockMode.Confined;
		people = GameObject.FindGameObjectsWithTag ("Person");

		int enemyIndex;
		do {
			enemyIndex = UnityEngine.Random.Range (0, people.Length - 1);
			Person personScript = people [enemyIndex].GetComponent<Person> ();

			if (!personScript.isQuest) {
				personScript.isEnemy = true;
				enemyChosen = true;
				Murderer.murderString = personScript.pString;
			}
		} while(enemyChosen == false);
		Debug.Log("Hint: The killer is " + people[enemyIndex].name);

		currentScene = SceneManager.GetActiveScene ().buildIndex;
		Debug.Log ("Current scene: " + currentScene);
		if (currentScene == 1) {
			TM = GameObject.FindGameObjectWithTag ("TM").GetComponent<TutManager>(); 
			TM.enemyIndex = enemyIndex;
			TM.runTutorial ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//PLAYER SELECT
		if (Input.GetMouseButtonDown(0) && !isEditMode)
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo);

			if (hit && hitInfo.transform.gameObject.tag == "Person") 
			{
				mainInput.gameObject.SetActive(true);
				player.GetComponent<PlayerMovement> ().enabled = false;
				tempPerson = hitInfo.transform.gameObject;

				//Check if this person is a quest holder
				if (tempPerson.GetComponent<QuestPerson> () != null) {
					tempPerson.GetComponent<QuestPerson> ().GetQuest ();
					tempPerson.GetComponent<Person> ().isQuest = true;
				}

				mainText.text = (tempPerson.name + ".");
				isEditMode = true;
			} 
		} 
	}
		
	public void SwitchStr (string inputF){
		
		//Cancel statement = return;

		try {
			//Run fail c	ases, mu	st contain all the below and semicolon in the right order

			int startIndex = inputF.IndexOf ("(");
			int endIndex = inputF.IndexOf (")");
			int middleIndex = 0;
			if (inputF.Contains(",")){
				 middleIndex = inputF.IndexOf(",");
			}
			if (!inputF.Contains(";") && inputF.Length > 0){
				UpdateLog("You're missing a semi-colon");
				return;
			}

			string inputSub = inputF.Substring (0, startIndex);
			string arguments = "";
			string argument1 = "";
			string argument2 = "";

			if (startIndex + 1 != endIndex){
				arguments = inputF.Substring(startIndex + 1, (endIndex - (startIndex + 1))).Trim(); //Note: Substring(startIndex, length);
				if (arguments.Contains(",")){
					argument1 = inputF.Substring(startIndex + 1, (middleIndex - (startIndex + 1)));
					argument2 = inputF.Substring(middleIndex + 1, (endIndex - (middleIndex + 1)));
				}
			}

			switch (inputSub) {
			case "toLowerCase":
				UpdateLog(tempPerson.GetComponent<Person>().pLowerCase());  
				toLowerCaseUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "toUpperCase":
				UpdateLog(tempPerson.GetComponent<Person>().pUpperCase()); 
				toUpperCaseUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "charAt":
				UpdateLog(tempPerson.GetComponent<Person>().pCharAt(int.Parse(arguments))); 
				charAtUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "contains":
				if (arguments.Contains("\"")){
					arguments = arguments.Replace("\"","");
				} else if (arguments.Contains("'")){
					arguments = arguments.Replace("'", "");
				} else {
					Debug.Log("Arguments for contains() has to be a \"string\" or 'char'!");
					UpdateLog("Arguments for contains() has to \nbe a \"string\" or 'char'!");
					break;
				}
				UpdateLog(tempPerson.GetComponent<Person>().pContains(arguments)); 
				containsUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "equals":
				if (arguments.Contains("urderer")){
					if (tempPerson.GetComponent<Person>().isEnemy == true){
						Debug.Log("Game over, you win!"); 
						UpdateLog("Game over, you win!"); 
						switch (currentScene){
						case 1: 
							resetFunctionUsage();
							SceneManager.LoadScene(2);
							currentScene = 2; 
							break;
						case 2: 
							resetFunctionUsage();
							SceneManager.LoadScene(0);
							currentScene = 0;
							break;
						}
					} else{
						Debug.Log("Sad lyfe, you've got the wrong guy!");
						UpdateLog("Sad lyfe, you've got the wrong guy!");
					}
				} else {
					Debug.Log("Invalid argument (in the future you can check if people are twins, not yet thoo!)");
					UpdateLog("Invalid argument; only equals(Murderer);");
				}
				break;

			case "length":
				UpdateLog(tempPerson.GetComponent<Person>().pLength());
				lengthUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "concat":
				if (arguments.Contains("\"")){
					arguments = arguments.Replace("\"","");
				} else {
					Debug.Log("Arguments for concat() has to be a \"string\"!");
					UpdateLog("Arguments for concat() has to be a \"string\"!");
					break;
				}
				UpdateLog(tempPerson.GetComponent<Person>().pConcat(arguments));
				concatUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "replace":
				if (argument1.Contains("'")){
					argument1 = argument1.Replace("'", "");
				} else {
					Debug.Log("Arguments for replace() has to be a 'char'!");
					UpdateLog("Arguments for replace() has to be a 'char'!");
					break;
				}
				if (argument2.Contains("'")){
					argument2 = argument2.Replace("'", "");
				} else {
					Debug.Log("Arguments for replace() has to be a 'char'!");
					UpdateLog("Arguments for replace() has to be a 'char'!");
					break;
				}
				argument1 = argument1.Trim();
				argument2 = argument2.Trim();
				char tempChar1 = char.Parse(argument1);
				char tempChar2 = char.Parse(argument2);
				UpdateLog(tempPerson.GetComponent<Person>().pReplace(tempChar1,tempChar2));
				replaceUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "substring":
				UpdateLog(tempPerson.GetComponent<Person>().pSubstring(int.Parse(argument1),int.Parse(argument2))); 
				substringUsed++;
				if (currentScene == 1) TM.stageChanger();
				break;

			case "cancel":
				break;

			default:
				UpdateLog ("WRONG INPUT BRUV!");
				break;
			}

			if (tempPerson.GetComponent<QuestPerson> () != null) {
				tempPerson.GetComponent<QuestPerson>().OutOfEdit();
			}

			player.GetComponent<PlayerMovement> ().enabled = true;
			mainInput.text = "";
			mainInput.gameObject.SetActive(false);
			mainText.text = "";
			isEditMode = false;

		} catch (Exception e){
			UpdateLog("Invalid input, try again!");
			Debug.Log ("Invalid input, try again!");
			Debug.Log (e);
		}
	}

	public void UpdateLog (string inputStr){
		string oldText = inputLog.text;
		inputLog.text = inputStr + "\n" + oldText;
	}

	public void CheatSheetButton(){
		if (!cheatSheet.activeInHierarchy) {
			cheatSheet.SetActive (true);
		} else {
			cheatSheet.SetActive (false);
		}
	}

	public static void resetFunctionUsage(){
		Debug.Log ("Functions reset!");
		charAtUsed = 0;
		containsUsed = 0;
		concatUsed = 0;
		toLowerCaseUsed = 0;
		toUpperCaseUsed = 0;
		lengthUsed = 0;
		replaceUsed = 0;
		substringUsed = 0;
	}

	public static void printFunctionUsage(){
		Debug.Log ("charAt: " + charAtUsed + "\ncontains: " + containsUsed + "\nconcat: " + concatUsed + "\ntoLower: " + toLowerCaseUsed + "\ntoUpper: " + 
			toUpperCaseUsed + "\nlength: " + lengthUsed + "\nreplace: " + replaceUsed + "\nsubstring: " + substringUsed);
	}
}
