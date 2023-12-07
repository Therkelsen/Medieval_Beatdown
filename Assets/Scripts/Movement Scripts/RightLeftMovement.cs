using UnityEngine;
using System.Collections;

public class RightLeftMovement : MonoBehaviour {

	// Afstanden, objektet skal bevæge sig.
	public float Distance;
	
	// Hastigheden, som objektet skal bevæge sig med.
	public float Speed;
	
	// Er objektet ved, at bevæge sig til højre? Ved at vælge "false" vil objektet starte med at bevæge sig mod venstre
	public bool moveRight = true;

	// Objektets startposition.
	private Vector3 originalPosition;
	
	// Start køres når scenen starter.
	void Start () 
	{
		// Startpositionen gemmes
		originalPosition = transform.position;		
	}
	
	// Update køres hver gang der tegnes et nyt billede.
	void Update () {
		
		// Bevægelse til højre:
		if (moveRight)
		{
			// Bevæg objektet mod højre.
			transform.Translate (Speed * Time.deltaTime, 0, 0);

			// Hvis vi er kommet for langt vender vi om.
			if (Vector3.Distance(transform.position, originalPosition) >= Distance)
			{
				originalPosition = transform.position;
				moveRight = false;
			}
		}
		
		// Bevægelse til venstre:
		else 
		{
			transform.Translate (-Speed * Time.deltaTime, 0, 0);
			
			// Hvis vi er kommet for langt vender vi om.
			if (Vector3.Distance(transform.position, originalPosition) >= Distance)
			{
				originalPosition = transform.position;
				moveRight = true;
			}
		}
		
	}
}
