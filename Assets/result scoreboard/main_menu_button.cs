using UnityEngine;
using System.Collections;

public class main_menu_button : MonoBehaviour {

	//~ private Transform button_mainmenu;
	
	private string language="english";
	
	// Use this for initialization
	void Start () {
		//~ button_mainmenu=transform.Find("button_mainmenu");
		transform.guiText.material.color=new Color(0.75f, 0.75f, 0.75f, 0.75f);
		
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		if(language=="japanese") transform.guiText.text="¥á¥Ë¥å©`";
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 pos=Input.mousePosition;
			if (transform.guiText.HitTest( pos )){
				transform.guiText.material.color=Color.red;
				StartCoroutine(Load_Next_Level("main_menu"));
			}
		}
		
		if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began){
			iPhoneTouch touch = iPhoneInput.GetTouch(0);
			if (transform.guiText.HitTest( touch.position )){
				transform.guiText.material.color=Color.red;
				StartCoroutine(Load_Next_Level("main_menu"));
			}
		}
		
		if(iPhoneInput.touchCount==1){
			//~ touch_state=true;
			iPhoneTouch touch = iPhoneInput.GetTouch(0);
			if (transform.guiText.HitTest( touch.position )){
				transform.guiText.material.color=Color.green;
			}
			else{
				transform.guiText.material.color=new Color(0.75f, 0.75f, 0.75f, 0.75f);
			}
		}
		//~ else{
			//~ transform.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
		//~ }
	}
	
	
	IEnumerator Load_Next_Level(string next_level){
		yield return new WaitForSeconds (0.1f);
		Application.LoadLevel(next_level);
	}
	
	void OnMouseDown(){
		transform.guiText.material.color=Color.red;
	}
	
	void OnMouseUp(){
		transform.guiText.material.color=Color.green;
	}
	
	void OnMouseEnter(){
		transform.guiText.material.color=Color.green;
	}
	
	void OnMouseExit(){
		transform.guiText.material.color=new Color(0.75f, 0.75f, 0.75f, 0.75f);
	}
}
