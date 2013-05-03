using UnityEngine;
using System.Collections;

public class BlockCollider : MonoBehaviour {

	public bool isStoned;
	
	 void OnTriggerEnter (Collider other ) {
		if(other.gameObject.name=="FloorCollider")
        	this.transform.parent.gameObject.GetComponent<Block>().playable=false;
		else if(other.gameObject.name=="RightCollider"||other.gameObject.name=="LeftCollider")
		{
			this.transform.parent.gameObject.GetComponent<Block>().isWallCollide=true;
		}
		
    }

}
