using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	//Block Colorsettings
	public Color blockColor;
	
	//Block Pattern sves here
	public string[] block;
	
	//Flag to trigger if the block should start falling
	public bool playable;
	
	//if its not our regular Blocks then it would be our Nextblock 
	public bool isNextBlock;
	
	
	
	//Private variables
	//this Defines out Block Matrix it would be define as the pattern
	private bool[,] blockMatrix;
	
	//Block Fall speed it would variate as change level or press drop button
	private float fallSpeed;
	
	//Logical position on screen fo block nd its not the Transform position
	private int yPosition;
	private	int xPosition;
	
	//logical Size of The Block 
	private int size;
	
	//this is for defining center origin so block can rotate move 
	private int halfSize;
	private float halfSizeFloat;
	
	
	
	// Use this for initialization
	void Start () {
		
		size = block.Length;
		var width = block[0].Length;
		
		//Code is optional only test certain limitation of block usage		
		if (size < 2) {
			Debug.LogError ("Blocks must have at least two lines");
		return;
		}
		if (width != size) {
			Debug.LogError ("Block width and height must be the same");
		return;
		}
		if (size > GameManager.instance.maxBlockSize) {
			Debug.LogError ("Blocks must not be larger than " + GameManager.instance.maxBlockSize);
		return;
		}
			for ( int i = 1; i < size; i++) {
				if (block[i].Length != block[i-1].Length) {
					Debug.LogError ("All lines in the block must be the same length");
				return;
				}
		}
		////////////////////
		
		
		//To define center origin of the block
		halfSize = ((size*20)/2);
		//Used Float so not to do alot of Type conversions
		halfSizeFloat = ((size*20)/2);
	
		//Block matrix this would allow checking Collistions
		//Basicaly a predefined Physics.
		blockMatrix = new bool[size, size];
		
		//this is where our Pattern is converted into usable Node positions like checking 1 for Node and 0 for empty space
		//This itertes through block pattern array and checks for 1
		for (int y = 0; y < size ; y++) {
			for (int x = 0; x < size; x++) {
				
				if (block[y][x]=="1"[0]) {
					blockMatrix[x, y] = true;
					GameObject blocker =(GameObject) Instantiate(GameManager.instance.Node);
					blocker.transform.parent = transform;
					blocker.transform.localScale=Vector3.one*20;
					//Node placement as our pattern suggests
					blocker.transform.localPosition= new Vector3((x*20)-halfSizeFloat, (size-(y*20))+halfSizeFloat-size, 0.0f);
					//Node each color is now our block color
					blocker.GetComponent<UISprite>().color=this.blockColor;
			
				}
			}
		}
		
		//Check if its our regular Block or Next BLock
		if(!this.isNextBlock)
		{
			//Logical block position from which Block would start falling
			yPosition = (int)((GameManager.instance.fieldHeight*20)-20);
			
			//Pysical Block location with origin
			transform.localPosition =new Vector3((float)((((GameManager.instance.fieldWidth)/2)*20 + (size%2 == 0? 0 : 20/2))),(yPosition - halfSizeFloat),-1);
			
			//Logical X position 
			xPosition =(int)(transform.localPosition.x- halfSizeFloat);
			
			//our fall speed is equal Block Normal speed so current strting speed would Block normal speed
			fallSpeed =(float) GameManager.instance.blockNormalSpeed;
			
			//This is the logic which would define when the block is collided and its th emain source of whole tetris
			if (GameManager.instance.CheckBlock (blockMatrix, (xPosition/20), (yPosition/20))) {
					//Gameover logic initiates when block is unable to move
				GameManager.instance.GameOver();
					
				return;
			}
			
			
			StartCoroutine( Delay(.5f));
			
		}
		
		
	}
	
	
	public IEnumerator Delay (float time) {
	
		yield return new WaitForSeconds(time);
		this.playable=true;
		StartCoroutine(fall());
		StartCoroutine(CheckInput());
	}
	
	public IEnumerator yieldDelay(float time)
	{
		StartCoroutine(Delay(time));
		yield return null;
	}
		


	
	public IEnumerator emptyYield()
	{
		yield return null;
	}
	
