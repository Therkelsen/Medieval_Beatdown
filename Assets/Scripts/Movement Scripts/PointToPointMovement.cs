using UnityEngine;
using System.Collections;

public class PointToPointMovement : MonoBehaviour {
	
	// Hastigheden, som objektet skal bevæge sig med.
	public float Speed=5;
	
	// Er objektet ved, at bevæge sig mod slut positionen?
	private bool moveEnd = true;

    //De to objekter der skal bevæges imellem
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
		
		// Bevægelse mod slut position:
		if (moveEnd)
		{
            // Bevæg objektet mod slut positionen.
            transform.position = Vector3.MoveTowards(transform.position, endPos.transform.position, Speed * Time.deltaTime);
			// Hvis vi er kommet til punktet, vender vi om.
			if (Vector3.Distance(endPos.transform.position, transform.position) <= 0)
			{
				moveEnd = false;
			}
		}
		
		// Bevægelse mod start positionen:
		else 
		{
            transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, Speed * Time.deltaTime);
            // Hvis vi er kommet til punktet, vender vi om.
            if (Vector3.Distance(startPos.transform.position, transform.position) <= 0)
			{
				moveEnd = true;
			}
		}
		
	}
}