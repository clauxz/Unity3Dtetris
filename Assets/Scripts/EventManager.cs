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
			PlayerPrefs.SetString("GameKind", "DynamicM");
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
			Debug.Log((int)((GameManager.levelValue-1)*5));
			PlayerPrefs.SetInt("GameSpeed", GameManager.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.DynamicM:
			PlayerPrefs.SetString("GameKind", "DynamicM");
			PlayerPrefs.SetInt("GameSpeed", GameManager.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Constant:
			PlayerPrefs.SetString("GameKind", "Constant");
			PlayerPrefs.SetInt("GameSpeed", GameManager.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Manual:
			PlayerPrefs.SetString("GameKind", "Manually");
			PlayerPrefs.SetInt("GameSpeed", GameManager.levelValue);
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.Level_inc:
			
			if(GameManager.levelValue < 10){
				GameManager.levelValue++;
			GameObject.Find("levelValue").GetComponent<UILabel>().text=((((float)(GameManager.levelValue))/5)+.8f).ToString();
			}
			
			break;
		case ButtonType.Level_dec:
			if(GameManager.levelValue > 1) {
				GameManager.levelValue--;
			GameObject.Find("levelValue").GetComponent<UILabel>().text=((((float)(GameManager.levelValue))/5)+.8f).ToString();
			}
			break;
		case ButtonType.Return:
			Application.LoadLevel("TetrisInit");
			break;
		case ButtonType.inGameReset:
			Application.LoadLevel("TetrisClone");
			break;
		case ButtonType.inGameExit:
			Application.LoadLevel("TetrisInit");
			break;
		case ButtonType.inGameBack:
			if(GameManager.instance.gameKind=="DynamicM")
				Application.LoadLevel("TetrisInit");
			else
				Application.LoadLevel("TetrisStart");
			break;
		case ButtonType.exportCSV:
			
			ScoreboardScreen.instance.ExportToCSV();
			
			break;
		case ButtonType.deleteDB:
		//	GameObject.Find("Popup"). .SetActive(true);
		//	PlayerPrefs.SetInt("Players", 1);
		//	Application.LoadLevel("TetrisScore");
			break;
			
		case ButtonType.deleteRow:
			GameObject.Find("PopupAll").SetActive(true);
		/*	string valu = (this.gameObject.name).Substring(this.gameObject.name.Length-1,1);
			Destroy(GameObject.Find("item"+valu));
			GameObject.Find("Offset").GetComponent<UITable>().repositionNow=true;
			int val = System.Convert.ToInt16(valu);
			ScoreboardScreen.instance.DeletePlayer(ScoreboardScreen.instance.prefixes[val]);
			ScoreboardScreen.instance.status[val] = PlayerPrefs.GetString(ScoreboardScreen.instance.prefixes[val]+"_status");
			*/
			break;
			
		}
		
	}
	
	
}
