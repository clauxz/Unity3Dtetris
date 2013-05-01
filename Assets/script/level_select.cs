using UnityEngine;
using System.Collections;

public class level_select : MonoBehaviour {

	public string next_level;
	public Transform button_lvl_1;
	public Transform button_lvl_2;
	public Transform button_lvl_3;
	public Transform button_lvl_4;
	public Transform scene_title;
	
	private string language = "english";
	
	// Use this for initialization
	void Start () {
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		
		if(language=="english"){
			scene_title.guiText.text="Select Level";
			button_lvl_1.guiText.text="Game A";
			button_lvl_2.guiText.text="Game B";
			//~ button_lvl_3.guiText.text="Level 3";
			//~ button_lvl_4.guiText.text="Level 4";
		}
		else if(language=="japanese"){
			scene_title.guiText.text="ﬂxík•Ï•Ÿ•Î";
			button_lvl_1.guiText.text="•≤©`•‡A";
			button_lvl_2.guiText.text="•≤©`•‡B";
			//~ button_lvl_3.guiText.text="•Ï•Ÿ•Î3";
			//~ button_lvl_4.guiText.text="Level 4";
		}
		
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
	
	private GameObject next_level_indicator;
	
	void Test_Button_Pressed(Vector2 pos){
		//~ int level;
		if (button_lvl_1.guiText.HitTest( pos )){
			button_lvl_1.guiText.material.color=Color.red;
			//~ level= Random.Range(1, 6);
			//~ level=1;
			
			next_level_indicator = Instantiate(Resources.Load("next_level_indicator")) as GameObject;
			if(next_level_indicator!=null){
				next_level_indicator.GetComponent<next_level_indicator>().Set_Level("1");
				DontDestroyOnLoad(next_level_indicator);
			}
			
			StartCoroutine(Load_Next_Level("inter_level"));
		}
		else if (button_lvl_2.guiText.HitTest( pos )){
			button_lvl_2.guiText.material.color=Color.red;
			//~ level= Random.Range(1, 3);
			//~ level=1;
			
			next_level_indicator = Instantiate(Resources.Load("next_level_indicator")) as GameObject;
			if(next_level_indicator!=null){
				next_level_indicator.GetComponent<next_level_indicator>().Set_Level("4");
				DontDestroyOnLoad(next_level_indicator);
			}
			
			StartCoroutine(Load_Next_Level("inter_level"));
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
