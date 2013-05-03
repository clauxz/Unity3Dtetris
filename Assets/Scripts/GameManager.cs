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
	
	public List<string[]> blocks;
	public List<Color> blocksColors;
	
	public static bool[,] field;
	
	//Tetris Field Size...
	
	public int fieldWidth;
	public int fieldHeight;
	
	public GameObject blockHolder;
	
	public GameObject block;
	
	public GameObject nextBlockObject;
	
	
	//Block Settings...
	//public GameObject node;
	public int maxBlockSize;
	public double blockNormalSpeed;
	public double blockDropSpeed;
	public double blockMoveDelay;
	
	public string gameKind="Manually";
	int totalRowsCleared = 0;

	long score = 0;
	double timeTaken = 0.0;
	
	public double delayTime=1.0f;
	
	public bool gameOver;
	
	private Transform[] cubeReferences;
	private int[] cubePositions;
	private float lastTimeTaken = 0;
	private int nextBlock;
	private int currentBlock;
	private GameObject goNextBlock;
	private bool isNext;
	

	private int rowsCleared = 0;


	
	//Define Types of Blocks here..
	
	
	private void InitiateBlocks()
	{
		blocks = new List<string[]>();
		
		
		string[] block = new string[3];
		
		block[0]="000";
		block[1]="111";
		block[2]="010";
		
		blocks.Add(block);
		
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0110";
		block[2]="0110";
		block[3]="0000";
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="1111";
		block[2]="0000";
		block[3]="0000";
		
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0110";
		block[2]="0100";
		block[3]="0100";
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0010";
		block[2]="0110";
		block[3]="0100";
		
		blocks.Add(block);
		
		block = new string[3];
		
		block[0]="000";
		block[1]="010";
		block[2]="010";
		
		blocks.Add(block);
		
	}
	
	// Use this for initialization
	void Start () {
		
		InitiateBlocks();
		
		
		if(!instance)
			instance=this;
		else
		{
			Debug.LogError("Only one Game Manager can be present");
			return;
		}
		
		//Clearfield(fieldWidth,fieldHeight);
		field=new bool[fieldWidth,fieldHeight];
		//Retrieving game type
	gameKind = PlayerPrefs.GetString("GameKind", "TimePlus");
	
	//Updating speed by start level chosen previously
	var level = PlayerPrefs.GetInt("GameSpeed", 1);
	for(int i = 1; i < level; i++)
	{
		blockNormalSpeed += 0.5f;
		delayTime -= delayTime * 0.4;
	}
		
	for (int i = 0; i < fieldHeight; i++) {
		for (int j = 0; j < maxBlockSize; j++) {
			field[j, i] = true;
			field[fieldWidth-1-j, i] = true;
		}
	}
	for (int i = 0; i < fieldWidth; i++) {
		field[i, 0] = true;
	}

	cubeReferences = new Transform[fieldWidth * fieldHeight];
	cubePositions = new int[fieldWidth * fieldHeight];
	
	//Every time it starts the Manager, gameOver is false
	gameOver = false;
	
	nextBlock = Random.Range(0, blocks.Count);
		
	SpawnBlock();
	}
	
	/*private static void Clearfield(int fieldWidth,int fieldHeight)
	{
		field=new bool[fieldWidth,fieldHeight];
		for(int x=0;x<fieldWidth;x++)
		{
			for(int y =0;y<fieldHeight;y++)
			{
				field[x,y]=false;
			}
		}
	}*/
	
/*	function SetBlock (blockMatrix : boolean[,], xPos : int, yPos : int, mat : Material) {
	var size = blockMatrix.GetLength(0);
	for (y = 0; y < size; y++) {
		for (x = 0; x < size; x++) {	
			if (blockMatrix[x, y]) {
				var c = Instantiate (cube, Vector3((xPos+x)*.1, (yPos-y)*.1, 0.0), Quaternion.identity);
				c.renderer.material = mat;
				field[xPos+x, yPos-y] = true;
			}
		}
	}
	yield CheckRows (yPos - size, size);
	SpawnBlock();
}

void CheckRows (int yStart, int size) {
	//yield;	// Wait a frame for block to be destroyed so we don't include those cubes
	if (yStart < 1) yStart = 1;	// Make sure to start above the floor
	for (int y = yStart; y < yStart+size; y++) {
		for (int x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) { // We don't need to check the walls
			if (!field[x, y]) break;
		}
		// If the loop above completed, then x will equal fieldWidth-maxBlockSize, which means the row was completely filled in
		if (x == fieldWidth-maxBlockSize) {
			CollapseRows (y);
			y--; // We want to check the same row again after the collapse, in case there was more than one row filled in
		}
	}
}*/
	
	public bool CheckBlock (bool[,] blockMatrix,int xPos,int yPos){
	int size = blockMatrix.GetLength(0);
	for (int y = size-1; y >= 0; y--) {
		for (int x = 0; x < size; x++) {
				Debug.Log("x :"+(xPos+x)+" y :"+ (yPos-y) );
			if (blockMatrix[x, y] && field[xPos+x, yPos-y]) {
				return true;
			}
		}
	}
	return false;
	}
	
	void SpawnBlock () {
	//Instantiating new block
	currentBlock = nextBlock;
	GameObject go =(GameObject) Instantiate (block);
	go.transform.parent=blockHolder.transform;
	go.GetComponent<Block>().block=blocks[currentBlock];
	go.GetComponent<Block>().blockColor=blocksColors[currentBlock];
	
	go.GetComponent<Block>().enabled=true;
	
	go.transform.localScale = Vector3.one;
	
	
	//Randoming next block
	nextBlock = Random.Range(0, blocks.Count);
	
	var children = new List<GameObject>();

		foreach (Transform child in nextBlockObject.transform) children.Add(child.gameObject);
			children.ForEach(child => Destroy(child));	
		
	goNextBlock =(GameObject) Instantiate (block);
	goNextBlock.transform.parent=nextBlockObject.transform;
	goNextBlock.GetComponent<Block>().block=blocks[nextBlock];
	goNextBlock.GetComponent<Block>().blockColor=blocksColors[nextBlock];
	goNextBlock.GetComponent<Block>().isNextBlock=true;
	
	goNextBlock.GetComponent<Block>().enabled=true;
	goNextBlock.transform.localPosition = Vector3.zero;
	goNextBlock.transform.localScale = Vector3.one;
}
	// Update is called once per frame
	void Update () {
	
	}
}