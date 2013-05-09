using UnityEngine;
using System.Collections;

public class SelectLanguage : MonoBehaviour {
	
	private LanguageType lang;
	
	public GameObject LanguageUI;
	
	// Use this for initialization
	void Start () {
		if(this.GetComponent<UIPopupList>())
		{
			this.GetComponent<UIPopupList>().eventReceiver=this.gameObject;
			this.GetComponent<UIPopupList>().selection=PlayerPrefs.GetString("language", "English");
		}
		
		
		string language = PlayerPrefs.GetString("language", "English");
		
		if(language=="English")
		{
			lang=LanguageType.English;
		}
		else
		{
			lang=LanguageType.Japanese;
		}
		
		LanguageChange[] targetObjs = LanguageUI.GetComponentsInChildren<LanguageChange>();
		
		foreach(LanguageChange lc in targetObjs)
		{
			lc.lang=lang;
			lc.ChangeLanguage();
		}
		
		
	}
	
	void OnSelectionChange()
	{
		PlayerPrefs.SetString("language", this.GetComponent<UIPopupList>().selection);
		
		string language = this.GetComponent<UIPopupList>().selection;
		
		if(language==null||language==""||language=="English")
		{
			lang=LanguageType.English;
		}
		else
		{
			lang=LanguageType.Japanese;
		}
		
		LanguageChange[] targetObjs = LanguageUI.GetComponentsInChildren<LanguageChange>();
		
		foreach(LanguageChange lc in targetObjs)
		{
			lc.lang=lang;
			lc.ChangeLanguage();
		}
		
	}
}
