using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.P)) {

			if(Time.timeScale == 1){

				Time.timeScale = 0;

			}
			else
			{

				Time.timeScale = 1;

			}
				
		}

	}
}