public IEnumerator fall () {
	while (playable&&!(GameManager.instance.isGamePaused)) {
		
		yPosition--;//=(int)fallSpeed;
		
		if (GameManager.instance.CheckBlock (blockMatrix, (int)(xPosition/20), (int)(yPosition/20))) {
			GameManager.instance.SetBlock (blockMatrix, (xPosition/20), (yPosition+20)/20, this.blockColor);
		
			Destroy(gameObject);
			
			break;

		}
		
				
		// Make on-screen block fall down 1 square
		// Also serves as a delay...if you want old-fashioned square-by-square movement, replace this with yield WaitForSeconds
		//Our fall logic Forced
		
			
		for (float i = yPosition+1; i > yPosition; i -= Time.deltaTime*(fallSpeed*30)) {
				Vector3 val = transform.localPosition;
				if(transform.localEulerAngles.z==0)
				{
					transform.localPosition=new Vector3(val.x,(i - halfSize),val.z);
				}
				else if(transform.localEulerAngles.z<=180f)
				{
					transform.localPosition=new Vector3(val.x,(i - halfSize)+20,val.z);
					
				}
				else
				{
					transform.localPosition=new Vector3(val.x,(i - halfSize),val.z);
				}
				yield return null;
			
		}
		
	//	StartCoroutine(CheckInput());
		
	}
}
	

	


public IEnumerator CheckInput () {
	
		while(playable&&!(GameManager.instance.isGamePaused))
		{
		
		if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A)) {
			
			MoveHorizontal(-1*20);
			
			yield return new WaitForSeconds(.2f);
			
		}
		else if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D)) {
			MoveHorizontal(1*20);
		
			yield return new WaitForSeconds(.2f);
		
		}

		if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W)) {
			RotateBlock();
			GameManager.instance.numOfRotations++;
				yield return new WaitForSeconds(.2f);
		}
		
		if (Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.Space)) {
			GameManager.instance.numOfDropPressed++;
			
		}
		
		if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.Space)) {
			fallSpeed =(float)GameManager.instance.blockDropSpeed;
			GameManager.instance.isDropPressed=true;
			
		}
		else
		{
			fallSpeed =(float) GameManager.instance.blockNormalSpeed ;
			
		}
		
		//Change the next block
		if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
		{
			GameManager.instance.ChangeNextBlock();
		}
		
		//Get speed down ou up just in Manual game
		if(GameManager.instance.gameKind == "Manually"||GameManager.instance.gameKind=="DynamicM")
		{

			if(Input.GetKeyDown(KeyCode.Plus)||Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				GameManager.instance.blockNormalSpeed += .2;
				GameManager.instance.delayTime -= GameManager.instance.delayTime * 0.4;
				GameManager.instance.numOfSpeedUp++;
			}
			if(((Input.GetKeyDown(KeyCode.Minus)||Input.GetKeyDown(KeyCode.KeypadMinus))) && GameManager.instance.blockNormalSpeed > 1.0)
			{
				GameManager.instance.blockNormalSpeed -= .2;
				GameManager.instance.delayTime += GameManager.instance.delayTime * 0.4;
				GameManager.instance.numOfSpeedDown++;
			}
		}
			yield return null;
		}
		
	
}
	
public void MoveHorizontal (int dir) {
	// Check to see if block could be moved in the desired direction
		
			if (!GameManager.instance.CheckBlock (blockMatrix, (xPosition + dir)/20, (yPosition/20))) {
		
				Vector3 pos = transform.localPosition;
				transform.localPosition=new Vector3((pos.x+dir),pos.y,pos.z);

				xPosition += dir;
				
			}
		
}
	
	

void RotateBlock () {
	
		//This codesmakes the rotation fo logical block
		var tempMatrix = new bool[size, size];
	for (int y = 0; y < size; y++) {
		for (int x = 0; x < size; x++) {
			tempMatrix[y, x] = blockMatrix[x, (size-1)-y];
		}
	}
	
	// If the rotated block doesn't overlap existing blocks, copy the rotated matrix back and rotate on-screen block to match
	if (!GameManager.instance.CheckBlock (tempMatrix, (xPosition/20), (yPosition/20))) {
		System.Array.Copy (tempMatrix, blockMatrix, size*size);
			
				Vector3 pos= this.transform.localPosition;
		
			if(transform.eulerAngles.z==180f)
			{
			
				this.transform.localPosition= new Vector3(pos.x+20,pos.y+20,pos.z);
				
			}
			else if(this.transform.localRotation.z==0f)
			{
			
				this.transform.localPosition= new Vector3(pos.x-20,pos.y,pos.z);
				
			}
			//This line makes the Block rotate Physicaly...
				this.transform.Rotate(Vector3.forward*-90);
			
			
			
			
	}
}
}
