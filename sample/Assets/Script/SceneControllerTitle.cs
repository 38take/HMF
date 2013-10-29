using UnityEngine;
using System.Collections;

public class SceneControllerTitle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.N))
		{
			Application.LoadLevel("Adventure");
			GameSystemScript gameSystem = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			gameSystem.SystemOutPut(-gameSystem.GetActID());
		}		
	}
}
