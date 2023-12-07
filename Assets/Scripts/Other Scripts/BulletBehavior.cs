using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {

	private bool canHitPlayer = false, canHitGround = false, canHitEnemy = false;
	private Vector3 direction;
	private float speed;
	private bool fixedOrientation = true;
	private bool targetHit = false;
	private bool destroyOnHit = true;
	private float damage = 0f;

	void Update () {
		transform.position += direction * speed * Time.deltaTime;
		if (!fixedOrientation) {
			float rotation = Vector2.Angle(Vector2.up, (Vector2) direction);
			if(direction.x > 0) {
				rotation *= -1;
			}
			Vector3 tempRot = transform.eulerAngles;
			tempRot.z = rotation;
			transform.eulerAngles = tempRot;
		}
	}

	public void SetInfo(Vector3 newDir, float newSpeed, bool fixedOri, bool playerHit, bool groundHit, bool enemyHit, bool destroy, float bulletDamage) {
		damage = bulletDamage;
		destroyOnHit = destroy;
		canHitPlayer = playerHit;
		canHitGround = groundHit;
		canHitEnemy = enemyHit;
		direction = newDir;
		speed = newSpeed;
		fixedOrientation = fixedOri;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (!targetHit) {

			if (canHitPlayer) {
				if (coll.tag == "Player") {
					CheckCollision(coll);
//					if (coll.gameObject.GetComponent<Health> () != null || coll.transform.parent.GetComponent<Health>() != null) {
//						if (coll.GetComponent<Health> () != null) {
//							coll.GetComponent<Health>().ReceiveDamage(damage);
//						}
//						if (coll.transform.parent.GetComponent<Health> () != null) {
//							coll.transform.parent.GetComponent<Health>().ReceiveDamage(damage);
//						}
////						print ("Player hit, health script");
//					} else {
//						print ("Player hit, no health script");
//					}
//					targetHit = true;
//
//					if(GetComponent<PlaySound>() == null && destroyOnHit) {
//						Destroy(gameObject);
//					}
				}
			}

			if(canHitGround) {
				if(coll.tag == "Ground") {
					CheckCollision(coll, true);
				}
			}

			if(canHitEnemy) {
				if(coll.tag == "Enemy") {
					CheckCollision(coll);
				}
			}

		}
	}


	void CheckCollision(Collider2D coll, bool forceDestroyOnHit = false) {
		if(coll.transform.parent == null) {
			if (coll.gameObject.GetComponent<Health> () != null) {
				if (coll.GetComponent<Health> () != null) {
					coll.GetComponent<Health>().ReceiveDamage(damage);
				}
				
				targetHit = true;
				
				if(GetComponent<PlaySound>() == null && destroyOnHit) {
					Destroy(gameObject);
				} else {
					GetComponent<PlaySound>().Play();
				}
			}
		} else {
			if (coll.gameObject.GetComponent<Health> () != null || coll.transform.parent.GetComponent<Health>() != null) {
				if (coll.GetComponent<Health> () != null) {
					coll.GetComponent<Health>().ReceiveDamage(damage);
				}
				if (coll.transform.parent.GetComponent<Health> () != null) {
					coll.transform.parent.GetComponent<Health>().ReceiveDamage(damage);
				}
				
				targetHit = true;
				
				if(GetComponent<PlaySound>() == null && destroyOnHit) {
					Destroy(gameObject);
				} else {
					GetComponent<PlaySound>().Play();
				}
			}
		}

		if (forceDestroyOnHit) {
			if(GetComponent<PlaySound>() == null && destroyOnHit) {
				Destroy(gameObject);
			} else {
				PlaySound sound = GetComponent<PlaySound>();
				sound.destroyAfterHit = true;
				sound.Play();
			}
		}
	}

	void OnDestroy() {
		if(GetComponent<PlaySound>() != null) {
			PlaySound sound = GetComponent<PlaySound>();
			sound.Play(true);
		}
	}

}
