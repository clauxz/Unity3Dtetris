using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public GUISkin skin;
	public string levelToLoadAfterRegister;
	
	private string playerName = "";
	//~ private int ageGrid = 0;
	//~ private string[] ages = new string[] {"<17", "18-22", "23-30", "31-45", "46-64", "65>"};
	//~ private string status = "";
	//~ private int genderGrid = 0;
	//~ private string[] genders = new string[] { "male", "female" };
	private string result = "";
	private bool typegame = false;
	private bool platform = false;
	
	//TO DELETE
	/*private string _level = "Level Number (1 ~ 18)";
	private string _steps = "Number Clicks";
	private string _time = "Time Taken";
	private string _errors = "Error Clicks";
	private string _reset = "Resets";
	private string _skip = "Skips";
	private bool showError = false;
	private bool showResult = false;*/
	//END TO DELETE
	
	private string language = "english";
	
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString("currentPlayer", "");
		//PlayerPrefs.SetInt("totalPlayers", 0);
		
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		
		//~ if(language=="japanese") genders = new string[] { "男性", "女性" };
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if(language=="english"){
			GUI.BeginGroup(new Rect(0, 0, 1280, 800));
			GUI.Label(new Rect(580,50, 120, 40), "Register", skin.GetStyle("Header1"));
			
			GUI.Label(new Rect(580,100,120,20), "Name", skin.GetStyle("Header2"));
			playerName = GUI.TextField(new Rect(580,120, 120, 20), playerName);
			
			/*GUI.Label(new Rect(580,160,120,20), "Age", skin.GetStyle("Header2"));
			ageGrid = GUI.SelectionGrid(new Rect(580, 180, 120, 120), ageGrid, ages, 1);
			
			GUI.Label(new Rect(580,330,120,20), "Occupation", skin.GetStyle("Header2"));
			status = GUI.TextField(new Rect(580,350, 120, 20), status);
			
			GUI.Label(new Rect(580,400,120,20), "Gender", skin.GetStyle("Header2"));
			genderGrid = GUI.SelectionGrid(new Rect(580, 420, 120, 20), genderGrid, genders, 2);*/
			
			typegame = GUI.Toggle(new Rect(580, 150, 80, 20), typegame, "Game A");
			typegame = GUI.Toggle(new Rect(650, 150, 80, 20), !typegame, "Game B");		
			
			platform = GUI.Toggle(new Rect(580, 170, 80, 20), platform, "PC");
			platform = GUI.Toggle(new Rect(650, 170, 80, 20), !platform, "MT");
			
			if(GUI.Button(new Rect(580, 200, 120, 40), "Save"))
			{
				//showError = false;
				SaveInfos();
				StartCoroutine(SendPlayerToDB());
			}
		}
		else if(language=="japanese"){
			GUI.BeginGroup(new Rect(0, 0, 1280, 800));
			GUI.Label(new Rect(580,50, 120, 40), "登録する", skin.GetStyle("Header1"));
			
			GUI.Label(new Rect(580,100,120,20), "名前", skin.GetStyle("Header2"));
			playerName = GUI.TextField(new Rect(580,120, 120, 20), playerName);
			
			//~ GUI.Label(new Rect(580,160,120,20), "年齢", skin.GetStyle("Header2"));
			//~ ageGrid = GUI.SelectionGrid(new Rect(580, 180, 120, 120), ageGrid, ages, 1);
			
			//~ GUI.Label(new Rect(580,330,120,20), "職業", skin.GetStyle("Header2"));
			//~ status = GUI.TextField(new Rect(580,350, 120, 20), status);
			
			//~ GUI.Label(new Rect(580,400,120,20), "性別", skin.GetStyle("Header2"));
			//~ genderGrid = GUI.SelectionGrid(new Rect(580, 420, 120, 20), genderGrid, genders, 2);
			
			typegame = GUI.Toggle(new Rect(580, 150, 80, 20), typegame, "Game A");
			typegame = GUI.Toggle(new Rect(650, 150, 80, 20), !typegame, "Game B");		
			
			platform = GUI.Toggle(new Rect(580, 170, 80, 20), platform, "PC");
			platform = GUI.Toggle(new Rect(650, 170, 80, 20), !platform, "MT");
			
			if(GUI.Button(new Rect(580, 500, 120, 40), "続ける"))
			{
				//showError = false;
				SaveInfos();
				StartCoroutine(SendPlayerToDB());
			}
		}
		
		//TO DELETE
		/*if(GUI.Button(new Rect(50, 750, 120, 40), "Scoreboard"))
		{
			Application.LoadLevel("score");
		}
		
		GUILayout.BeginArea(new Rect(50, 680, 1180, 780));
			Label("Level Simulator");
			GUILayout.BeginHorizontal();
				_level = GUILayout.TextField (_level, 25);
				_steps = GUILayout.TextField (_steps, 25);
				_time = GUILayout.TextField (_time, 25);
				_errors =GUILayout.TextField (_errors, 25);
				_reset = GUILayout.TextField (_reset, 25);
				_skip = GUILayout.TextField (_skip, 25);
				if(GUILayout.Button("Send To DB"))
				{
					if(GetValue("currentPlayer") == "")
						showError = true;
					SaveScore(int.Parse(_level), int.Parse(_steps), int.Parse(_time), int.Parse(_errors), int.Parse(_reset), int.Parse(_skip));
				}
			GUILayout.EndHorizontal();
			if(showError)
				Label("Save Player First");
		GUILayout.EndArea();
		
		if(showResult)
		{
			if(GUI.Button(new Rect(580, 550, 120, 40), "Player DB ID is: " + result))
				showResult = false;
		}*/			
		//END TO DELETE
		
		GUI.EndGroup();

	}
	
	//Save information about player
	void SaveInfos(){
		if(name == "")
			return;
		
		int totalPlayers = PlayerPrefs.GetInt("totalPlayers", 0);
		totalPlayers++;
		PlayerPrefs.SetInt("totalPlayers", totalPlayers);
		
		string currentPlayer = "spot_player_" + totalPlayers.ToString();

		SetValue("currentPlayer", currentPlayer);
		SetValue(currentPlayer + "_name", playerName);
		
		/*string age = "";
		switch(ageGrid){
			case 0: age = "<17"; break;
			case 1: age = "18-22"; break;
			case 2: age = "23-30"; break;
			case 3: age = "31-45"; break;
			case 4: age = "46-64"; break;
			case 5: age = "65>"; break;
		}*/

		//SetValue(currentPlayer + "_age", age);
		//SetValue(currentPlayer + "_occupation", status);
		//SetValue(currentPlayer + "_gender", genderGrid == 0 ? "male" : "female");
		SetValue(currentPlayer + "_actived", "true");
		SetValue(currentPlayer + "_platform", platform ? "MT" : "PC");
		SetValue(currentPlayer + "_gametype", typegame ? "Game B" : "Game A");

		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null){
			language_indicator.GetComponent<language_indicator>().Set_Platform(platform ? "MT" : "PC");
		}

	}
	
	//Save information about the level has been completed
	public void SaveScore(string level, string steps, string time, bool[] errors, string reset, string skip, string flick, string zoomIn, string zoomOut, string pan){
		string currentPlayer = GetValue("currentPlayer");
		
		int error_count = 0;
		for(int i = 0; i < 5; i++)
			if(errors[i]) error_count++;
				
		//~ int itime = (int)Mathf.Floor(float.Parse(time));
		//~ int itime = time;
		int isteps = int.Parse(steps);
		
		SetValue(currentPlayer + "_level" + level + "_steps", steps);
		//~ SetValue(currentPlayer + "_level" + level + "_time", skip == "skipped" ? "60" : itime.ToString());
		SetValue(currentPlayer + "_level" + level + "_time", time);
		SetValue(currentPlayer + "_level" + level + "_wrong", (isteps - error_count).ToString());
		SetValue(currentPlayer + "_level" + level + "_errors", error_count.ToString());
		SetValue(currentPlayer + "_level" + level + "_error1", errors[0] ? "Found" : "Not Found");
		SetValue(currentPlayer + "_level" + level + "_error2", errors[1] ? "Found" : "Not Found");
		SetValue(currentPlayer + "_level" + level + "_error3", errors[2] ? "Found" : "Not Found");
		SetValue(currentPlayer + "_level" + level + "_error4", errors[3] ? "Found" : "Not Found");
		SetValue(currentPlayer + "_level" + level + "_error5", errors[4] ? "Found" : "Not Found");
		SetValue(currentPlayer + "_level" + level + "_resets", reset);	
		SetValue(currentPlayer + "_level" + level + "_skips", skip);
		SetValue(currentPlayer + "_level" + level + "_flick", flick);
		SetValue(currentPlayer + "_level" + level + "_zoomin", zoomIn);
		SetValue(currentPlayer + "_level" + level + "_zoomout", zoomOut);
		SetValue(currentPlayer + "_level" + level + "_pan", pan);
		
		StartCoroutine(SendLevelToDB(level.ToString()));
	}
	
	IEnumerator SendLevelToDB(string levelId)
	{
		Debug.Log("Called SendLevelToDB " + levelId);
		string pN = GetValue("currentPlayer");
		Debug.Log("Player saved is: " + GetValue( pN +"_dbid", "-1"));
		WWWForm form = new WWWForm();
		form.AddField("playerid", GetValue( pN +"_dbid", "-1")); 
		form.AddField("levelid", levelId); 
		form.AddField("levelname", GetLevelName(int.Parse(levelId))); 
		form.AddField("time", GetValue( pN + "_level" + levelId +"_time")); 
		form.AddField("clicks", GetValue( pN + "_level" + levelId +"_steps")); 
		form.AddField("wrong", GetValue( pN + "_level" + levelId +"_wrong")); 
		form.AddField("errors", GetValue( pN + "_level" + levelId +"_errors")); 
		form.AddField("error1", GetValue( pN + "_level" + levelId +"_error1")); 
		form.AddField("error2", GetValue( pN + "_level" + levelId +"_error2")); 
		form.AddField("error3", GetValue( pN + "_level" + levelId +"_error3")); 
		form.AddField("error4", GetValue( pN + "_level" + levelId +"_error4")); 
		form.AddField("error5", GetValue( pN + "_level" + levelId +"_error5")); 
		form.AddField("resets", GetValue( pN + "_level" + levelId +"_resets")); 
		form.AddField("skips", GetValue( pN + "_level" + levelId +"_skips")); 
		form.AddField("flick", GetValue( pN + "_level" + levelId +"_flick")); 
		form.AddField("zoomin", GetValue( pN + "_level" + levelId +"_zoomin")); 
		form.AddField("zoomout", GetValue( pN + "_level" + levelId +"_zoomout")); 
		form.AddField("pan", GetValue( pN + "_level" + levelId +"_pan")); 
		
		/*Debug.Log("playerid " + GetValue( pN +"_dbid", "-1"));
		Debug.Log("levelid " + levelId);
		Debug.Log("levelname " + GetLevelName(int.Parse(levelId)));
		Debug.Log("time " + GetValue( pN + "_level" + levelId +"_time"));
		Debug.Log("clicks " + GetValue( pN + "_level" + levelId +"_steps"));
		Debug.Log("errors " + GetValue( pN + "_level" + levelId +"_errors"));
		Debug.Log("resets" + GetValue( pN + "_level" + levelId +"_resets"));
		Debug.Log("skips " + GetValue( pN + "_level" + levelId +"_skips"));
		Debug.Log("flick " + GetValue( pN + "_level" + levelId +"_flick")); 
		Debug.Log("zoomin " + GetValue( pN + "_level" + levelId +"_zoomin")); 
		Debug.Log("zoomout " + GetValue( pN + "_level" + levelId +"_zoomout")); */
		
		//WWW www = new WWW("http://localhost/unity/level_register.php", form);
		WWW www = new WWW("http://hoshi-zora.org/spot3d/level_register.php", form);
		yield return www;
		
		if (www.error != null) { 
			result = www.error; 
			Debug.Log(result);
		}else { 
			result = www.text;
			Debug.Log(result);
			//showResult = true;
		}
		yield return 0;
	}
	
	//Send information to a php page.
	IEnumerator SendPlayerToDB()
	{
		string pN = GetValue("currentPlayer");
		
		WWWForm form = new WWWForm();
		form.AddField("name", GetValue( pN +"_name")); 
		//form.AddField("age", GetValue( pN +"_age")); 
		//form.AddField("occupation", GetValue( pN +"_occupation")); 
		//form.AddField("gender", GetValue( pN +"_gender")); 
		form.AddField("platform", GetValue( pN +"_platform")); 
		form.AddField("gametype", GetValue( pN +"_gametype")); 
		
		/*Debug.Log(GetValue( pN +"_name"));
		Debug.Log(GetValue( pN +"_platform"));
		Debug.Log(GetValue( pN +"_gametype"));*/

		WWW www = new WWW("http://hoshi-zora.org/spot3d/player_register.php", form);
		//WWW www = new WWW("http://localhost/unity/player_register.php", form);
		yield return www;
		
		if (www.error != null) { 
			result = www.error; 
			Debug.Log(result);
		}else { 
			result = www.text;	
			Debug.Log(result);
			PlayerPrefs.SetString(pN + "_dbid", result);
			Application.LoadLevel(levelToLoadAfterRegister);
			//showResult = true;
		}
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
	
	void Label(string text, bool headerStyle = false)
	{
		GUILayout.Label(text, skin.GetStyle(headerStyle ? "ScoreboardHeader" : "Scoreboard"), GUILayout.MinWidth(160), GUILayout.MaxWidth(160)); 	
	}
	
	bool Button(string text)
	{
		if(GUILayout.Button( text, skin.GetStyle("Scoreboard"), GUILayout.MinWidth(160), GUILayout.MaxWidth(160)))
			return true;
		else
			return false;
	}
}
