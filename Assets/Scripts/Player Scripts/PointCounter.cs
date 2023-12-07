using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PointCounter : MonoBehaviour {
	//Scriptet holder styr på point og viser point på skærmen
	
	
	//siger at vi skal have en variabel for hele tal, som kaldes "Score"
	private int score;
	
	//antal point der skal til for at vinde spillet
	public int WinValue = 1000;
	
	//Den skrifttype, som Score skrives med på skærmen
	public GUIStyle ScoreStyle;
	
	
	
	// void Start køres når scenen startes (og kun der)
	void Start () {
		
		//Score sættes fra start til 0
		score = 0;
		
	}
	
	
		
	//void Update: Hver gang der tegnes et nyt billede
	void Update () {
	
		//Hvis antal point er større eller lig med WinValue går spillet videre til scenen "Win"
		if (score >= WinValue)
		{
			SceneManager.LoadScene ("Win");
		}
	}
	
	
	// modtager point værdier fra andre objekter, lægger point til PointValue
	public void ReceivePoints (int points) 
	{
		score = score + points;
	}
	
	
	//Skriver den aktuelle Score i skærmens nederste venstre hjørne
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width*0.02f, Screen.height*0.05f, 500, 500), "" +	"Score: "+ score, ScoreStyle);
	}
}
