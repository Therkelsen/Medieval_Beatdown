using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {
	//Scriptet holder styr på Health og viser Health værdi på skærmen

	public GameObject Star;
	private Vector3 spawnPos;

	[Header("-- Health settings --")]
	//Health værdi fra start
	public float startHealth = 100;
	public float currentHealth = 100;

	[Header("-- References --")]
	public GameObject healthBar;

	private HealthTracker healthTracker;

	void Start() {
		if (healthBar != null) {
			SpriteRenderer sprtRend = GetComponentInChildren<SpriteRenderer>();
			Vector3 spriteSize = sprtRend.bounds.size;

			Vector3 canvasPos = sprtRend.bounds.center;
			canvasPos.y += (spriteSize.y / 2) + 0.2f;

			Vector3 canvasSize = healthBar.transform.localScale;
			canvasSize.x = (spriteSize.x + 0.2f) / 1000.0f;

			GameObject newHealthBar = Instantiate (healthBar, canvasPos, Quaternion.identity) as GameObject;
			newHealthBar.transform.localScale = canvasSize;

			newHealthBar.transform.SetParent(transform);
			healthTracker = newHealthBar.GetComponent<HealthTracker> ();
		}
		spawnPos = Vector3.zero;
		spawnPos.x = 0.0f;
		spawnPos.y = -3.71f;
		currentHealth = startHealth;

	}

	//void Update: Hver gang der tegnes et nyt billede
	void Update () {	
		//Hvis Health er mindre eller lig med 0 går spillet videre til scenen "Lose"
		if (currentHealth <= 0) {
			if (gameObject.tag == "Player") {
				SceneManager.LoadScene ("Lose");
			} else {
				PlaySound[] sounds = GetComponents<PlaySound> ();

				bool foundSound = false;

				foreach (PlaySound sound in sounds) {
					if (sound.onDeath) {
						sound.isDead = true;
						foundSound = true;
					}
				}

				if (!foundSound) {
					Destroy (gameObject);
					GameObject.FindGameObjectWithTag ("Spawn").GetComponent<Spawn> ().currentEnemies -= 1;
					Instantiate (Star, spawnPos, Quaternion.identity);
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			currentHealth = 100;
		}
	}
	
	// modtager damage værdi fra andre objekter, trækker damage fra Health
	public void ReceiveDamage (float damage) 
	{
		currentHealth -= damage;

		float newHealthPercentage = currentHealth / startHealth;
		if (newHealthPercentage < 0) {
			newHealthPercentage = 0.0f;
		}

		if (newHealthPercentage > 1) {
			newHealthPercentage = 1.0f;
		}

		if (healthBar != null) {
			healthTracker.SetHealth (newHealthPercentage);
		}
	}
			
}
