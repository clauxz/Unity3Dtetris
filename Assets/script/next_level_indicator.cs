using UnityEngine;
using System.Collections;

public class next_level_indicator : MonoBehaviour {

	string next_level="main_menu";
	string error_count="";
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public string Get_Level(){
		return next_level;
	}
	
	public void Set_Level(string next_lvl){
		next_level=next_lvl;
	}
	
	public string Get_Error_Count(){
		return error_count;
	}
	
	public void Set_Error_Count(string count){
		error_count=count;
	}
}
