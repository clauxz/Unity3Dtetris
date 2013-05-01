var skin:GUISkin;

function Update () {
	//PlayerPrefs.SetInt("Players", 1);
}

function OnGUI(){
	GUILayout.BeginArea(Rect (Screen.width / 2 - 100, Screen.height / 2 - 250, 200, 500));
	GUILayout.BeginVertical();
		GUILayout.Label("TETRIS",skin.GetStyle("Title"),GUILayout.Height(30)); 
		GUILayout.Label("\t\texpirement",skin.GetStyle("Title"),GUILayout.Height(30)); 
		GUILayout.Label("by Takahashilab",skin.GetStyle("SubTitle"),GUILayout.Height(30)); 
		//Start button
		if(GUILayout.Button("Start",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			Application.LoadLevel("TetrisRegister");
		}
		//Tutorial button
		if(GUILayout.Button("Tutorial",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			PlayerPrefs.SetString("GameKind", "Tutorial");
			Application.LoadLevel("TetrisClone");
		}
		//Score button
		if(GUILayout.Button("Score",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			Application.LoadLevel("TetrisScore");
		}
		//Exit button
		if(GUILayout.Button("Exit",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			Application.Quit();
		}
	
	GUILayout.EndVertical();
	GUILayout.EndArea();
}