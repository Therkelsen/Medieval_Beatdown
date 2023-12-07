using UnityEngine;
using System.Collections;

public class JumpMovement : MonoBehaviour {

	//Dette script får objektet til at hoppe fra side til side.
	//Objektet skal have rigidbody med gravity

	//Den kraft der hoppes op med
	public float JumpPower = 6f;

	//Den kraft der hoppes til siden med
	public float RightLeftSpeed = 3;

	//Den tid, der ventes inden næste hop
	public float PauseTime = 1;
	
	//Hvis den er +1 hoppes mod højre; hvis den er -1 hoppes mod venstre
	public int RightLeft;

	//Den tid der er gået siden hoppet standsede
	private float TimeCounter;


	// Use this for initialization
	void Start () {

		//Tidstæller sættes til 0 og retning til højre
		TimeCounter = 0;
		RightLeft = 1;
	}


	// Update is called once per frame
	void Update () {

		//Hvis hastighed på y-aksen er mindre end 0.01
		if (GetComponent<Rigidbody2D>().velocity.y < 0.01)
		{
			//Hvis tidstæller overstiger pausetid
			if (TimeCounter > PauseTime)
			{
				//Hop sættes i gang
				GetComponent<Rigidbody2D>().velocity = new Vector3 (RightLeftSpeed * RightLeft, JumpPower, 0f);

				//Tidstæller nulstilles
				TimeCounter = 0;

				//Hopperetning ændres
				RightLeft *= -1;
			}

			else
			{
				//Tidstæller øges med tiden siden sidste frame
				TimeCounter += Time.deltaTime;
			}
		}
	
	}
}
