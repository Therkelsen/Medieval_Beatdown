using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour {

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

	private bool canShoot = true;
	[HideInInspector] public bool playShootSound = false;
	private Rigidbody2D myRigid;

	void Start() {
		myRigid = GetComponent<Rigidbody2D> ();
		StartCoroutine (AutoShoot ());
	}

	void Update () {
		if (!autoShooting) {
			if (Input.GetKeyDown (KeyCode.LeftShift) && canShoot) {
				SpawnBullet ();
				StartCoroutine (ShootDelay ());
			}
		}
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

		playShootSound = true;

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
