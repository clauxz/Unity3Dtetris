var skin:GUISkin;
private var pname = "";

function Update () {
}

function OnGUI()
{
	GUILayout.BeginArea(Rect (Screen.width / 2 - 200, Screen.height / 2 - 275, 400, 550));
	GUILayout.BeginVertical();
		GUILayout.Label("Put Your Name:", skin.GetStyle("Title"));
		pname = GUILayout.TextField(pname, 10, skin.GetStyle("TextField"),GUILayout.Height(100) );
		GUILayout.BeginHorizontal();
			//Back button
			if(GUILayout.Button("Back",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
			{
				Application.LoadLevel("TetrisInit");
			}
			//Ok button
			if(GUILayout.Button("Ok",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
			{
				if(pname.Length > 0)
				{
					//Getting current player id
					/*var id = PlayerPrefs.GetInt("Players", 1);
					
					//Save the name of the current player
					PlayerPrefs.SetString("Player"+id.ToString()+"_Name", pname);
										
					//Sets the current player name
					PlayerPrefs.SetString("CurrentPlayer", "Player"+id.ToString());
										
					//Update player id for the next player
					PlayerPrefs.SetInt("Players", id + 1);*/
								
					PlayerPrefs.SetString("CurrentPlayerName", pname);
					//Starts the screen to choose game
					Application.LoadLevel("TetrisStart");
				}
			}			
		GUILayout.EndHorizontal();
	GUILayout.EndVertical();
	GUILayout.EndArea();
}