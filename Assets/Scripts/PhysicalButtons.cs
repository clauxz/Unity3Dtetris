using UnityEngine;
using System.Collections;

public class PhysicalButtons : MonoBehaviour {

	public bool inGame;
	
	public string prevPage;
	
	
	// Use this for initialization
	void Start () {
		prevPage= PlayerPrefs.GetString("GamePage","None");
	}
	
	// Update is called once per frame
	void Update () {
	//	if(Application.platform==RuntimePlatform.Android)
	//	{
			if(inGame)
			{
				if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
				{
					GameManager.instance.PauseGame();
					
				}
			}
			else
			{
				if (Input.GetKey(KeyCode.Escape))
				{
					
						if(prevPage=="None")
						{
							Application.Quit();
						}
						else
						{
							Debug.Log(prevPage);
							Application.LoadLevel(prevPage);
							PlayerPrefs.SetString("GamePage","None");
							
						}
				}
			}
			
	//	}
	}
}
