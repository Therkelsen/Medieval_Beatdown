using UnityEngine;
using System.Collections;

public class PlatformGroundCheck2D : MonoBehaviour {

	public bool isGrounded = false;

	private bool onGround = false;
	private bool leftGround = false;

	void FixedUpdate() {
		if(onGround) {
			isGrounded = true;
		}
		if(leftGround && !onGround) {
			isGrounded = false;
		}

		leftGround = false;
		onGround = false;
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.tag == "Ground") {
			leftGround = true;
		}
	}

	void OnTriggerStay2D(Collider2D coll) {
		if(coll.tag == "Ground") {
			onGround = true;
		}
	}
		
}
