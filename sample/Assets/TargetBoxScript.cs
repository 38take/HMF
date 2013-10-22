using UnityEngine;
using System.Collections;

public class TargetBoxScript : MonoBehaviour {

	public GameObject obj;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space))
		{
			Instantiate(obj, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		}
	}
}
