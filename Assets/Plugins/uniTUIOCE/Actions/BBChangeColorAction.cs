using UnityEngine;
using System.Collections;

public class BBChangeColorAction : MonoBehaviour {

	public bool state = false;
	public bool pressToToggle = false;
	public Color onStateColor = Color.green;
	public Color offStateColor = Color.red;
	public Color pressedStateColor = Color.yellow;
	
	void Start()
	{
		if (state) {
			renderer.material.color = onStateColor;
		} else {
			renderer.material.color = offStateColor;
		}
	}

	// Use this for initialization
	public void doTouchDown () {
		renderer.material.color = pressedStateColor;
	}

	public void doTouchUp () {
		if (pressToToggle) state = !state;
		if (state) {
			renderer.material.color = onStateColor;
		} else {
			renderer.material.color = offStateColor;
		}
	}
	
}
