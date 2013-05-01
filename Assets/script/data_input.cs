using UnityEngine;
using System.Collections;

public class data_input : MonoBehaviour {

	//~ public string next_level;
	//~ public Transform button_lvl_1;
	//~ public Transform button_lvl_2;
	//~ public Transform button_lvl_3;
	//~ public Transform button_lvl_4;
	
	// Use this for initialization
	void Start () {
		//~ button_lvl_1.guiText.text="Level 1";
		//~ button_lvl_2.guiText.text="Level 2";
		//~ button_lvl_3.guiText.text="Level 3";
		//~ button_lvl_4.guiText.text="Level 4";
	}
	
	// Update is called once per frame
	void Update () {

		//~ if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began){
			//~ iPhoneTouch touch = iPhoneInput.GetTouch(0);

			//~ if (button_lvl_1.guiText.HitTest( touch.position )){
				//~ button_lvl_1.guiText.material.color=Color.red;
				//~ StartCoroutine(Load_Next_Level("level_1"));
			//~ }
			//~ else if (button_lvl_2.guiText.HitTest( touch.position )){
				//~ button_lvl_2.guiText.material.color=Color.red;
				//~ StartCoroutine(Load_Next_Level("level_2"));
			//~ }
			//~ else if (button_lvl_3.guiText.HitTest( touch.position )){
				//~ button_lvl_3.guiText.material.color=Color.red;
				//~ StartCoroutine(Load_Next_Level("level_3"));
			//~ }
			//~ else if (button_lvl_4.guiText.HitTest( touch.position )){
				//~ button_lvl_4.guiText.material.color=Color.red;
				//~ StartCoroutine(Load_Next_Level("level_4"));
			//~ }
		//~ }	 
		
		//~ if(iPhoneInput.touchCount==1){
			//~ iPhoneTouch touch = iPhoneInput.GetTouch(0);
			//~ if (button_lvl_1.guiText.HitTest( touch.position )){
				//~ button_lvl_1.guiText.material.color=Color.green;
			//~ }
			//~ else if (button_lvl_2.guiText.HitTest( touch.position )){
				//~ button_lvl_2.guiText.material.color=Color.green;
			//~ }
			//~ else if (button_lvl_3.guiText.HitTest( touch.position )){
				//~ button_lvl_3.guiText.material.color=Color.green;
			//~ }
			//~ else if (button_lvl_4.guiText.HitTest( touch.position )){
				//~ button_lvl_4.guiText.material.color=Color.green;
			//~ }	
			//~ else{
				//~ button_lvl_1.guiText.material.color=Color.white;
				//~ button_lvl_2.guiText.material.color=Color.white;
				//~ button_lvl_3.guiText.material.color=Color.white;
				//~ button_lvl_4.guiText.material.color=Color.white;
			//~ }
			
		//~ }
		//~ else{
			//~ button_lvl_1.guiText.material.color=Color.white;
			//~ button_lvl_2.guiText.material.color=Color.white;
			//~ button_lvl_3.guiText.material.color=Color.white;
			//~ button_lvl_4.guiText.material.color=Color.white;
		//~ }
		
		//~ if(iPhoneInput.touchCount==0){
			//~ GameObject cursor = GameObject.Find("blursor(Clone)");
			//~ if(cursor!=null) Destroy(cursor);
		//~ }

	}
	
	IEnumerator Load_Next_Level(string next_level){
		yield return new WaitForSeconds (0.1f);
		Application.LoadLevel(next_level);
	}
		
	public string stringToEdit = "Hello World";
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 20),  "Name : ");
		stringToEdit = GUI.TextField(new Rect(60, 10, 200, 20), stringToEdit, 25);
		
		GUI.Label(new Rect(10, 35, 100, 20),  "Status	: ");
		stringToEdit = GUI.TextField(new Rect(60, 35, 200, 20), stringToEdit, 25);
	}
	
}
