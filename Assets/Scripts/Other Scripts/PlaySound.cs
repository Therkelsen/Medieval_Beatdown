using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

	[Header("-- Sound File --")]
	public AudioClip soundToPlay;

	[Header("-- When to play the sound --")]
	public bool whenGameBegins;
	public bool playOnCollisionHit;
	public bool playOnTriggerHit;
	public bool jumping;
	public bool shooting;
	public bool onDeath;
	public bool onInputMessage;

	[Header("-- Play when hitting --")]
	public bool hittingAnything;
	public bool hittingPlayer;
	public bool hittingNotPlayer;

	[Header("-- Other Setting --")]
	public bool destroyAfterHit;
	public bool loopSound = false;
	[Range(0.0f, 1.0f)] public float volume = 1.0f;

	private float soundLength;
	private Jump myJump;
	private ShootBullet myShoot;

	[HideInInspector] public bool isDead = false;

	void Start () {
		soundLength = soundToPlay.length;
		if (jumping) {
			myJump = GetComponent<Jump>();
		}
		if (shooting) {
			myShoot = GetComponent<ShootBullet>();
		}

		if (whenGameBegins) {
			CreateSound();
		}
	}

	void Update() {
		if (myJump != null) {
			if (myJump.playJumpSound) {
				CreateSound ();
				myJump.playJumpSound = false;
			}
		}

		if (myShoot != null) {
			if(myShoot.playShootSound) {
				CreateSound();
				myShoot.playShootSound = false;
			}
		}

		if (onDeath && isDead) {
			CreateSound();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (playOnCollisionHit) {
			if(hittingAnything) {
				CreateSound();
			}
			
			if(hittingPlayer) {
				if(coll.transform.tag == "Player") {
					CreateSound();
				}
			}

			if(hittingNotPlayer) {
				if(coll.transform.tag != "Player") {
					CreateSound();
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (playOnTriggerHit) {
			if(hittingAnything) {
				CreateSound();
			}
			
			if(hittingPlayer) {
				if(coll.tag == "Player") {
					CreateSound();
				}
			}
			
			if(hittingNotPlayer) {
				if(coll.transform.tag != "Player") {
					CreateSound();
				}
			}
		}
	}

	public void Play(bool forcePlay = false) {
		if (onInputMessage || forcePlay) {
			CreateSound();
		}
	}

	void CreateSound(bool loop = false) {
		GameObject newSound = new GameObject ();
		newSound.name = "Sound (" + soundToPlay.name + ")";
		AudioSource audio = newSound.AddComponent<AudioSource> ();
		if (!loopSound) {
			audio.PlayOneShot (soundToPlay, volume);
			Destroy (newSound, soundLength + 0.2f);
		} else {
			audio.clip = soundToPlay;
			audio.loop = true;
			audio.volume = volume;
			audio.Play();
		}

		if(destroyAfterHit) {
			Destroy(gameObject);
		}
	}

#if UNITY_EDITOR
	void OnValidate() {
		if (onDeath) {
			destroyAfterHit = true;
		}
	}
#endif

}
