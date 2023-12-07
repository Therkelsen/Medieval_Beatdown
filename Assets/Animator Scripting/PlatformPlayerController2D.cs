using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformPlayerController2D : MonoBehaviour {

	[Header("-- CREATE GROUND CHECK --")]
	[SerializeField] private bool createGroundCheck = false;

	[Header("-- Movement Settings --")]
	public KeyCode moveLeft = KeyCode.A;
	public KeyCode moveLeftAlt = KeyCode.LeftArrow;
	public KeyCode moveRight = KeyCode.D;
	public KeyCode moveRightAlt = KeyCode.RightArrow;
	public float maxSpeed = 7.0f;
	public float acceleration = 20.0f;
	public float dropOffTime = 0.1f;
	public float currentSpeed = 0.0f;

	private float refVelSpeed;

	[Header("-- Jump Settings --")]
	public KeyCode jumpButton = KeyCode.Space;
	public float jumpForce = 10.0f;
	public int maxJumps = 2;
	public int currentJumps = 0;

	[Header("-- Other Settings --")]
	public bool spriteLookingRight = true;

	// Other variables/references
	private Rigidbody2D myRigid;
	private PlatformGroundCheck2D groundCheck;
	private Animator anim;
	private bool beenInAir = true;
	private bool isGrounded = false;

	void Start () {
		anim = GetComponent<Animator> ();
		myRigid = GetComponent<Rigidbody2D> ();
		groundCheck = GetComponentInChildren<PlatformGroundCheck2D> ();
		isGrounded = groundCheck.isGrounded;
	}

	void Update () {
		isGrounded = groundCheck.isGrounded;

		// Movement Settings
		if(Input.GetKey(moveLeft) || Input.GetKey(moveLeftAlt)) {
			currentSpeed -= acceleration * Time.deltaTime;
		}
		if(Input.GetKey(moveRight) || Input.GetKey(moveRightAlt)) {
			currentSpeed += acceleration * Time.deltaTime;
		}
		if(!Input.GetKey(moveLeft) && !Input.GetKey(moveRight)) {
			if (Mathf.Abs (currentSpeed) > 0.05f) {
				currentSpeed = Mathf.SmoothDamp (currentSpeed, 0.0f, ref refVelSpeed, dropOffTime);
			} else {
				currentSpeed = 0.0f;
			}
		}

		if(Mathf.Abs(currentSpeed) > maxSpeed) {
			currentSpeed = maxSpeed * Mathf.Sign (currentSpeed);
		}
			
		if(anim != null) {
			if (currentSpeed != 0) {
				anim.SetBool ("Walk", true);
			} else {
				anim.SetBool ("Walk", false);
			}
		}

		Vector3 tempScale = transform.localScale;
		if(spriteLookingRight) {
			if(currentSpeed > 0) {
				tempScale.x = Mathf.Abs (tempScale.x);
			}
			if(currentSpeed < 0){
				tempScale.x = -Mathf.Abs (tempScale.x);
			}
		} else {
			if(currentSpeed > 0) {
				tempScale.x = -Mathf.Abs (tempScale.x);
			}
			if(currentSpeed < 0){
				tempScale.x = Mathf.Abs (tempScale.x);
			}
		}
		transform.localScale = tempScale;

		// Jumping Settings
		if(currentJumps < maxJumps && Input.GetKeyDown(jumpButton)) {
			currentJumps++; 
			Vector2 tempVel = myRigid.velocity;
			tempVel.y = jumpForce;
			myRigid.velocity = tempVel;

			if(anim != null) {
//				anim.SetTrigger ("Jump");
			}
		}

		if(isGrounded == false) {
			beenInAir = true;
		}

		if(isGrounded == true && beenInAir == true) {
			currentJumps = 0;
			beenInAir = false;
		}
	}

	void FixedUpdate() {
		Vector3 tempPos = transform.position;
		tempPos.x += currentSpeed * Time.deltaTime;
		transform.position = tempPos;
	}

	#if UNITY_EDITOR
	void OnValidate() {
		if(createGroundCheck == true) {
			createGroundCheck = false;
			GameObject ground;

			groundCheck = GetComponentInChildren<PlatformGroundCheck2D> ();
			// If there is no child with groundCheck, then create a new child
			if(groundCheck == null) {
				ground = new GameObject ();
				BoxCollider2D coll = ground.AddComponent<BoxCollider2D> ();
				coll.isTrigger = true;
				groundCheck = ground.AddComponent<PlatformGroundCheck2D> ();
				ground.name = "Ground Check";
				SetGroundTriggerSize (ground.transform, GetSpriteSize (gameObject));
			} else {
				SetGroundTriggerSize (groundCheck.transform, GetSpriteSize (gameObject));
			}
		}
	}

	void SetGroundTriggerSize(Transform trans, SpriteSize size) {
		trans.SetParent (null);
		trans.position = size.bottom;
		Vector3 tempScale = trans.localScale;
		tempScale.x = size.width * 0.8f;
		tempScale.y = 0.2f;
		trans.localScale = tempScale;
		trans.SetParent (gameObject.transform);
	}

	SpriteSize GetSpriteSize(GameObject go) {
		SpriteRenderer[] rends = go.GetComponentsInChildren<SpriteRenderer> ();
		float minX = 0.0f, minY = 0.0f, maxX = 0.0f, maxY = 0.0f;

		if (rends.Length > 0) {
			for (int i = 0; i < rends.Length; i++) {
				if(i == 0) {
					minX = rends [i].bounds.min.x;
					minY = rends [i].bounds.min.y;
					maxX = rends [i].bounds.max.x;
					maxY = rends [i].bounds.max.y;
				} else {
					if(rends[i].bounds.min.x < minX) {
						minX = rends [i].bounds.min.x;
					}
					if(rends[i].bounds.min.y < minY) {
						minY = rends [i].bounds.min.y;
					}
					if(rends[i].bounds.max.x > maxX) {
						maxX = rends [i].bounds.max.x;
					}
					if(rends[i].bounds.max.y > maxY) {
						maxY = rends [i].bounds.max.y;
					}
				}
			}
		}
		SpriteSize sprite = new SpriteSize ();
		sprite.width = maxX - minX;
		sprite.height = maxY - minY;
		sprite.center = new Vector3 (minX + sprite.width / 2, minY + sprite.height / 2, 0.0f);
		sprite.bottom = sprite.center;
		sprite.bottom.y = minY;
		return sprite;
//		return maxX - minX;
//		return new Vector3 (maxX - minX, maxY - minY, 0.0f);
	}
	#endif
}

[System.Serializable]
public class SpriteSize {
	public float width;
	public float height;
	public Vector3 center;
	public Vector3 bottom;
}
