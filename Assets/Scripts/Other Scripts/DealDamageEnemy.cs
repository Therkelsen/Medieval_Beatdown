using UnityEngine;
using System.Collections;

public class DealDamageEnemy : MonoBehaviour {
	// Giver damage, når den rammes af en spiller
	// Der skal være en collider (ikke trigger) på dette gameObject, for at det virker.
 
	//Størrelsen af den skade, objektet giver
	public float damage = 10;

	public bool useTriggerHit = false;
	public bool useCollisionHit = true;

	void OnCollisionEnter2D(Collision2D coll) {
		if (useCollisionHit) {
			if(coll.gameObject.tag == "Enemy") {
				if (coll.gameObject.GetComponent<Health> () != null) {
					coll.gameObject.GetComponent<Health> ().ReceiveDamage (damage);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (useTriggerHit) {
			if (coll.gameObject.tag == "Enemy") {
				if (coll.gameObject.GetComponent<Health> () != null) {
					coll.gameObject.GetComponent<Health> ().ReceiveDamage (damage);
				}
			}
		}
	}

}
