using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {

	//Dette script sættes på main camera og får kameraet til på x-aksen at følge spillerfigur

	[Header("-- References --")]
	public GameObject player;

	[Header("-- Follow Settings --")]
	public bool followX;
	public bool followY;
	public bool useOffsetX;
	public bool useOffsetY;

	[Header("-- Limitations --")]
	public bool useLimitX;
	public float minLimitX = 0, maxLimitX = 100;
	public bool useLimitY;
	public float minLimitY = 0, maxLimitY = 100;

	[Header("-- Fixed Movement --")]
	public bool canOnlyMoveLeft;
	public bool canOnlyMoveRight;
	public bool canOnlyMoveUp;
	public bool canOnlyMoveDown;

	private Vector3 offset;

	void Start() {
		offset = transform.position - player.transform.position;
	}

	void Update ()
    {
		Vector3 tempPos = transform.position;
		if (followX) {
			tempPos.x = player.transform.position.x;
			if(useOffsetX) {
				tempPos.x += offset.x;
			}
		}

		if(useLimitX) {
			if(tempPos.x < minLimitX) {
				tempPos.x = minLimitX;
			}
			if(tempPos.x > maxLimitX) {
				tempPos.x = maxLimitX;
			}
		}

		if (followY) {
			tempPos.y = player.transform.position.y;
			if(useOffsetY) {
				tempPos.y += offset.y;
			}
		}

		if(useLimitY) {
			if(tempPos.y < minLimitY) {
				tempPos.y = minLimitY;
			}
			if(tempPos.y > maxLimitY) {
				tempPos.y = maxLimitY;
			}
		}

		transform.position = tempPos;

		if (canOnlyMoveUp) {
			minLimitY = transform.position.y;
			maxLimitY = transform.position.y + 10;
		}
		if (canOnlyMoveDown) {
			minLimitY = transform.position.y - 10;
			maxLimitY = transform.position.y;
		}
		if (canOnlyMoveRight) {
			minLimitX = transform.position.x;
			maxLimitX = transform.position.x + 10;
		}
		if (canOnlyMoveLeft) {
			minLimitX = transform.position.x - 10;
			maxLimitX = transform.position.x;
		}
	}

#if UNITY_EDITOR
	void OnValidate() {
		if (canOnlyMoveUp || canOnlyMoveDown) {
			useLimitY = true;
			followY = true;
		}
		if (canOnlyMoveLeft || canOnlyMoveRight) {
			useLimitX = true;
			followX = true;
		}
	}
#endif

}
