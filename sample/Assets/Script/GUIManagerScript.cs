using UnityEngine;
using System.Collections;

public class GUIManagerScript : MonoBehaviour {
	
	public GameObject TextBoxObj;
	
	Tex2DBaseScript xxxx = new Tex2DBaseScript();
	
	// Use this for initialization
	void Start () {
		Instantiate(this.TextBoxObj,new Vector3(0,0,0),Quaternion.identity);	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
