using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

[ExecuteInEditMode]
public class GroundCheck : MonoBehaviour {
	//Dette script tjekker om objektet rører noget med et tag kaldet "Ground"
	//Scriptet sættes på empty object med trigger collider under player figur (så det tjekker om player står på noget Ground)

	//Fortæller om figuren er grounded
	public bool grounded;
	public bool resetCollider = false;


	// Use this for initialization
	void Start () 
	{
		//figur sættes fra start til ikke at være grounded
		grounded = false;

#if UNITY_EDITOR
		DestroyImmediate (GetComponent<SpriteRenderer> ());
		StartInfo ();
#endif
	}


	//Når Groound-objekt forlader triggerzone gøres figur ikke-grounded
	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Ground")
		{
			grounded = false;
		}
	}

	//Når Ground-objekt befinder sig i triggerzone gøres figur grounded
	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Ground")
		{
			grounded = true;
		}
	}

#if UNITY_EDITOR
	void OnValidate() {
		StartInfo ();
	}

	public void StartInfo() {
		Transform sibling = null;

		foreach (Transform child in transform.parent) {
			if(child.name == "Player Sprite") {
				sibling = child;
			}
		}

//		Vector3 spriteSize = sibling.GetComponent<SpriteRenderer> ().bounds.size;
		Vector3 colliderSize = sibling.GetComponent<Collider2D> ().bounds.size;
		Vector3 colliderPos = sibling.GetComponent<Collider2D> ().bounds.center;

		resetCollider = false;
		GetComponent<Collider2D> ().isTrigger = true;
		transform.name = "Ground Check";
		
		Vector3 tempPos = colliderPos;
		tempPos.y -= colliderSize.y / 2;
		transform.position = tempPos;
		
		Vector3 tempScale = sibling.localScale;
		tempScale.x = colliderSize.x - 0.1f;
		tempScale.y = 0.15f;
		transform.localScale = tempScale;		
	}
#endif
	
}
