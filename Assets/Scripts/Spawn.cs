using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
	public int currentEnemies;
	public GameObject[] enemies;
	private Vector3 spawnPos;

	public int amountToSpawn = 2;
	public float spawnInterval = 0.2f;

	public bool canSpawn = true;


	void Start () {
		spawnPos = Vector3.zero;
		spawnPos.y = 1.5f;
		StartCoroutine (SpawnMobs ());
	}

	void Update() {
		if (canSpawn == false && currentEnemies == 0) {
			canSpawn = true;
		}

		if(canSpawn == true) {
			StartCoroutine (SpawnMobs ());
		}
	}

	IEnumerator SpawnMobs() {
		if (canSpawn == true) {
			canSpawn = false;
			for (int i = 0; i < amountToSpawn; i++) {
				int spawnType = Random.Range (0, enemies.Length);
				spawnPos.x = Random.Range (-8.5f, 8.5f);
				Instantiate (enemies [spawnType], spawnPos, Quaternion.identity);
				currentEnemies++;
				yield return new WaitForSeconds (spawnInterval);
			}
			amountToSpawn += 2;
		}
	}
}