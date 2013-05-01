using UnityEngine;
using System.Collections;

public class main_menu : MonoBehaviour {

	public string next_level;
	public Transform title;
	public Transform button_start;
	public Transform button_tutorial;
	public Transform button_scoreboard;
	public Transform button_language;
	public Transform button_quit;
	
	public string language = "english";
	public bool using_mouse=false;
	GameObject language_indicator;
	
	// Use this for initialization
	void Start () {
		language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		

		if(language=="english"){
			title.guiText.text="Main Menu";
			button_start.guiText.text="Start";
			button_tutorial.guiText.text="Tutorial & Practice";
			button_scoreboard.guiText.text="ScoreBoard";
			button_language.guiText.text="Change Language";
			button_quit.guiText.text="Exit";
		}
		else if(language=="japanese"){
			title.guiText.text="¥á¥Ë¥å©`";
			button_start.guiText.text="¥¹¥¿©`¥È";
			button_tutorial.guiText.text="¥Á¥å©`¥È¥ê¥¢¥ë&¾šÁ•";
			button_scoreboard.guiText.text="µÃµã±í";
			button_language.guiText.text="Change Language";
			button_quit.guiText.text="½K¤ï¤ê";
		}
		
		button_start.gameObject.AddComponent<button>();
		button_tutorial.gameObject.AddComponent<button>();
		button_scoreboard.gameObject.AddComponent<button>();
		button_language.gameObject.AddComponent<button>();
		button_quit.gameObject.AddComponent<button>();
		
		Color temp_colour=Color.white;
		button_start.gameObject.SendMessage("SetColour", temp_colour);
		button_tutorial.gameObject.SendMessage("SetColour", temp_colour);
		button_scoreboard.gameObject.SendMessage("SetColour", temp_colour);
		button_language.gameObject.SendMessage("SetColour", temp_colour);
		button_quit.gameObject.SendMessage("SetColour", temp_colour);
	}
	
	// Update is called once per frame
	void Update () {
		
		//~ int count = iPhoneInput.touchCount;
		if(!using_mouse){
			if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began){
				iPhoneTouch touch = iPhoneInput.GetTouch(0);
				Test_Button_Pressed(touch.position);
			}	 
		}
		else if(using_mouse){
			if(Input.GetMouseButtonDown(0)){
				Vector2 pos=Input.mousePosition;
				Test_Button_Pressed(pos);
			}
		}
		
		if(iPhoneInput.touchCount==1){
			iPhoneTouch touch = iPhoneInput.GetTouch(0);
			if (button_start.guiText.HitTest( touch.position )){
				button_start.guiText.material.color=Color.green;
			}
			else if (button_tutorial.guiText.HitTest( touch.position )){
				button_tutorial.guiText.material.color=Color.green;
			}
			else if (button_scoreboard.guiText.HitTest( touch.position )){
				button_scoreboard.guiText.material.color=Color.green;
			}
			else if (button_language.guiText.HitTest( touch.position )){
				button_language.guiText.material.color=Color.green;
			}
			else if (button_quit.guiText.HitTest( touch.position )){
				button_quit.guiText.material.color=Color.green;
			}	
			else{
				button_start.guiText.material.color=Color.white;
				button_tutorial.guiText.material.color=Color.white;
				button_scoreboard.guiText.material.color=Color.white;
				button_language.guiText.material.color=Color.white;
				button_quit.guiText.material.color=Color.white;
			}
		}
		//~ else{
			//~ button_start.guiText.material.color=Color.white;
			//~ button_tutorial.guiText.material.color=Color.white;
			//~ button_scoreboard.guiText.material.color=Color.white;
			//~ button_quit.guiText.material.color=Color.white;
		//~ }

	}
	
	//test if a button has been pressed
	void Test_Button_Pressed(Vector2 pos){
		if (button_start.guiText.HitTest( pos )){
			button_start.guiText.material.color=Color.red;
			StartCoroutine(Load_Next_Level("gameset_select"));
		}
		else if (button_tutorial.guiText.HitTest( pos )){
			button_tutorial.guiText.material.color=Color.red;
			StartCoroutine(Load_Next_Level("level_0.1"));
		}
		else if (button_scoreboard.guiText.HitTest( pos )){
			button_scoreboard.guiText.material.color=Color.red;
			StartCoroutine(Load_Next_Level("score"));
		}
		else if (button_language.guiText.HitTest( pos )){
			button_language.guiText.material.color=Color.red;

			if(language=="english") language="japanese";
			else if(language=="japanese") language="english";
			
			language_indicator=GameObject.FindWithTag("language_indicator");
			if(language_indicator!=null){
				language_indicator.SendMessage("Set_Language", language);
			}
			else{
				language_indicator = Instantiate(Resources.Load("language_indicator")) as GameObject;
				if(language_indicator!=null){
					language_indicator.SendMessage("Set_Language", language);
					DontDestroyOnLoad(language_indicator);
				}
			}
			
			Start();
		}
		else if (button_quit.guiText.HitTest( pos )){
			button_quit.guiText.material.color=Color.red;
			Application.Quit();
		}
	}
	
	IEnumerator Load_Next_Level(string next_level){
		yield return new WaitForSeconds (0.1f);
		Application.LoadLevel(next_level);
	}
		
}
