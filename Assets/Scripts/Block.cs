using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public Color blockColor;
	public string[] block;
	public bool playable;
	public GameObject node;
	public bool isNextBlock;
	public int BlockCurrentPos=0;
	public bool isCollided=false;
	
	
	//Private variables
	
	private bool[,] blockMatrix;
	private float fallSpeed;
	private int yPosition;
	private	int xPosition;
	private int size;
	//private int halfSize;
	private int halfSize;
	private float halfSizeFloat;
	private bool dropped = false;
	private Material material;
	private int rotationCount=0;
	
	// Use this for initialization
	void Start () {
		
		
		
		size = block.Length;
		var width = block[0].Length;
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
	
		
		halfSize = ((size*20)/2);
		halfSizeFloat = ((size*20)/2);
		// int halfSizediff =((size*20)/2);
		blockMatrix = new bool[size, size];
		
		for (int y = 0; y < size ; y++) {
		for (int x = 0; x < size; x++) {
				
			if (block[y][x]=="1"[0]) {
				blockMatrix[x, y] = true;
				GameObject blocker =(GameObject) Instantiate(GameManager.instance.Node);//Instantiate(GameManager.instance.node,new Vector3((x-13), (size-y)+13-size, 0.0), Quaternion.identity) ;
				blocker.transform.parent = transform;
					blocker.transform.localScale=Vector3.one*20;
			//blocker.transform.localPosition= new Vector3((x*20), (y*(-20))+halfSizediff, 0.0f);
				blocker.transform.localPosition= new Vector3((x*20)-halfSizeFloat, (size-(y*20))+halfSizeFloat-size, 0.0f);
				blocker.GetComponent<UISlicedSprite>().color=this.blockColor;
			
				}
			}
		}
		
		if(!this.isNextBlock)
		{
			
			
			
			
			yPosition = (int)((GameManager.instance.fieldHeight*20)-20);
			
			//Debug.Log((xPosition*GameManager.instance.GameScale)+" " + (yPosition*GameManager.instance.GameScale));
			transform.localPosition =new Vector3((float)((((GameManager.instance.fieldWidth)/2)*20 + (size%2 == 0? 0 : 20/2))),(yPosition - halfSizeFloat),-1);
			
			xPosition =(int)(transform.localPosition.x- halfSizeFloat);
			
			fallSpeed =(float) GameManager.instance.blockNormalSpeed;

		if (GameManager.instance.CheckBlock (blockMatrix, (xPosition/20), (yPosition/20))) {
					GameManager.instance.GameOver();
					
				return;
			}
			StartCoroutine(yieldDelay((float)GameManager.instance.delayTime));
			this.playable=true;
	
		}
		
		
	}
	
	
	public IEnumerator Delay (float time) {
	
		yield return new WaitForSeconds(time);
	
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
	
void Update () {
	if (playable) {
		// Check to see if block would collide if moved down one row
		yPosition-=(int)fallSpeed;
	//	int newY= (int)ConvertRange(170,-90,13,0,yPosition);
		if (GameManager.instance.CheckBlock (blockMatrix, (int)(xPosition/20), (int)(yPosition/20))) {
			GameManager.instance.SetBlock (blockMatrix, (xPosition/20), (yPosition+20)/20, this.blockColor);
		
			Destroy(gameObject);
			
			return;

		}
		
				
		// Make on-screen block fall down 1 square
		// Also serves as a delay...if you want old-fashioned square-by-square movement, replace this with yield WaitForSeconds
		for (float i = yPosition+1; i > yPosition; i -= Time.deltaTime) {
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
				//transform.position.y = i - halfSize;
			emptyYield();
		//	yield;
		}
		
		CheckInput();
		emptyYield();
	}
}
	


private void CheckInput () {
	
		
		//var input = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A)) {
			StartCoroutine(yieldMoveHorizontal(-1*20));
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D)) {
			StartCoroutine(yieldMoveHorizontal(1*20));
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)) {
			RotateBlock();
		}
		
		if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.Space)) {
			fallSpeed =(float)GameManager.instance.blockDropSpeed;
			//dropped = true;
			//Manager.use.score += 5;
			//break;	// Break out of while loop, so the coroutine stops (we don't care about input anymore)
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
		if(GameManager.instance.gameKind == "Manually")
		{
			if(Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				GameManager.instance.blockNormalSpeed += .5;
				GameManager.instance.delayTime -= GameManager.instance.delayTime * 0.4;
			}
			if(Input.GetKeyDown(KeyCode.KeypadMinus) && GameManager.instance.blockNormalSpeed > 2.0)
			{
				GameManager.instance.blockNormalSpeed -= .5;
				GameManager.instance.delayTime += GameManager.instance.delayTime * 0.4;
			}
		}
		
	
}
	
public IEnumerator MoveHorizontal (int dir) {
	// Check to see if block could be moved in the desired direction
	if (!GameManager.instance.CheckBlock (blockMatrix, (xPosition + dir)/20, (yPosition/20))) {
	
		Vector3 pos = transform.localPosition;
		transform.localPosition=new Vector3((pos.x+dir),pos.y,pos.z);
	//	transform.position.x += dir;
	//	Debug.Log(xPosition + " " + dir);
		xPosition += dir;
	//	Debug.Log(xPosition + " " + dir);
		
		
		yield return new WaitForSeconds ((float)GameManager.instance.blockMoveDelay);
			
	}
	
	
}
	
	
public IEnumerator yieldMoveHorizontal(int dir)
	{
		StartCoroutine(MoveHorizontal(dir));
		yield return null;
	}


void RotateBlock () {
	
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
			
				this.transform.Rotate(Vector3.forward*-90);
			
			
			
			
	}
}
}
