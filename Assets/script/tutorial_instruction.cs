using UnityEngine;
using System.Collections;

public class tutorial_instruction : MonoBehaviour {

	private string[]  instruction= new string [10];
	private int inst_num=0;
	private Transform label_instruct;
	
	// Use this for initialization
	void Start () {
		instruction[0]="swipe around the screen to rotate, press next when you are ready to move on";
		instruction[1]="use your index finger";
		instruction[2]="zoom out";
		instruction[3]="pan";
		instruction[4]="select an error";
		instruction[5]="reset";
		//~ instruction[6]="skip/next";
		
		label_instruct=transform.Find("instruction");
		label_instruct.guiText.text=instruction[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void On_Instruction_Changed(){
		inst_num+=1;
		label_instruct.guiText.text=instruction[inst_num];
	}
	
}
