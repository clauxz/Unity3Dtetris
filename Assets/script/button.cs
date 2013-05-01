using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {

	Color custom_color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown(){
		transform.guiText.material.color=Color.red;
	}
	
	void OnMouseUp(){
		transform.guiText.material.color=Color.green;
	}
	
	void OnMouseEnter(){
		transform.guiText.material.color=Color.green;
	}
	
	void OnMouseExit(){
		transform.guiText.material.color=custom_color;
	}
	
	void SetColour(Color temp){
		custom_color=temp;
	}
}
