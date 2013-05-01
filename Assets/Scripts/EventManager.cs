using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	
	public ButtonType buttonEvent;
	
	
	//private int levelValue=1;
	
	void OnClick()
	{
		switch(buttonEvent)
		{
		case ButtonType.Start:
				Application.LoadLevel("TetrisRegister");
			break;
		case ButtonType.Tutorial:
			PlayerPrefs.SetString("GameKind", "Tutorial");
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Score:
			Application.LoadLevel("TetrisScore");
			break;
		case ButtonType.Exit:
			Application.Quit();
			break;
		case ButtonType.Register_ok:
			string pname = GameObject.Find("pname").GetComponent<UIInput>().text;
			if(pname.Length > 0)
				{
					
								
					PlayerPrefs.SetString("CurrentPlayerName", pname);
					//Starts the screen to choose game
					Application.LoadLevel("TetrisStart");
				}
			break;
		case ButtonType.Register_back:
			Application.LoadLevel("TetrisInit");
			break;
			
		case ButtonType.Dynamic:
			PlayerPrefs.SetString("GameKind", "TimePlus");
			PlayerPrefs.SetInt("GameSpeed", Helper.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Constant:
			PlayerPrefs.SetString("GameKind", "Constant");
			PlayerPrefs.SetInt("GameSpeed", Helper.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Manual:
			PlayerPrefs.SetString("GameKind", "Manually");
			PlayerPrefs.SetInt("GameSpeed", Helper.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Level_inc:
			//GameObject.Find("levelValue").GetComponent<UILabel>().text=levelValue.ToString();
			if(Helper.levelValue < 10){
				Helper.levelValue++;
			GameObject.Find("levelValue").GetComponent<UILabel>().text=Helper.levelValue.ToString();
			}
			
			break;
		case ButtonType.Level_dec:
			if(Helper.levelValue > 1) {
				Helper.levelValue--;
			GameObject.Find("levelValue").GetComponent<UILabel>().text=Helper.levelValue.ToString();
			}
			break;
		case ButtonType.Return:
			Application.LoadLevel("TetrisInit");
			break;
			
		}
		
	}
}
