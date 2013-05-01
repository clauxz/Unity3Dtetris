using UnityEngine;
using System.Collections;

public class inter_level : MonoBehaviour {

	public string next_level = "main_menu";
	public string error_count = "";
	private string language = "english";
	private Transform title;
	private Transform message;
		
	// Use this for initialization
	void Start () {
		GameObject next_level_indicator=GameObject.FindWithTag("next_level_indicator");
		if(next_level_indicator!=null){
			next_level="level_"+next_level_indicator.GetComponent<next_level_indicator>().Get_Level();
			error_count=next_level_indicator.GetComponent<next_level_indicator>().Get_Error_Count();
			Destroy(next_level_indicator);
		}
		
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		
		title=transform.Find("scene_title");
		message=transform.Find("message");
		
		if(language=="english"){
			if(error_count=="")
				title.guiText.text="There are 5 Errors in the next level please find them.";
			else
				title.guiText.text="There are "+error_count+" Errors in the next level please find them.";
			message.guiText.text="If you are ready please press or touch any key to start the game ";
		}
		if(language=="japanese"){
			if(error_count=="")
				title.guiText.text="次のレベルでは、全部で５つの間違いあります。間違いを全て見つけて下さい。";
			else
				title.guiText.text="次のレベルでは、全部で"+error_count+"つの間違いあります。間違いを全て見つけて下さい。";
			message.guiText.text="準備ができたら、何かボタンを押して下さい。ゲームが始まります。";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad>0.1){
			if(Input.anyKey) StartCoroutine(Load_Next_Level(next_level));
		}
			
		if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began) StartCoroutine(Load_Next_Level(next_level));
	}
	
	IEnumerator Load_Next_Level(string next_level){
		float time_load=Time.time;
		while(true){
			if(language=="english"){
				message.guiText.text="Game start in "+Mathf.Ceil(time_load+3.0f-Time.time)+"sec";
			}
			else if(language=="japanese"){
				message.guiText.text="\n"+Mathf.Ceil(time_load+3.0f-Time.time)+"でゲームスタート";
			}
			if(time_load+3.0f-Time.time<=0) break;
			yield return null;
		}
		//~ yield return new WaitForSeconds (0.1f);
		Application.LoadLevel(next_level);
	}
}
