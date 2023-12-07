using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Jump : MonoBehaviour {
	//Dette script lader spilleren hoppe med figuren.
	//Figuren SKAL have child: et empty object med trigger collider og scriptet "GroundCheck"
	//De ting, figuren skal kunne hoppe på, skal have et tag kaldet "Ground"

	//Her vælger man, om der kan hoppes i luften
	public bool DoubleJump = false;
	public bool MultiJump = false;

	//Den kraft man hopper med
	public float JumpPower = 8f;

	//fortæller om figuren er grounded
	private bool grounded = true;

	//antal hop siden figuren sidst var grounded
	private int jumpCounter;
	[HideInInspector] public bool playJumpSound = false;

	//det script på ground trigger (child), som undersøger om figuren er grounded
	private GroundCheck groundCheck;



	// Use this for initialization
	void Start () {
		//finder scriptet GroundCheck, så det kan undersøges om figuren er grounded
		groundCheck = gameObject.GetComponentInChildren <GroundCheck>();
		if (groundCheck == null) {
			Debug.LogError("Missing a child with Ground Check!");
		}
	}


	// Update is called once per frame
	void Update () {
		if (groundCheck != null) {
			//grounded gøres sand, hvis figuren er grounded (den finder bool Grounden i scriptet GroundCheck)
			grounded = groundCheck.grounded;

			//Hvis der trykkes Space OG figuren er enten grounded, klar til doublejump eller multijump er tilsluttet, skal figuren hoppe
			if (Input.GetKeyDown (KeyCode.Space) && (grounded || DoubleJump && jumpCounter < 2 || MultiJump)) {

				//Ændrer figurens hastighed: x beholdes, y sættes til JumpPower, z sættes til 0
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, JumpPower);
				playJumpSound = true;

				//antal hop øges med 1
				jumpCounter += 1;

				//hvis grounded sættes hoptæller tilbage til 1
				if (grounded) {
					jumpCounter = 1;
				}
			}
		}	
	}
}
