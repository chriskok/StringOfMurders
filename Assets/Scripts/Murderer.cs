using UnityEngine;
using System.Collections;

public class Murderer : MonoBehaviour {

	public static string murderString;
	public static string shownString;
	public TextMesh murderText;

	void Start (){
		murderText.text = "?";
		shownString = "?";
	}

	void Update(){
		murderText.text = shownString;
	}
}
