using UnityEngine;
using System.Collections;

public class PointToPointOneWay : MonoBehaviour {
	
	// Hastigheden, som objektet skal bevæge sig med.
	public float Speed=5;
	
	//De to objekter der skal bevæges imellem - kan være Empty GameObjects
	public GameObject startPos;
	public GameObject endPos;
	
	// Start køres når scenen starter.
	void Start () 
	{
		// Startpositionen sættes til første punkt
		transform.position = startPos.transform.position;
	}
	
	// Update køres hver gang der tegnes et nyt billede.
	void Update () {
		
		// Bevæg objektet mod slut positionen.
		transform.position = Vector3.MoveTowards(transform.position, endPos.transform.position, Speed * Time.deltaTime);

		// Hvis vi er kommet til punktet, starter vi forfra.
		if (Vector3.Distance(endPos.transform.position, transform.position) <= 0)
		{
			transform.position = startPos.transform.position;
		}
	}
}
