using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockCollider : MonoBehaviour {

	
	//Real time Colliders To Check whether Block is Collided with Floor Or Wall
	//Collider also Tests For Blocks and ataches on top..
	
	 void OnTriggerEnter (Collider other ) {
		
		
		if(other.gameObject.name=="FloorCollider")//||other.gameObject.name=="BlockCollider")
		{
			
        	this.transform.parent.gameObject.GetComponent<Block>().playable=false;
			
				
			if(!this.transform.parent.gameObject.GetComponent<Block>().isCollided)
			{
				StartCoroutine( GameManager.instance.SpawnBlock());
				
				var children = new List<GameObject>();
				foreach (Transform child in this.transform.parent.gameObject.transform) children.Add(child.gameObject);
					children.ForEach(child => child.gameObject.name="BlockCollider");	
			}
			this.transform.parent.gameObject.GetComponent<Block>().isCollided=true;
				
		}
		
		
		
    }

}
