using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerControl : MonoBehaviour {
	//Dette script lader spilleren styre figurens bevægelser med piletaster eller med adsw

	//MoveSpeed styrer hastigheden af spillerens bevægelse
	public float moveSpeed = 5;

	//her kan man vælge om det skal være muligt at bevæge sig op/ned og højre/venstre
	public bool upDown = false;
	public bool rightLeft = true;
	public bool startLookingRight = false;
	public bool startLookingLeft = false;

	//værdi for hvor meget x og y skal ændres ved hver frame
	private float deltaX;
	private float deltaY;
	public GameObject myPlatform;
	
	private Rigidbody2D myRigid;
	[HideInInspector] public bool moveLeft = true;
	[HideInInspector] public bool moveRight = true;

	void Start() {
		myRigid = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Hvis right/left bevægelse er tilvalgt beregnes deltaX. Input.GetAxis er en Unity-feature, som giver bløde bevægelser.
		if (rightLeft)
		{
			deltaX = moveSpeed * Input.GetAxis("Horizontal");

			if(!moveLeft) {
				if(deltaX < 0) {
					deltaX = 0;
				}
			}
			if(!moveRight) {
				if(deltaX > 0) {
					deltaX = 0;
				}
			}

			float scaleX = transform.localScale.x;

			if(startLookingRight) {
				if((scaleX > 0 && deltaX < 0) || (scaleX < 0 && deltaX > 0)) {
//					print ("Changed direction");
					Vector3 tempScale = transform.localScale;
					tempScale.x *= -1;
					transform.localScale = tempScale;
				}
			}

			if(startLookingLeft) {
				if((scaleX > 0 && deltaX > 0) || (scaleX < 0 && deltaX < 0)) {
//					print ("Changed direction");
					Vector3 tempScale = transform.localScale;
					tempScale.x *= -1;
					transform.localScale = tempScale;
				}
			}

			//Her ændres objektets hastighed på x-akse. Y-værdi fastholdes.
			myRigid.velocity = new Vector2 (deltaX, myRigid.velocity.y);
		}

		//Hvis right/left bevægelse er tilvalgt beregnes deltaY. Input.GetAxis er en Unity-feature, som giver bløde bevægelser.
		if (upDown)
		{
			deltaY = moveSpeed * Input.GetAxis("Vertical");

			//Her ændres objektets hastighed på y-akse. X-værdi fastholdes.
			myRigid.velocity = new Vector2 (myRigid.velocity.x, deltaY);
		}

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (!upDown) {
			if (coll.tag == "Ground") {
				myPlatform = coll.gameObject;
				transform.parent = coll.gameObject.transform;
			}
		}
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (!upDown) {
			if (coll.tag == "Ground" && myPlatform == null) {
				myPlatform = coll.gameObject;
				transform.parent = coll.gameObject.transform;
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (!upDown) {
			if (coll.tag == "Ground") {
				myPlatform = null;
				transform.parent = null;
			}
		}
	}


#if UNITY_EDITOR
	void OnValidate() {
		if (upDown) {
			GetComponent<Rigidbody2D> ().gravityScale = 0f;
		} else {
			if(rightLeft && GetComponent<Rigidbody2D>().gravityScale == 0) {
				GetComponent<Rigidbody2D> ().gravityScale = 1f;
			}
		}
	}
#endif
	
}