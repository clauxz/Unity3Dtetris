using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public Color blockColor;
	public string[] block;
	public bool playable;
	
	
	//Private variables
	
	private bool[,] blockMatrix;
	private float fallSpeed;
	private int yPosition;
	private	int xPosition;
	private int size;
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
		//halfSize = (size/2);
		//halfSizeFloat = (size*.5);
		
		blockMatrix = new bool[size, size];
		
		for (int y = 0; y < size; y++) {
		for (int x = 0; x < size; x++) {
			if (block[y][x]=="1"[0]) {
				blockMatrix[x, y] = true;
				GameObject blocker =(GameObject) Instantiate(GameManager.instance.node);//Instantiate(GameManager.instance.node,new Vector3((x-13), (size-y)+13-size, 0.0), Quaternion.identity) ;
				blocker.transform.parent = transform;
					blocker.transform.localScale=Vector3.one*15;
				blocker.transform.localPosition= new Vector3(x*13, y*(-13), 0.0f);
				blocker.GetComponent<UISlicedSprite>().color=this.blockColor;
			//	block.renderer.material = material;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
