using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {
private Animator anim;

	[Header("-- Bullet Settings --")]
	public GameObject bulletPrefab;
	public float bulletSpeed = 5.0f;
	public float bulletDamage = 10.0f;
	public float bulletLifetime = 1f;

	[Header("-- Shooting Direction --")]
	public bool shootInFixedDirection = false;
	public Vector2 shootDirection = Vector2.zero;
	public bool shootTowardsMousePosition = false;
	public bool shootTowardsObject = false;
	public GameObject targetObject = null;
	public bool shootInMovingDirection = false;

	[Header("-- 2D Platform Specific Shooting --")]
	[Tooltip("Only works in 2D platform games!")]
	public bool shootInFacingDirection = false;
	public bool startFacingRight = false;

	[Header("-- What to hit -- ")]
	public bool canHitPlayer = false;
	public bool canHitGround = false;
	public bool canHitEnemies = false;

	[Header("-- Other Settings --")]
	public bool autoShooting = false;
	public float timeBetweenShots = 0.2f;
	public Vector2 spawnOffset = Vector2.zero;
	public bool destroyOnHit = true;
	public bool fixedOrientation = false;

	public Sprite attackSprite;

	private bool canShoot = true;
	private Rigidbody2D myRigid;
	private SpriteRenderer sprtRend;

	void Start() {
		sprtRend = GetComponentInChildren<SpriteRenderer> ();
		myRigid = GetComponent<Rigidbody2D> ();
		StartCoroutine (AutoShoot ());
		anim = GetComponent<Animator> ();
	}

	void Update () {
		if (!autoShooting) {
			if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Mouse0) && canShoot) {
				SpawnBullet ();
				StartCoroutine (ShootDelay ());
				StartCoroutine(SpriteChange ());
			}
		}
		if(anim != null) {
			if (Input.GetKeyDown (KeyCode.LeftShift)||Input.GetKeyDown(KeyCode.Mouse0)) {
				anim.SetBool ("Attack", true);
			} else {
				anim.SetBool ("Attack", false);
			}
		}
	}

	IEnumerator SpriteChange(){
		Sprite oldSprite = sprtRend.sprite;
		sprtRend.sprite = attackSprite;
		yield return new WaitForSeconds (0.5f);
		sprtRend.sprite = oldSprite;
	}

	void SpawnBullet() {
		Vector3 bulletDir = Vector3.zero;

		Vector3 spawnPos = transform.position + (Vector3)spawnOffset;

		if (shootTowardsMousePosition) {
			bulletDir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - spawnPos;
			bulletDir.z = 0;
			bulletDir = bulletDir.normalized;
		}
		if (shootInFixedDirection) {
			bulletDir = (Vector3)shootDirection.normalized;
		}
		if (shootInFacingDirection) {
			float facingDirection = transform.localScale.x;
			if (startFacingRight) {
				if (facingDirection > 0) {
					bulletDir = Vector3.right;
				} else {
					bulletDir = Vector3.left;
				}
			} else {
				if (facingDirection < 0) {
					bulletDir = Vector3.right;
				} else {
					bulletDir = Vector3.left;
				}
			}
		}
		if (shootInMovingDirection) {
			bulletDir = (Vector3)myRigid.velocity.normalized;
		}

		if (shootTowardsObject) {
			bulletDir = (targetObject.transform.position - transform.position).normalized;
		}

		GameObject newBullet = Instantiate (bulletPrefab, spawnPos, Quaternion.identity) as GameObject;
		newBullet.GetComponent<BulletBehavior> ().SetInfo (bulletDir, bulletSpeed, fixedOrientation, canHitPlayer, canHitGround, canHitEnemies, destroyOnHit, bulletDamage);

		if (bulletLifetime <= 0) {
			bulletLifetime = 1.0f;
		}

		Destroy (newBullet, bulletLifetime);
	}

	IEnumerator ShootDelay() {
		if (timeBetweenShots > 0) {
			canShoot = false;
			yield return new WaitForSeconds (timeBetweenShots);
			canShoot = true;
		}
	}

	IEnumerator AutoShoot() {
		while (autoShooting) {
			yield return new WaitForSeconds (timeBetweenShots);
			SpawnBullet ();
		}
	}
}
