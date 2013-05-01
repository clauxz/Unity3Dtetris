using UnityEngine;
using System.Collections;

public class BBTurnOffTextureAction : MonoBehaviour {

	public void doTouchUp () 
	{
		if (guiText != null) guiText.enabled = false;
		if (guiTexture != null) guiTexture.enabled = false;
	}
}
