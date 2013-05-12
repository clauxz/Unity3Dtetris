using UnityEngine;
using System.Collections;
using System.IO;

public class ScoreboardScreen : MonoBehaviour {
	
	//Our Row Prefab Stores Here
	public GameObject prefab;
	public GameObject camera;
	
	private int totalPlayers;
	private string[] names;
	private float[] times;
	private int[] rows;
	private float[] levels;
	private int[] scores;
	private string[] games;
	public string[] status;
	public string[] prefixes;
	public int[] numOfRotates;
	public int[] numOfDrops;
	public int[] numOfBlockGen;
	public int[] numOfSpeedUp;
	public int[] numOfSpeedDown;
	public string[] startingDates;
	
	public static ScoreboardScreen instance;
	
	//private Vector2 scrollPosition = new Vector2(20, 20);
	
	// Use this for initialization
	void Start () {
		instance=this;
		totalPlayers = PlayerPrefs.GetInt("Players", 0);
		names = new string[totalPlayers];
		times = new float[totalPlayers];
		rows = new int[totalPlayers];
		levels = new float[totalPlayers];
		scores = new int[totalPlayers];
		games = new string[totalPlayers];
		status = new string[totalPlayers];
		prefixes = new string[totalPlayers];
		numOfBlockGen = new int[totalPlayers];
		numOfRotates = new int[totalPlayers];
		numOfDrops = new int[totalPlayers];
		numOfSpeedDown= new int[totalPlayers];
		numOfSpeedUp = new int[totalPlayers];
		startingDates = new string[totalPlayers];
		
		for(int i = 1; i < totalPlayers; i++)
		{
			var prefix = "Player"+i.ToString();
			names[i] = PlayerPrefs.GetString(prefix+"_name");
			times[i] = PlayerPrefs.GetFloat(prefix+"_timetaken");
			rows[i] = PlayerPrefs.GetInt(prefix+"_rows");
			levels[i] = PlayerPrefs.GetFloat(prefix+"_level");
			scores[i] = PlayerPrefs.GetInt(prefix+"_score");
			games[i] = PlayerPrefs.GetString(prefix+"_game");
			status[i] = PlayerPrefs.GetString(prefix+"_status");
			prefixes[i] = PlayerPrefs.GetString(prefix+"_prefix");
			numOfBlockGen[i] = PlayerPrefs.GetInt(prefix+"_numOfBlockGen");
			numOfRotates[i] = PlayerPrefs.GetInt(prefix+"_numOfRotations");
			numOfDrops[i] = PlayerPrefs.GetInt(prefix+"_numOfDropPressed");
			numOfSpeedDown[i] = PlayerPrefs.GetInt(prefix+"_numOfSpeedDown");
			numOfSpeedUp[i] = PlayerPrefs.GetInt(prefix+"_numOfSpeedUp");
			
			startingDates[i] = PlayerPrefs.GetString(prefix+"_startingDate");
			
		}
	
	//Sorting by Higher Score
	for(int i = 1; i < totalPlayers; i++)
	{
		for(int j = i + 1; j < totalPlayers; j++)
		{
			if(scores[j] > scores[i])
			{
				string nameaux = names[j];
				names[j] = names[i];
				names[i] = nameaux;
				
				float timeaux = times[j];
				times[j] = times[i];
				times[i] = timeaux;	
				
				int rowaux = rows[j];
				rows[j] = rows[i];
				rows[i] = rowaux;
				
				float levelaux = levels[j];
				levels[j] = levels[i];
				levels[i] = levelaux;	
				
				int scoreaux = scores[j];
				scores[j] = scores[i];
				scores[i] = scoreaux;	
					
				int numOfRotatesAux = numOfRotates[j];
				numOfRotates[j] = numOfRotates[i];
				numOfRotates[i] = numOfRotatesAux;
					
				int numOfDropsAux = numOfDrops[j];
				numOfDrops[j] = numOfDrops[i];
				numOfDrops[i] = numOfDropsAux;
					
				int numOfBlockGenAux = numOfBlockGen[j];
				numOfBlockGen[j] = numOfBlockGen[i];
				numOfBlockGen[i] = numOfBlockGenAux;
					
				int numOfSpeedUpAux = numOfSpeedUp[j];
				numOfSpeedUp[j] = numOfSpeedUp[i];
				numOfSpeedUp[i] =numOfSpeedUpAux;
					
				int numOfSpeedDownAux = numOfSpeedDown[j];
				numOfSpeedDown[j] = numOfSpeedDown[i];
				numOfSpeedDown[i] = numOfSpeedDownAux;
					
				string startingDatesaux = startingDates[j];
				startingDates[j] = startingDates[i];
				startingDates[i] = startingDatesaux;
				
				string gameaux = games[j];
				games[j] = games[i];
				games[i] = gameaux;
					
				string statusaux = status[j];
				status[j] = status[i];
				status[i] = statusaux;
					
				string prefaux = prefixes[j];
				prefixes[j] = prefixes[i];
				prefixes[i] = prefaux;
			}
		}
	}
	
		//Populate Scores
		PopulateData();
		
	}
	
	
	/// <summary>
	/// Populates the data.
	/// </summary>
	public void PopulateData()
	{
		for(int i = 1; i < totalPlayers; i++) 
				{
					if(status[i] == "active")
					{
						GameObject nItem = (GameObject)Instantiate(prefab);
						nItem.name="item"+i;
						nItem.transform.parent=this.transform;
				
						nItem.transform.localPosition=Vector3.zero;
				
						nItem.transform.localScale=Vector3.one*(float)0.55;
				
						UILabel[] array = nItem.GetComponentsInChildren<UILabel>();
						array[0].text=string.Format("{0:F1}", times[i]);
						array[1].text=games[i];
				
						array[2].text=levels[i].ToString();
						array[3].text=names[i];
						array[4].text=rows[i].ToString();
						array[5].text=scores[i].ToString();
						array[6].text=numOfRotates[i].ToString();
				array[7].text=numOfDrops[i].ToString();
				array[8].text=numOfBlockGen[i].ToString();
				array[9].text=numOfSpeedUp[i].ToString();
				array[10].text=numOfSpeedDown[i].ToString();
				array[11].text=startingDates[i];//numOfRotates[i].ToString();
						GameObject.Find("Delete").name="delete"+i;
						nItem.GetComponent<UIDragCamera>().draggableCamera=camera.GetComponent<UIDraggableCamera>();
			/*			GUILayout.BeginHorizontal();
							Label( names[i], false, 100);
							Label( games[i], false, 100);
							Label( levels[i].ToString(), false, 100);
							Label( rows[i].ToString(), false, 100);
							Label( string.Format("{0:F1}", times[i]), false, 100);
							Label( scores[i].ToString(), false, 100);
							if(Button("X", 100))
							{
								DeletePlayer(prefixes[i]);
								status[i] = PlayerPrefs.GetString(prefixes[i]+"_status");
							}
						GUILayout.EndHorizontal();*/
					}
				}
		this.GetComponent<UITable>().repositionNow=true;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
/*	void OnGUI () {
		GUILayout.BeginArea( new Rect(Screen.width/2-350, 10, 700, Screen.height-20));
			Label("ScoreBoard", true, 700);
			//Build scoreboard header
			GUILayout.BeginHorizontal();
				Label("Name", true, 100);			
				Label("Game", true, 100); 			
				Label("Level", true, 100); 			
				Label("Destroyed Rows", true, 100);		
				Label("Time", true, 100);				
				Label("Score", true, 100);		
				Label("Delete", true, 100);
			GUILayout.EndHorizontal();
			
			//Fill the scoreboard with database data
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width-200), GUILayout.Height ( Screen.height-100));
				for(int i = 1; i < totalPlayers; i++) 
				{
					if(status[i] == "active")
					{
						GUILayout.BeginHorizontal();
							Label( names[i], false, 100);
							Label( games[i], false, 100);
							Label( levels[i].ToString(), false, 100);
							Label( rows[i].ToString(), false, 100);
							Label( string.Format("{0:F1}", times[i]), false, 100);
							Label( scores[i].ToString(), false, 100);
							if(Button("X", 100))
							{
								DeletePlayer(prefixes[i]);
								status[i] = PlayerPrefs.GetString(prefixes[i]+"_status");
							}
						GUILayout.EndHorizontal();
					}
				}
			GUILayout.EndScrollView ();
			
			GUILayout.BeginHorizontal();
				//Export button
				if(Button("Export CSV", 100))
				{
					ExportToCSV();
				}
				//Delete db button
				if(Button("Delete DB", 100))
				{
					PlayerPrefs.SetInt("Players", 1);
					Application.LoadLevel("TetrisScore");
				}
				//Back button
				if(Button("Back", 100))
				{
					Application.LoadLevel("TetrisInit");
				}
			GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
	}*/
	
	//Export to a csv file the database
	/// <summary>
	/// Exports to CS.
	/// </summary>
	public void ExportToCSV()
	{
		StreamWriter sw = new StreamWriter(Application.dataPath + "/export.csv");
		string message = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "Name", "Game", "Level","Destroyed Rows","Time","Score","Rotates","Drops","Blocks","SpeedUps","SpeedDowns","DateTime");
		sw.WriteLine(message);
		for(int i = 1; i < totalPlayers; i++)
		{
			if(status[i] == "active")
			{
				message = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", names[i], games[i], levels[i].ToString(),rows[i].ToString(),string.Format("{0:F1}", times[i]),scores[i].ToString(),numOfRotates[i].ToString(),numOfDrops[i].ToString(),numOfBlockGen[i].ToString(),numOfSpeedUp[i].ToString(),numOfSpeedDown[i].ToString(),startingDates[i]);
				sw.WriteLine(message);
			}
		}
		sw.Close();
	
	}
	/// <summary>
	/// Deletes the player.
	/// </summary>
	/// <param name='prefix'>
	/// Prefix.
	/// </param>
	public void DeletePlayer(string prefix)
	{
		Debug.Log(PlayerPrefs.GetString(prefix+"_status"));
		PlayerPrefs.SetString(prefix+"_status", "deleted");
		//Application.LoadLevel("TetrisScore");
	}
	
	//Aux functions//
/*	void Label(string text, bool headerStyle, int width)
	{
		GUILayout.Label(text, skin.GetStyle(headerStyle ? "sbh" : "sb"), GUILayout.Width(width), GUILayout.Height(25)); 	
	}
	
	bool Button(string text, int width)
	{
		if(GUILayout.Button( text, skin.GetStyle("sbbutton"), GUILayout.Width(width), GUILayout.Height(25)))
			return true;
		else
			return false;
	}*/
}
