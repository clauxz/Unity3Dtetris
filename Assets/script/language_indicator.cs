using UnityEngine;
using System.Collections;

public class language_indicator : MonoBehaviour {

	public string language="english";
	public string platform="PC";
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Set_Language(string temp){
		language=temp;
	}
	
	public string Get_Language(){
		//~ print(language);
		return language;
	}
	
	public void Set_Platform(string temp){
		platform=temp;
		print(platform);
	}
	
	public string Get_Platform(){
		//~ print(language);
		return platform;
	}
}
