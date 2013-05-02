var _fieldWidth = 5;
var _fieldHeight = 13;
var maxBlockSize = 5;
var blockNormalSpeed = 2.0;
var blockDropSpeed = 30.0;
var blockMoveDelay = 0.1f;
//var rowsClearedToSpeedup = 1;
//var speedupAmount = 5;
var blocks : GameObject[];
var materials : Material[];
var cube : Transform;
var brickHolder : Transform;
//var leftWall : Transform;
//var rightWall : Transform;

/*sergio's variables*/
var delayTime = 1.0;
var totalRowsCleared = 0;
var gameKind = "Manually";
var score = 0;
var timeTaken = 0.0;
var skin : GUISkin;
private var lastTimeTaken = 0;
private var nextBlock : int;
private var currentBlock : int;
private var goNextBlock : GameObject;
private var isNext : boolean;

private var fieldWidth : int;
private var fieldHeight : int;
private var field : boolean[,];
private var cubeReferences : Transform[];
private var cubePositions : int[];
private var rowsCleared = 0;
private var gameOver = false;
static var use : Manager;

function Start () {
	//PlayerPrefs.SetInt("Players", 1);
	if (!use) {
		use = this;	// Get a reference to this script, which is a static variable so it's used as a singleton
	}
	else {
		Debug.LogError ("Only one instance of this script is allowed");
		return;
	}
	
	//Retrieving game type
	gameKind = PlayerPrefs.GetString("GameKind", "TimePlus");
	
	//Updating speed by start level chosen previously
	var level = PlayerPrefs.GetInt("GameSpeed", 1);
	for(i = 1; i < level; i++)
	{
		blockNormalSpeed += 0.5;
		delayTime -= delayTime * 0.4;
	}

	// Make the "real" width/height larger, to account for borders and space at the top for initial block placement
	fieldWidth = _fieldWidth + maxBlockSize*2;
	fieldHeight = _fieldHeight + maxBlockSize;
	field = new boolean[fieldWidth, fieldHeight];
	
	// Make the "walls" and "floor" in the array...we use true = block, and false = open space
	// This way we don't need special logic to deal with the bottom or edges of the playing field,
	// since blocks will collide with the walls/floor the same as with other blocks
	// Also, we use 0 = bottom and fieldHeight-1 = top, so that positions in the array match positions in 3D space
	for (i = 0; i < fieldHeight; i++) {
		for (j = 0; j < maxBlockSize; j++) {
			field[j, i] = true;
			field[fieldWidth-1-j, i] = true;
		}
	}
	for (i = 0; i < fieldWidth; i++) {
		field[i, 0] = true;
	}
	
	// Position stuff in the scene so it looks right regardless of what sizes are entered for the playing field
	// (Though the camera would have to be moved back for larger sizes)
	//leftWall.position.x = maxBlockSize-.5;
	//rightWall.position.x = fieldWidth-maxBlockSize+.5;
//	Camera.main.transform.position = Vector3(fieldWidth/2, fieldHeight/2, -16.0);
	
	cubeReferences = new Transform[fieldWidth * fieldHeight];
	cubePositions = new int[fieldWidth * fieldHeight];
	
	//Every time it starts the Manager, gameOver is false
	gameOver = false;
	
	nextBlock = Random.Range(0, blocks.Length);
	
	SpawnBlock();
}

function Update(){
	if(!gameOver)
	{
		timeTaken += Time.deltaTime;
		iTimeTaken = Mathf.Ceil(timeTaken);
		if(gameKind == "TimePlus" && iTimeTaken%10 == 0 && lastTimeTaken != iTimeTaken)
		{
			lastTimeTaken = iTimeTaken;
			score += 2;
		}
	}
}

function SpawnBlock () {
	//Instantiating new block
	currentBlock = nextBlock;
	var go = Instantiate (blocks[currentBlock]);
	//go.transform.parent=brickHolder;
	go.transform.localScale = Vector3.one;
	go.transform.position.x =0;// Vector3.zero;
	go.GetComponent("Block").SetMaterial(materials[currentBlock]);
	
	//Randoming next block
	nextBlock = Random.Range(0, blocks.Length);
	Destroy(goNextBlock);
	goNextBlock = Instantiate (blocks[nextBlock]);
	goNextBlock.GetComponent("Block").SetMaterial(materials[nextBlock]);
	goNextBlock.GetComponent("Block").playable = false;
	goNextBlock.transform.position.x = -8.5;
	goNextBlock.transform.position.y = 7.5;
}

function ChangeNextBlock () {
	//Randoming a new next block
	nextBlock = Random.Range(0, blocks.Length);
	Destroy(goNextBlock);
	goNextBlock = Instantiate (blocks[nextBlock]);
	goNextBlock.GetComponent("Block").SetMaterial(materials[nextBlock]);
	goNextBlock.GetComponent("Block").playable = false;
	goNextBlock.transform.position.x = -8.5;
	goNextBlock.transform.position.y = 7.5;
}

function FieldHeight () : int {
	return fieldHeight;
}

function FieldWidth () : int {
	return fieldWidth;
}

// See if the block matrix would overlap existing blocks in the playing field
// (Check from bottom-up, since in general gameplay usage it's a bit more efficient that way)
function CheckBlock (blockMatrix : boolean[,], xPos : int, yPos : int) : boolean {
	var size = blockMatrix.GetLength(0);
	for (y = size-1; y >= 0; y--) {
		for (x = 0; x < size; x++) {
			if (blockMatrix[x, y] && field[xPos+x, yPos-y]) {
				return true;
			}
		}
	}
	return false;
}

