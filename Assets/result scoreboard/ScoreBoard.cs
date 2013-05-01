using UnityEngine;
using System.Collections;
using System.IO;

public class ScoreBoard : MonoBehaviour {
	
	public GUISkin skin; // Skin
	
	private Vector2 scrollPosition = new Vector2(50, 10);//inital position of scoreboard
	private bool managedScore = false; //let manage the score(to delete them).
	private bool showPlayerList = true; //show the list of players
	private string showNamePlayerData = "";//name of player must be shown
	
	private string language = "english";

	// Use this for initialization
	void Start () {
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetButtonDown("Fire2"))
		{
			managedScore = !managedScore;
		}
	}
	
	// Update GUI
	void OnGUI(){

		GUILayout.BeginArea(new Rect(50, 10, 1260, 790));
			if(showPlayerList)
				ShowPlayerList();
			else
				ShowResultList();
		
		//TO DELETE
		/*if(GUI.Button(new Rect(0, 740, 120, 40), "Register"))
		{
			Application.LoadLevel("register");
		}*/
		//END TO DELETE	
		GUILayout.EndArea();
	}
	
	//Show List of Players have had played.
	void ShowPlayerList()
	{
		// total of players have had played
		int totalPlayers = PlayerPrefs.GetInt("totalPlayers", 0); //get total player
	
		//Header	
		GUILayout.Label("ScoreBoard (ctrl + alt to manage)", skin.GetStyle("Header1"));
		GUILayout.BeginHorizontal();
			if(language=="english"){
				Label("Name", true, 300);		//Name
				//Label("Age", true); 			//Age
				//Label("Occupation", true); 		//Status
				//Label("Gender", true);		//Gender
				Label("Platform", true, 100); 	//Platform
				Label("GameType", true, 100); 	//Platform
			}
			else if(language=="japanese"){
				Label("名前", true);		//Name
				//~ Label("年齢", true); 			//Age
				//~ Label("職業", true); 		//Status
				//~ Label("性別", true);		//Gender
				Label("Platform", true); 	//Platform
				Label("GameType", true); 	//Platform
			}
		GUILayout.EndHorizontal(); //end Header
		
		//Content
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width (1200), GUILayout.Height (700));
			// loop for total players
			for(int i =0; i < totalPlayers; i++) 
			{
				//current player id
				string pN = "spot_player_" + (i+1).ToString();  
				
				//if player is the current player and has information about this level and it is active, we show up!
				if(GetValue( pN +"_actived", "undefined") == "true") 
				{
					GUILayout.BeginHorizontal();
						Label( GetValue( pN +"_name", "undefined"), false, 300); 						//get player name
						//Label( GetValue( pN +"_age", "undefined"));  						//get age
						//Label( GetValue( pN +"_occupation", "undefined"));				//get occupation
						//Label( GetValue( pN +"_gender", "undefined"));                    //get gender
						Label( GetValue( pN +"_platform", "undefined"), false, 100);                  //get platform
						Label( GetValue( pN +"_gametype", "undefined"), false, 100);                  //get platform
						
						//show result details
						if(Button( "More Results"))
						{
							showPlayerList = false;
							showNamePlayerData = pN;
						}
						
						if(managedScore)
						{
							//delete result
							if(Button( "Delete Results"))
							{ 
								SetValue( pN +"_actived", "false");
							}
						}
					GUILayout.EndHorizontal();
				}
			}
		GUILayout.EndScrollView (); //End of Content
	}
	
	void ShowResultList()
	{
		string pN = showNamePlayerData;
		GUILayout.Label("Game Results for " + GetValue( pN +"_name"), skin.GetStyle("Header1"));
		GUILayout.BeginHorizontal();
			if(language=="english"){
				Label("Level Type", true, 150);				//Level Type
				Label("Time Taken", true); 			//Time Taken
				Label("N. Clicks", true);		//Number of Click
				Label("Error Click", true);				//Error Click
				Label("Error Found", true);				//Error Found
				Label("N. Resets", true);		//Number of Reset
				Label("Level Was", true);		//Number of Skip
			}
			else if(language=="japanese"){
				Label("レベル Type", true, 150);				//Level Type
				Label("時間", true); 			//Time Taken
				Label("クリック回数", true);		//Number of Click
				Label("エラーをクリック", true);				//Error Click
				Label("Error Found", true);				//Error Found
				Label("リセット回数", true);		//Number of Reset
				Label("跳躍回数", true);		//Number of Skip
			}
			//Back to Player List
			if(Button("Back", 80))
			{
				showPlayerList = true;
			}
		GUILayout.EndHorizontal(); //end Header
		
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width (1200), GUILayout.Height (700));
			for(int l = 0; l < 18; l++)//loop to get result of all levels.
			{				
				string lvl = l.ToString(); //current level
				//if player is the current player and has information about this level and it is active, we show up!
				if(	GetValue( pN + "_level" + lvl + "_steps", "undone") != "undone") 
				{
					GUILayout.BeginHorizontal();
						Label( GetLevelName(l), false, 150);
						Label( GetValue( pN +"_level" + lvl + "_time"));   		//get time
						Label( GetValue( pN +"_level" + lvl + "_steps"));     	//get clicks
						Label( GetValue( pN +"_level" + lvl + "_wrong"));   	//get errors
						Label( GetValue( pN +"_level" + lvl + "_errors"));   	//get errors
						Label( GetValue( pN +"_level" + lvl + "_resets"));   	//get reset
						Label( GetValue( pN +"_level" + lvl + "_skips"));   	//get skips
					GUILayout.EndHorizontal();
				}
			}
		GUILayout.EndScrollView ();
	}
	

	string GetLevelName(int level)
	{
		switch(level){
			//~ case 0:		return "Tutorial Level 1";
			case 1:		return "GameA ToyPlane";
			case 2:		return "GameA Merry-Go-Around";
			case 3:		return "GameA Tractor";
			case 4:		return "GameA Extractor";
			case 5:		return "GameA FireTruck";
			case 6:		return "GameB ClassicCar";
			case 7:		return "GameB ToyCraneTruck";
			case 8:		return "GameB MarsRover";
			case 9:		return "GameB M3-Miner";
			case 10:	return "GameB ToyTrain";
			//~ case 11:	return "Level 2 Model 4";
			//~ case 12:	return "Level 2 Model 5";
			//~ case 13:	return "Level 3 Model 1";
			//~ case 14:	return "Level 3 Model 2";
			//~ case 15:	return "Level 3 Model 3";
			//~ case 16:	return "Level 3 Model 4";
			//~ case 17:	return "Level 3 Model 5";
			default:		return "Level ?";
		}
	}
	
	//Aux functions to shorter code. //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	string GetValue(string paramName, string valueDefault = "undefined")
	{
		return PlayerPrefs.GetString(paramName, valueDefault);
	}	
	
	void SetValue(string paramName, string value)
	{
		PlayerPrefs.SetString(paramName, value);
	}
	
	void Label(string text, bool headerStyle = false, int width = 120)
	{
		GUILayout.Label(text, skin.GetStyle(headerStyle ? "ScoreboardHeader" : "Scoreboard"), GUILayout.MinWidth(width), GUILayout.MaxWidth(width)); 	
	}
	
	bool Button(string text, int width = 120)
	{
		if(GUILayout.Button( text, skin.GetStyle("Scoreboard"), GUILayout.MinWidth(width), GUILayout.MaxWidth(width)))
			return true;
		else
			return false;
	}
	
}
