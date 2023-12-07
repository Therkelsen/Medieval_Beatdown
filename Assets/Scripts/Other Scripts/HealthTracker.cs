using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour {

	public Image healthBar;

	private float currentHealth = 1.0f;
	private float targetHealth = 1.0f;

	private float refHealth;

	void Update() {
		healthBar.fillAmount = Mathf.SmoothDamp (currentHealth, targetHealth, ref refHealth, 0.08f);
		currentHealth = healthBar.fillAmount;
	}

	public void SetHealth(float amount) {
		targetHealth = amount;
	}
}
