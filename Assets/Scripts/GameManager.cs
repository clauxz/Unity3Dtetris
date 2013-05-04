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
	
	public int GameScale;
	public int framePosX;
	public int framePosY;
	//Tetris Field Size...
	
	
	public GameObject Node;
	
	public int fieldWidth;
	public int fieldHeight;
	
	public GameObject blockHolder;
	
	public GameObject block;
	
	public GameObject nextBlockObject;
	public GameObject leftWall;
	public GameObject rightWall;
	public GameObject holderBack;
	
	//public static bool isOnceSpawn=false;
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
		
		
		string[] block = new string[4];
		
		block[0]="0000";
		block[1]="0000";
		block[2]="1110";
		block[3]="0100";
		
		blocks.Add(block);
		
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0000";
		block[2]="1100";
		block[3]="1100";
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0000";
		block[2]="1111";
		block[3]="0000";
		
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="1100";
		block[2]="1000";
		block[3]="1000";
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0100";
		block[2]="1100";
		block[3]="1000";
		
		blocks.Add(block);
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="0000";
		block[2]="1100";
		block[3]="0000";
		
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
		fieldWidth = fieldWidth + maxBlockSize*2;
		fieldHeight = fieldHeight + maxBlockSize;
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
	
//	leftWall.transform.localPosition =new Vector3( (float)(maxBlockSize-.5),0,-1);
//	rightWall.transform.localPosition =new Vector3( (float)(fieldWidth-maxBlockSize+.5),0,-1);
	//	holderBack.transform.localScale=new Vector3( (float)((fieldWidth)*GameScale),(fieldHeight*GameScale),1);
	/*for(int x=0;x<fieldWidth;x++)
			for(int y =0;y<fieldHeight;y++)
				Debug.Log(field[x,y]);*/
		
	cubeReferences = new Transform[fieldWidth * fieldHeight];
	cubePositions = new int[fieldWidth * fieldHeight];
	
	//Every time it starts the Manager, gameOver is false
	gameOver = false;
	
	nextBlock = Random.Range(0, blocks.Count);
		
	StartCoroutine(SpawnBlock());
	}
	
	
	
	public bool CheckBlock (bool[,] blockMatrix,int xPos,int yPos){
	//	Debug.Log(xPos+"  " +yPos);
	//	try{
	int size = (blockMatrix.GetLength(0));
	
	for (int y = size-1; y >= 0; y--) {
		for (int x = 0; x < size; x++) {
				if (blockMatrix[x, y] && field[xPos+x, yPos-y]) {
				return true;
			}
			
		}
				
	}
			
	//	}catch(System.Exception e){}
	return false;
	}
	
	
	
	public IEnumerator SpawnBlock () {
	//Instantiating new block
	currentBlock = nextBlock;
	GameObject go =(GameObject) Instantiate (block);
	go.transform.parent=blockHolder.transform;
	go.GetComponent<Block>().block=blocks[currentBlock];
	go.GetComponent<Block>().blockColor=blocksColors[currentBlock];
	
	go.GetComponent<Block>().enabled=true;
	
	go.transform.localScale = Vector3.one;
	
	
	//Randoming next block
	ChangeNextBlock();
	yield return new WaitForSeconds (1f);
}
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ChangeNextBlock () {
	//Randoming a new next block
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
	goNextBlock.transform.localPosition = new Vector3(0,0,-10);
	goNextBlock.transform.localScale = Vector3.one;
	}
	
	public void SetBlock (bool[,] blockMatrix,int xPos, int yPos,Color col) {
	var size = blockMatrix.GetLength(0);
	for ( int y = 0; y < size; y++) {
		for ( int x = 0; x < size; x++) {	
			if (blockMatrix[x, y]) {
				GameObject c =(GameObject) Instantiate (Node);
				c.transform.parent=blockHolder.transform;
				c.transform.localPosition=new Vector3((xPos+x)*20, (yPos-y)*20, -1);
				c.transform.localScale=new Vector3(20,20,1);
				c.GetComponent<UISlicedSprite>().color=col;
				field[xPos+x, yPos-y] = true;
			}
		}
	}
	//yield CheckRows (yPos - size, size);
	StartCoroutine(SpawnBlock());
	}
}
