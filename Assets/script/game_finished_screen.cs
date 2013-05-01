using UnityEngine;
using System.Collections;

public class game_finished_screen : MonoBehaviour {

	private string language = "english";
	private Transform title;
	private Transform message;
		
	// Use this for initialization
	void Start () {
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		
		title=transform.Find("scene_title");
		message=transform.Find("message");
		
		if(language=="english"){
			title.guiText.text="This is the end of the game.\nThank you for playing";
			message.guiText.text="press any key or touch the screen to go back to main menu...";
		}
		else if(language=="japanese"){
			title.guiText.text="お疲れ様でした、これでゲームは終了です。ありがとうございました。";
			message.guiText.text="何かボタンを押して下さい。ゲームが終了します。";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey) StartCoroutine(Load_Next_Level("main_menu"));
		
		if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began) StartCoroutine(Load_Next_Level("main_menu"));
	}
	
	IEnumerator Load_Next_Level(string next_level){
		yield return new WaitForSeconds (0.1f);
		Application.LoadLevel(next_level);
	}
}
