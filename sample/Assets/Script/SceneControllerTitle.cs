using UnityEngine;
using System.Collections;

public class SceneControllerTitle : MonoBehaviour {

	bool press;
	// Use this for initialization
	void Start () {
		press = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!press && Input.GetMouseButton(0))
		{
			Application.LoadLevel("Adventure");
			GameSystemScript gameSystem = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			gameSystem.SystemOutPut(-gameSystem.GetActID());
			press = true;
		}	
		else 
			press = false;
	}
}