// Make on-screen cubes from position in array when the block is stopped from falling any more
// Just using DetachChildren isn't feasible because the child cubes can be in different orientations,
// which can mess up their position on the Y axis, which we need to be consistent in CollapseRow
// Also write the block matrix into the corresponding location in the playing field
function SetBlock (blockMatrix : boolean[,], xPos : int, yPos : int, mat : Material) {
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

function CheckRows (yStart : int, size : int) {
	yield;	// Wait a frame for block to be destroyed so we don't include those cubes
	if (yStart < 1) yStart = 1;	// Make sure to start above the floor
	for (y = yStart; y < yStart+size; y++) {
		for (x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) { // We don't need to check the walls
			if (!field[x, y]) break;
		}
		// If the loop above completed, then x will equal fieldWidth-maxBlockSize, which means the row was completely filled in
		if (x == fieldWidth-maxBlockSize) {
			yield CollapseRows (y);
			y--; // We want to check the same row again after the collapse, in case there was more than one row filled in
		}
	}
}

function CollapseRows (yStart : int) {
	// Move rows down in array, which effectively deletes the current row (yStart)
	for (y = yStart; y < fieldHeight-1; y++) {
		for (x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) {
			field[x, y] = field[x, y+1];
		}
	}
	// Make sure top line is cleared
	for (x = maxBlockSize; x < fieldWidth-maxBlockSize; x++) {
		field[x, fieldHeight-1] = false;
	}
	
	// Destroy on-screen cubes on the deleted row, and store references to cubes that are above it
	var cubes = gameObject.FindGameObjectsWithTag("Cube");
	var cubesToMove = 0;
	for (cube in cubes) {
		if (cube.transform.position.y > yStart) {
			cubePositions[cubesToMove] = cube.transform.position.y;
			cubeReferences[cubesToMove++] = cube.transform;
		}
		else if (cube.transform.position.y == yStart) {
			Destroy(cube);
		}
	}
	// Move the appropriate cubes down one square
	// The third parameter in Mathf.Lerp is clamped to 1.0, which makes the transform.position.y be positioned exactly when done,
	// which is important for the game logic (see the code just above)
	var t = 0.0;
	while (t <= 1.0) {
		t += Time.deltaTime * 5.0;
		for (i = 0; i < cubesToMove; i++) {
			cubeReferences[i].position.y = Mathf.Lerp (cubePositions[i], cubePositions[i]-1, t);
		}
		yield;
	}
	
	// Make blocks drop faster when enough rows are cleared in TimePlus game
	if(gameKind == "TimePlus")
	{
		blockNormalSpeed += .2;
		delayTime -= delayTime * 0.08;
	}
	totalRowsCleared++;
	score += 10;
}

function GameOver () {
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
		Debug.Log(mess);
		hasSaved = true;
	}
	gameOver = true;
}

// Prints the state of the field array, for debugging
function PrintField () {
	var fieldChars = "";
	for (y = fieldHeight-1; y >= 0; y--) {
		for (x = 0; x < fieldWidth; x++) {
			fieldChars += field[x, y]? "1" : "0";
		}
		fieldChars += "\n";
	}
	Debug.Log (fieldChars);
}

/*function OnGUI()
{
	//var debugstring = "gameSpeed = " + blockNormalSpeed.ToString() + "\n";
	//GUI.Label(Rect(0,Screen.height *0.7, 100, 200),"DEBUG: " + debugstring);
	//Next Block Dialog
	GUI.Label(Rect(Screen.width*0.76, Screen.height *0.415, 230, 200), "Next:", skin.GetStyle("gameplay2"));
		
	//Must show time just in TimePlus and Tutorial game	
	if(gameKind == "TimePlus" || gameKind == "Tutorial" )
	{
		var time = String.Format("Time:\n{0:F1}", timeTaken);
		GUI.Label(Rect(Screen.width*0.05, Screen.height *0.065, 200, 100), time, skin.GetStyle("gameplay"));	
	}
	
	//shows Score
	GUI.Label(Rect(Screen.width*0.77, Screen.height *0.065, 200, 100), "Score:\n" + score.ToString(), skin.GetStyle("gameplay"));	
	//shows Destroyed Row
	GUI.Label(Rect(Screen.width*0.77, Screen.height *0.215, 200, 100), "Destroyed Row:\n" + totalRowsCleared.ToString(), skin.GetStyle("gameplay"));
	//shows Game Speed
	GUI.Label(Rect(Screen.width*0.05, Screen.height *0.215, 200, 100), "Game Speed:\n" + String.Format("Time:\n{0:F1}", blockNormalSpeed), skin.GetStyle("gameplay"));	

	//Reset Button
	if(GUI.Button(Rect(Screen.width*0.05, Screen.height *0.415, 200, 100), "Reset",skin.GetStyle("button")))
	{
		Application.LoadLevel("TetrisClone");
	}
	//If it isn't tutorial, it shows Return button
	if(gameKind != "Tutorial")
	{
		if(GUI.Button(Rect(Screen.width*0.05, Screen.height *0.565, 200, 100), "Return",skin.GetStyle("button")))
		{
			Application.LoadLevel("TetrisStart");
		}
	}
	//Exit Button
	if(GUI.Button(Rect(Screen.width*0.05, Screen.height *0.715, 200, 100), "Exit",skin.GetStyle("button")))
	{
		Application.LoadLevel("TetrisInit");
	}
	
	//Shows game over as long as needed 
	if(gameOver)
	{
		GUI.Label(Rect(Screen.width*0.4, Screen.height *0.5, 150, 25), "GAME OVER", skin.GetStyle("gameover"));	
	}
}*/
