var skin:GUISkin;

private var levelValue = 1;

function Update () {
}

function OnGUI(){
	GUILayout.BeginArea(Rect (Screen.width / 2 - 100, Screen.height / 2 - 300, 200, 600));
	GUILayout.BeginVertical();
		GUILayout.Label("Game Type:",skin.GetStyle("Title"),GUILayout.Height(30)); 
		//Dynamic Button
		if(GUILayout.Button("Dynamic",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			PlayerPrefs.SetString("GameKind", "TimePlus");
			PlayerPrefs.SetInt("GameSpeed", levelValue);
			Application.LoadLevel("TetrisClone");
		}
		//Constant Button
		if(GUILayout.Button("Constant",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			PlayerPrefs.SetString("GameKind", "Constant");
			PlayerPrefs.SetInt("GameSpeed", levelValue);
			Application.LoadLevel("TetrisClone");
		}
		//Manual Button
		if(GUILayout.Button("Manual",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			PlayerPrefs.SetString("GameKind", "Manually");
			PlayerPrefs.SetInt("GameSpeed", levelValue);
			Application.LoadLevel("TetrisClone");
		}
		//Level
		GUILayout.Label("Level:" + levelValue,skin.GetStyle("Title"),GUILayout.Width(200)); 
		//Levels button		
		GUILayout.BeginHorizontal();
				if(GUILayout.Button("+",skin.GetStyle("button"),GUILayout.Height(50), GUILayout.Width(100)))
				{
					if(levelValue < 10)levelValue++;
				}
				if(GUILayout.Button("-",skin.GetStyle("button"),GUILayout.Height(50), GUILayout.Width(100)))
				{
					if(levelValue > 1) levelValue--;
				}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		//Return Button
		if(GUILayout.Button("Return",skin.GetStyle("button"),GUILayout.Width(200),GUILayout.Height(100)))
		{
			Application.LoadLevel("TetrisInit");
		}
	
	GUILayout.EndVertical();
	GUILayout.EndArea();
}