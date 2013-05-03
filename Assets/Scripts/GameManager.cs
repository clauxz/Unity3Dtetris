using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ButtonType{
	Start=0,
	Tutorial=1,
	Score=2,
	Exit=3,
	Register_ok=4,
	Register_back=5,
	Dynamic=6,
	Manual=7,
	Constant=8,
	Level_inc=9,
	Level_dec=10,
	Return=11
	
}



public class GameManager : MonoBehaviour {
	
	public static GameManager instance;
	
	public static int levelValue=1;
	
	public static bool[,] fieldArray;
	
	//Tetris Field Size...
	
	public int fieldWidth;
	public int fieldHeight;
	
	
	
	
	//Block Settings...
	//public GameObject node;
	public int maxBlockSize;
	public int blockNormalSpeed;
	public int blockDropSpeed;
	public int blockMoveDelay;
	
	//Define Types of Blocks here..
	
	
	
	
	// Use this for initialization
	void Start () {
		
		if(!instance)
			instance=this;
		else
		{
			Debug.LogError("Only one Game Manager can be present");
			return;
		}
		
		Clearfield(fieldWidth,fieldHeight);
		
		
		
		
	}
	
	private static void Clearfield(int fieldWidth,int fieldHeight)
	{
		fieldArray=new bool[fieldWidth,fieldHeight];
		for(int x=0;x<fieldWidth;x++)
		{
			for(int y =0;y<fieldHeight;y++)
			{
				fieldArray[x,y]=false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
