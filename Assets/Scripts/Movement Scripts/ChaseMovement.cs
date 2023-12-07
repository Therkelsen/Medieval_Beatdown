using UnityEngine;
using System.Collections;

// Scriptet får objektet til, at følge efter et andet objekt, med en bestemt hastighed.
public class ChaseMovement : MonoBehaviour {
	
	// Objektet, som vi skal følge efter.
	public GameObject chasee;
	string Jesus;


	// Hastigheden, som vi skal bevæge os med.
	public float movementSpeed = 2f;
		
	void Start() {
		chasee = GameObject.FindGameObjectWithTag ("Player");
	}

	// void Update: Hver gang der tegnes et nyt billede
	void Update () {
		Vector3 tempPos = transform.position;
		Vector3 tempScale = transform.localScale;
		if(chasee.transform.position.x > transform.position.x){
			tempPos.x += movementSpeed * Time.deltaTime;
			tempScale.x = 1;
		}else{
			tempPos.x -= movementSpeed * Time.deltaTime;
			tempScale.x = -1;
		}
		transform.position = tempPos;	
		transform.localScale = tempScale;
	}
}
