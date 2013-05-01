using UnityEngine;
using System.Collections;

public class language_select : MonoBehaviour {

	public string next_scene;
	public Transform button_lvl_1;
	public Transform button_lvl_2;
	public Transform button_lvl_3;
	public Transform button_lvl_4;
	
	public GameObject language_indicator;
	
	// Use this for initialization
	void Start () {
		button_lvl_1.guiText.text="English";
		button_lvl_2.guiText.text="»’±æ’Z";
		//~ button_lvl_3.guiText.text="Environmental";
		//~ button_lvl_4.guiText.text="Level 4";
		
		button_lvl_1.gameObject.AddComponent<button>();
		button_lvl_2.gameObject.AddComponent<button>();
		//~ button_lvl_3.gameObject.AddComponent<button>();
		//~ button_lvl_4.gameObject.AddComponent<button>();
		
		Color temp_colour=Color.white;
		button_lvl_1.gameObject.SendMessage("SetColour", temp_colour);
		button_lvl_2.gameObject.SendMessage("SetColour", temp_colour);
		//~ button_lvl_3.gameObject.SendMessage("SetColour", temp_colour);
		//~ button_lvl_4.gameObject.SendMessage("SetColour", temp_colour);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began){
			iPhoneTouch touch = iPhoneInput.GetTouch(0);
			Test_Button_Pressed(touch.position);
		}	 
		
		if(Input.GetMouseButtonDown(0)){
			Vector2 pos=Input.mousePosition;
			Test_Button_Pressed(pos);
		}
		
		if(iPhoneInput.touchCount==1){
			iPhoneTouch touch = iPhoneInput.GetTouch(0);
			if (button_lvl_1.guiText.HitTest( touch.position )){
				button_lvl_1.guiText.material.color=Color.green;
			}
			else if (button_lvl_2.guiText.HitTest( touch.position )){
				button_lvl_2.guiText.material.color=Color.green;
			}
			//~ else if (button_lvl_3.guiText.HitTest( touch.position )){
				//~ button_lvl_3.guiText.material.color=Color.green;
			//~ }
			//~ else if (button_lvl_4.guiText.HitTest( touch.position )){
				//~ button_lvl_4.guiText.material.color=Color.green;
			//~ }	
			else{
				button_lvl_1.guiText.material.color=Color.white;
				button_lvl_2.guiText.material.color=Color.white;
				//~ button_lvl_3.guiText.material.color=Color.white;
				//~ button_lvl_4.guiText.material.color=Color.white;
			}
			
		}
		//~ else{
			//~ button_lvl_1.guiText.material.color=Color.white;
			//~ button_lvl_2.guiText.material.color=Color.white;
			//~ button_lvl_3.guiText.material.color=Color.white;
			//~ //button_lvl_4.guiText.material.color=Color.white;
		//~ }

	}
	
	void Test_Button_Pressed(Vector2 pos){

		if (button_lvl_1.guiText.HitTest( pos )){
			button_lvl_1.guiText.material.color=Color.red;
			language_indicator.SendMessage("Set_Language", "english");
			StartCoroutine(Load_Next_Level(next_scene));
			
		}
		else if (button_lvl_2.guiText.HitTest( pos )){
			button_lvl_2.guiText.material.color=Color.red;
			language_indicator.SendMessage("Set_Language", "japanese");
			StartCoroutine(Load_Next_Level(next_scene));
		}
		//~ else if (button_lvl_3.guiText.HitTest( pos )){
			//~ button_lvl_3.guiText.material.color=Color.red;
			//~ level = 1;
			//~ StartCoroutine(Load_Next_Level("level_3."+level.ToString()));
		//~ }
		//~ else if (button_lvl_4.guiText.HitTest( pos )){
			//~ button_lvl_4.guiText.material.color=Color.red;
			//~ StartCoroutine(Load_Next_Level("level_4"));
		//~ }
	}
	
	IEnumerator Load_Next_Level(string next_level){
		yield return new WaitForSeconds (0.1f);
		Application.LoadLevel(next_level);
	}
		
}
