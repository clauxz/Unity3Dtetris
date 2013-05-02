using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public Color blockColor;
	public string[] blocks;
	public bool playable;
	
	
	//Private variables
	
	private bool[,] blockmatrix;
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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
