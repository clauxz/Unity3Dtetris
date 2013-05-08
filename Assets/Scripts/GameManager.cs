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
	Return=11,
	inGameReset=12,
	inGameExit=13,
	inGameBack=14,
	exportCSV=15,
	deleteDB=16,
	deleteRow=17
	
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
	public AudioClip blockDrop;
	public AudioClip rowDestroyed;
	
	public GameObject Node;
	
	public int fieldWidth;
	public int fieldHeight;
	
	public GameObject blockHolder;
	
	public GameObject block;
	
	public GameObject nextBlockObject;
	public GameObject gameOverLabel;
	public GameObject scoreLabel;
	public GameObject timeLabel;
	public GameObject destrowLabel;
	public GameObject gameSpeedLabel;
	
	//public static bool isOnceSpawn=false;
	//Block Settings...
	//public GameObject node;
	public int maxBlockSize;
	public double blockNormalSpeed;
	public double blockDropSpeed;
	public double blockMoveDelay;
	
	public string gameKind="Manually";
	int totalRowsCleared = 0;

	int score = 0;
	float timeTaken = 0f;
	float iTimeTaken=0f;
	
	public double delayTime=1.0f;
	
	public bool gameOver;
	public bool hasSaved;
	
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
		
		
		string[] block;
		
		block = new string[3];
		
		block[0]="011";
		block[1]="010";
		block[2]="010";
		
		blocks.Add(block);
		
		block = new string[3];
		
		block[0]="010";
		block[1]="110";
		block[2]="100";
		
		blocks.Add(block);
		
		block = new string[2];
		
		
		block[0]="11";
		block[1]="11";
		
		blocks.Add(block);
		
			block = new string[3];
		
		block[0]="010";
		block[1]="011";
		block[2]="001";
		
		blocks.Add(block);
		
		block = new string[3];
		block[0]="000";
		block[1]="111";
		block[2]="010";
		
		blocks.Add(block);
		
		block = new string[3];
		
		block[0]="110";
		block[1]="010";
		block[2]="010";
		
		blocks.Add(block);
		
		
		block = new string[4];
		
		block[0]="0000";
		block[1]="1111";
		block[2]="0000";
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
		if(!gameOver)
		{
		timeTaken += Time.deltaTime;
		iTimeTaken = Mathf.Ceil(timeTaken);
		if(gameKind == "TimePlus" && iTimeTaken%10 == 0 && lastTimeTaken != iTimeTaken)
		{
			lastTimeTaken = iTimeTaken;
			score += 2;
		}
			destrowLabel.GetComponent<UILabel>().text=totalRowsCleared.ToString();
			scoreLabel.GetComponent<UILabel>().text=score.ToString();
			timeLabel.GetComponent<UILabel>().text=Mathf.RoundToInt(timeTaken).ToString();
			gameSpeedLabel.GetComponent<UILabel>().text=this.blockNormalSpeed.ToString();
		}
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
//		audio.clip=blockDrop;
			//audio.Play();
	}
		CheckRows (yPos - size, size);
	//StartCoroutine(yieldCheckRows(yPos - size, size));
	//yield CheckRows (yPos - size, size);
		score ++;
	StartCoroutine(SpawnBlock());
	}
	
	public void CheckRows (int yStart,int size) {
		StartCoroutine(emptyYield());	// Wait a frame for block to be destroyed so we don't include those cubes
	if (yStart < 1) yStart = 1;
		
	//int y=0;
//	int x=0;// Make sure to start above the floor
	for (int y = yStart; y < yStart+size; y++) {
		for (int x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) { // We don't need to check the walls
			if (!field[x, y]) break;
			Debug.Log(x+ " = " +(fieldWidth-maxBlockSize));
			if ((x+1) == fieldWidth-maxBlockSize) {
				Debug.Log("Row Completed");
		//	StartCoroutine(emptyYield());
			CollapseRows (y);
				
			y--; // We want to check the same row again after the collapse, in case there was more than one row filled in
			}
		}
//			Debug.Log(x+ " = " +(fieldWidth-maxBlockSize));
		// If the loop above completed, then x will equal fieldWidth-maxBlockSize, which means the row was completely filled in
		
	}
}
	
	public void CollapseRows (int yStart) {
	// Move rows down in array, which effectively deletes the current row (yStart)
		
	for (int y = yStart; y < fieldHeight-1; y++) {
		for (int x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) {
			field[x, y] = field[x, y+1];
		}
	}
	// Make sure top line is cleared
	for (int x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) {
		field[x, fieldHeight-1] = false;
	}
	
	// Destroy on-screen cubes on the deleted row, and store references to cubes that are above it
	GameObject[] cubes =(GameObject.FindGameObjectsWithTag("Cube")) ;
	
	int cubesToMove = 0;
	foreach (GameObject cube in cubes) {
		
		if (cube.transform.localPosition.y > ((yStart*20))) {
			cubePositions[cubesToMove] =(int) cube.transform.localPosition.y;
			cubeReferences[cubesToMove++] = cube.transform;
		}
		else if (cube.transform.localPosition.y == ((yStart*20))) {
			
			Destroy(cube);
		}
	}
		
	// Move the appropriate cubes down one square
	// The third parameter in Mathf.Lerp is clamped to 1.0, which makes the transform.position.y be positioned exactly when done,
	// which is important for the game logic (see the code just above)
	var t = 0.0;
	while (t <= 1.0) {
		t += Time.deltaTime * 5.0;
		for (int i = 0; i < cubesToMove; i++) {
				Vector3 sVal = cubeReferences[i].localPosition;
			cubeReferences[i].localPosition =new Vector3(sVal.x,
					Mathf.Lerp ((float)cubePositions[i], (float)(cubePositions[i]-20), (float)t),
					sVal.z);
		}
		StartCoroutine(emptyYield());
	}
	
	// Make blocks drop faster when enough rows are cleared in TimePlus game
	if(gameKind == "TimePlus")
	{
		blockNormalSpeed += .2;
		delayTime -= delayTime * 0.08;
	}
	totalRowsCleared++;
	score += 10;
	//	audio.clip=rowDestroyed;
	//		audio.Play();
}
	
	public IEnumerator emptyYield()
	{
		yield return null;
	}
	
	public IEnumerator yieldCheckRows(int yPos,int size)
	{
		emptyYield();
		CheckRows (yPos - size, size);
		yield return null;
	}
	
	public void GameOver () {
	//Debug.Log ("Game Over!");
	
	//save in database if game ins't tutorial
	if(gameKind != "Tutorial" && !gameOver)
	{
		var currentId = PlayerPrefs.GetInt("Players", 1);
		var curPlayer = "Player" + currentId.ToString();
		
		var n = PlayerPrefs.GetString("CurrentPlayerName");
		PlayerPrefs.SetString(curPlayer + "_name", n);
		PlayerPrefs.SetInt(curPlayer + "_score", score);
		PlayerPrefs.SetInt(curPlayer + "_rows", totalRowsCleared);
		
		if(gameKind == "TimePlus"){
			PlayerPrefs.SetInt(curPlayer + "_level", totalRowsCleared);	
		}
		else if(gameKind == "Constant"){
			PlayerPrefs.SetInt(curPlayer + "_level", PlayerPrefs.GetInt("GameSpeed", 1));
		}
		else if(gameKind == "Manually"){
			PlayerPrefs.SetInt(curPlayer + "_level", 0);	
		}
		
		PlayerPrefs.SetFloat(curPlayer + "_timetaken", timeTaken);
		PlayerPrefs.SetString(curPlayer + "_game", gameKind);
		PlayerPrefs.SetString(curPlayer + "_status", "active");
		PlayerPrefs.SetString(curPlayer + "_prefix", curPlayer);
				
		PlayerPrefs.SetInt("Players", ++currentId);		
		
		var mess = "CurrPlayer: " + curPlayer + "\n" +
		curPlayer + "_name: " + n + "\n" +
		curPlayer + "_score: " + score + "\n" +
		curPlayer + "_level: " + totalRowsCleared + "\n" +
		curPlayer + "_rows: " + totalRowsCleared + "\n" +
		curPlayer + "_timetaken: " + timeTaken + "\n" +
		curPlayer + "_game: " + gameKind;
		//Debug.Log(mess);
		hasSaved = true;
	}
	gameOver = true;
	gameOverLabel.GetComponent<UILabel>().enabled=true;
}
	
}
