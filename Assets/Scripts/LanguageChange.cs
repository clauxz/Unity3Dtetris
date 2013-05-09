using UnityEngine;
using System.Collections;

public class LanguageChange : MonoBehaviour {
	
	public LanguageType lang;
	
	public string labelType;
	
	public void ChangeLanguage()
	{
		if(lang==LanguageType.English)
		{
			this.GetComponent<UISlicedSprite>().spriteName=labelType + "Eng";
		}
		else if(lang==LanguageType.Japanese)
		{
			this.GetComponent<UISlicedSprite>().spriteName=labelType + "Jpn";
			
		}
	}
	
}
