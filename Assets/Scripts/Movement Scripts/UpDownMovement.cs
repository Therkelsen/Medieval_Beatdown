using UnityEngine;
using System.Collections;

public class UpDownMovement : MonoBehaviour {
	//Dette script får objekt til at bevæge sig op og ned

	// Afstanden, objektet skal bevæge sig.
	public float Distance;
	
	// Hastigheden, som objektet skal bevæge sig med.
	public float Speed;
	
	// Er objektet ved, at bevæge sig op? Ved at vælge "false" vil objekt starte med at bevæge sig ned.
	public bool MoveUp = true;
	
	// Objektets startposition.
	private Vector3 originalPosition;


	// Start køres når scenen starter.
	void Start () 
	{
		//objektets startposition gemmes
		originalPosition = transform.position;
	}


	// Update køres hver gang der tegnes et nyt billede.
	void Update () {
		
		// Bevægelse op:
		if (MoveUp)
		{
			// Bevæg objektet op.
			transform.Translate (0, Speed * Time.deltaTime, 0);
			
			// Hvis vi er kommet for langt vender vi om.
			if (Vector3.Distance(transform.position, originalPosition) >= Distance)
			{
				originalPosition = transform.position;
				MoveUp = false;
			}
		}
		
		else 
		{
			// Bevæg objektet ned:
			transform.Translate (0, -Speed * Time.deltaTime, 0);
			
			// Hvis vi er kommet for langt vender vi om.
			if (Vector3.Distance(transform.position, originalPosition) >= Distance)
			{
				originalPosition = transform.position;
				MoveUp = true;
			}
		}
		
	}
}
