using UnityEngine;
using System.Collections;

public class CircularMovement : MonoBehaviour {
	//Dette script får objekt til at bevæge sig i en cirkel
	//Det skal have tilføjet et centrum (Center) for at virke
	//Man kan vælge om objektet skal bevæge sig med eller mod uret
	//Objektet vil altid starte til højre for centrum

	//Centrum for cirkelbevægelsen
	public Transform Center;

	//Hastighed (afhænger dog også af radius
	public float Speed = 5f;

	//Bevægelse med uret?
	public bool Clockwise = true;

	//værdi til sørge for rigtig bevægelsesretning
	private int clockwise;

	private Vector3 startPosition;
	private float radius;

	// Use this for initialization
	void Start () {
	
		//Beregning af radius
		startPosition = transform.position;
		Vector2 radiusVector = new Vector2 (startPosition.x - Center.position.x, startPosition.y - Center.position.y);
		radius = radiusVector.magnitude;



	}
	
	// Update is called once per frame
	void Update () {

		if (Clockwise)
		{
			clockwise = -1;
		}

		else
		{
			clockwise = 1;
		}

		//Beregning af ny position
		transform.position = Center.position + new Vector3 (Mathf.Cos((Time.timeSinceLevelLoad)*Speed) * radius, clockwise * Mathf.Sin((Time.timeSinceLevelLoad)*Speed) * radius, 0f);                                  

	}
}
