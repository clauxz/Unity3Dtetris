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
	
	// Use this for initialization
	void Start () {
		
		
		
		size = block.Length;
		
		
		if(size<3)
			return;
		
		var width = block[0].Length;
		
		if(width<3)
			return;
		
		halfSize = size/2;
		halfSizeFloat = size/2;
		
		blockMatrix = new bool[size, size];
		
		for (int y = 0; y < size ; y++) {
		for (int x = 0; x < size; x++) {
				
			if (block[y][x]=="1"[0]) {
				blockMatrix[x, y] = true;
				GameObject blocker =(GameObject) Instantiate(GameManager.instance.Node);//Instantiate(GameManager.instance.node,new Vector3((x-13), (size-y)+13-size, 0.0), Quaternion.identity) ;
				blocker.transform.parent = transform;
					blocker.transform.localScale=Vector3.one*20;
			
				blocker.transform.localPosition= new Vector3((x*18)-halfSizeFloat, (size-(y*18))+halfSizeFloat-size, 0.0f);
				blocker.GetComponent<UISlicedSprite>().color=this.blockColor;
			
				}
			}
		}
		
		if(!this.isNextBlock)
		{
			
			
			
			xPosition =(int)((GameManager.instance.fieldWidth)/2 + (size%2 == 0? 0.0 : 0.5));
			yPosition = (int)((GameManager.instance.fieldHeight) - 1);
			transform.localPosition =new Vector3(xPosition,(yPosition - halfSizeFloat),-1);
			
			fallSpeed =(float) GameManager.instance.blockNormalSpeed;
			
			
	/*		float xVal = (float)(GameManager.instance.fieldWidth/2 + (size%2 == 0? 0.0 : .5));
			transform.localPosition =new Vector3(xVal,140,-1);//(Manager.use.FieldWidth()/2 + (size%2 == 0? 0.0 : .5));
			xPosition = (int)transform.localPosition.x;
			yPosition = (int)transform.localPosition.y;
		//	Debug.Log(transform.localPosition.x+" "+transform.localPosition.y);
		//	xPosition = (int)transform.position.x - halfSize;
		//	yPosition = GameManager.instance.fieldHeight - 1;
		//	Debug.Log(xPosition + " " + yPosition);
		//	int newY= (int)ConvertRange(130,-90,0,13,yPosition);*/
		if (GameManager.instance.CheckBlock (blockMatrix, xPosition, yPosition)) {
					//Manager.use.GameOver();
				Debug.Log("Game over");
				return;
			}
			StartCoroutine(yieldDelay((float)GameManager.instance.delayTime));
			this.playable=true;
		//	Fall();
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
		

public int ConvertRange(
    int originalStart, int originalEnd, // original range
    int newStart, int newEnd, // desired range
    int value) // value to convert
{
    double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
    return (int)(newStart + ((value - originalStart) * scale));
}	
	
	public IEnumerator emptyYield()
	{
		yield return null;
	}
	
void Update () {
	if (playable) {
		// Check to see if block would collide if moved down one row
		yPosition--;
	//	int newY= (int)ConvertRange(170,-90,13,0,yPosition);
		if (GameManager.instance.CheckBlock (blockMatrix, xPosition, yPosition)) {
	//		Manager.use.SetBlock (blockMatrix, xPosition, yPosition+1, material);
			
			Destroy(gameObject);
			
			return;
//			break;
		}
		//float valu = yPosition*18;
		// Make on-screen block fall down 1 square
		// Also serves as a delay...if you want old-fashioned square-by-square movement, replace this with yield WaitForSeconds
		for (float i = yPosition+1; i > yPosition; i -= Time.deltaTime*fallSpeed) {
			Vector3 val = transform.localPosition;
			transform.localPosition=new Vector3(val.x,(i - halfSize),val.z);
				//transform.position.y = i - halfSize;
		//	yield;
		}
		
		CheckInput();
		emptyYield();
	}
}
	


private void CheckInput () {
	
		
		//var input = Input.GetAxis("Horizontal");
		if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A)) {
			StartCoroutine(yieldMoveHorizontal(-18));
		}
		else if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D)) {
			StartCoroutine(yieldMoveHorizontal(18));
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
	//if (!GameManager.instance.CheckBlock (blockMatrix, xPosition + dir, yPosition)) {
	//	transform.position.x += dir;
		xPosition += dir;
		Vector3 pos = transform.localPosition;
		transform.localPosition=new Vector3(xPosition,pos.y,pos.z);
		
		yield return new WaitForSeconds ((float)GameManager.instance.blockMoveDelay);
			
//	}
	
	
}
	
	
public IEnumerator yieldMoveHorizontal(int dir)
	{
		StartCoroutine(MoveHorizontal(dir));
		yield return null;
	}


void RotateBlock () {
	
		transform.Rotate (Vector3.forward * -90);
	
}
}
