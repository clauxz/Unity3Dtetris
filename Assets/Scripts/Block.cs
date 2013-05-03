using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public Color blockColor;
	public string[] block;
	public bool playable;
	public GameObject node;
	public bool isNextBlock;
	
	
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
		
		for (int y = 0; y < size; y++) {
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
			transform.localPosition =new Vector3(-13,165,-1);//(Manager.use.FieldWidth()/2 + (size%2 == 0? 0.0 : .5));
			xPosition = (int)transform.localPosition.x - halfSize;
			yPosition = 165;
			//transform.position.y = yPosition;// - halfSize;
			fallSpeed = GameManager.instance.blockNormalSpeed;
			
			if (GameManager.instance.CheckBlock (blockMatrix, xPosition, yPosition)) {
					//Manager.use.GameOver();
				Debug.Log("Game over");
				return;
				}
		}
		
		
	}
	
/*	void Delay (float time) {
	var t = 0.0;
	//Debug.Log(time);
	while (t <= GameManager.instance.delayTime && !dropped) {
		t += Time.deltaTime;	
	//	yield;
	}
}

void Fall () {
	while (playable) {
		// Check to see if block would collide if moved down one row
		yPosition--;
		if (Manager.use.CheckBlock (blockMatrix, xPosition, yPosition)) {
			Manager.use.SetBlock (blockMatrix, xPosition, yPosition+1, material);
			Destroy(gameObject);
			break;
		}
		
		// Make on-screen block fall down 1 square
		// Also serves as a delay...if you want old-fashioned square-by-square movement, replace this with yield WaitForSeconds
		for (float i = yPosition+1; i > yPosition; i -= Time.deltaTime*fallSpeed) {
			transform.position.y = i - halfSizeFloat;
		//	yield;
		}
	}
}*/
}
