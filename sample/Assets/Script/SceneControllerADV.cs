using UnityEngine;
using System.Collections;

public class SceneControllerADV : MonoBehaviour {
	
	TextBoxScript StextBox;
	// Use this for initialization
	void Start () {
		StextBox = ((GameObject)GameObject.Find("TextBox")).GetComponent<TextBoxScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!StextBox.isValid())
		{
			Application.LoadLevel("Play");
			GameSystemScript gameSystem = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			gameSystem.SystemOutPut(1);
		}
	}
}
