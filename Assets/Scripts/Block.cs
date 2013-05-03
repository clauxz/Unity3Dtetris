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
	private bool dropped = false;
	private Material material;
	
	// Use this for initialization
	void Start () {
		
		size = block.Length;
		
		
		if(size<3)
			return;
		
		var width = block[0].Length;
		
		if(width<3)
			return;
		//halfSize = (size/2);
		halfSize = 18;
		
		blockMatrix = new bool[size, size];
		
		for (int y = 0; y < size ; y++) {
		for (int x = 0; x < size; x++) {
				
			if (block[y][x]=="1"[0]) {
				blockMatrix[x, y] = true;
				GameObject blocker =(GameObject) Instantiate(this.node);//Instantiate(GameManager.instance.node,new Vector3((x-13), (size-y)+13-size, 0.0), Quaternion.identity) ;
				blocker.transform.parent = transform;
					blocker.transform.localScale=Vector3.one*20;
				blocker.transform.localPosition= new Vector3(x*halfSize, y*(-halfSize), 0.0f);
				blocker.GetComponent<UISlicedSprite>().color=this.blockColor;
			
				}
			}
		}
		if(!this.isNextBlock)
		{
			transform.localPosition =new Vector3(-18,165,-1);//(Manager.use.FieldWidth()/2 + (size%2 == 0? 0.0 : .5));
			xPosition = (int)transform.localPosition.x - halfSize;
			yPosition = 165;
		//	xPosition = (int)transform.position.x - halfSize;
		//	yPosition = GameManager.instance.fieldHeight - 1;
			Debug.Log(xPosition + " " + yPosition);
			fallSpeed =(float) GameManager.instance.blockNormalSpeed;
			
		/*	if (GameManager.instance.CheckBlock (blockMatrix, xPosition, yPosition)) {
					//Manager.use.GameOver();
				Debug.Log("Game over");
				return;
				}*/
			this.playable=true;
		//	Fall();
		}
		
		
	}
	
	
/*	void Delay (float time) {
	var t = 0.0;
	//Debug.Log(time);
	while (t <= GameManager.instance.delayTime && !dropped) {
		t += Time.deltaTime;	
	//	yield;
	}
}*/

void Update () {
	if (playable) {
		// Check to see if block would collide if moved down one row
		yPosition--;
	/*	if (Manager.use.CheckBlock (blockMatrix, xPosition, yPosition)) {
			Manager.use.SetBlock (blockMatrix, xPosition, yPosition+1, material);
			Destroy(gameObject);
			break;
		}*/
		
		// Make on-screen block fall down 1 square
		// Also serves as a delay...if you want old-fashioned square-by-square movement, replace this with yield WaitForSeconds
		for (float i = yPosition+1; i > yPosition; i -= Time.deltaTime*fallSpeed) {
			Vector3 val = transform.localPosition;
			transform.localPosition=new Vector3(val.x,(i - halfSize),val.z);
				//transform.position.y = i - halfSize;
		//	yield;
		}
		
		CheckInput();
		
	}
}
	


private void CheckInput () {
	
		
		//var input = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A)) {
			StartCoroutine(MoveHorizontal(-18));
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.W)) {
			StartCoroutine(MoveHorizontal(18));
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)) {
			RotateBlock();
		}
		
		if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S)) {
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
		if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Space))
		{
		//	GameManager.instance.ChangeNextBlock();
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
//	if (!isWallCollide) {
	//	transform.position.x += dir;
		xPosition += dir;
		Vector3 pos = transform.localPosition;
		transform.localPosition=new Vector3(xPosition,pos.y,pos.z);
		
		yield return new WaitForSeconds (1f);
			
/*	}
	else
		{
			isWallCollide=false;
		}*/
	
}


void RotateBlock () {
	// Rotate matrix 90° to the right and store the results in a temporary matrix
	
	// If the rotated block doesn't overlap existing blocks, copy the rotated matrix back and rotate on-screen block to match
//	if (!GameManager.instance.CheckBlock (tempMatrix, xPosition, yPosition)) {
//		System.Array.Copy (tempMatrix, blockMatrix, size*size);
		transform.Rotate (Vector3.forward * -90);
//	}
}
}
