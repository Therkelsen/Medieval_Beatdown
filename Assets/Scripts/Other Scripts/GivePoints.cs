using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]

// Giver point, når den rammes af en spiller.
// Der skal være en trigger collider på dette gameObject, for at det virker.

[ExecuteInEditMode]
public class GivePoints : MonoBehaviour {
	
	//Mængden af point, objektet giver.
	public int Points = 5;

	void Start() {
		GetComponent<Collider2D> ().isTrigger = true;
	}

	//når noget colliderer med det her objekt bliver det betegnet "other"
	private void OnTriggerEnter2D (Collider2D coll)
	{
		//Hvis "other" har et tag, der hedder "Player" sendes Points til funktionen ReceivePoints i scriptet PointCounter på Player.
		if (coll.tag == "Player")
		{
			coll.gameObject.GetComponentInParent<PointCounter>().ReceivePoints(Points);
			if(GetComponent<PlaySound>() == null) {
				Destroy(gameObject);
			}
		}
	}
}
	