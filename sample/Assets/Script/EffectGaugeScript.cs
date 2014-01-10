using UnityEngine;
using System.Collections;

public class EffectGaugeScript : MonoBehaviour {
	
	private	float	s_width, s_height;
	
	// Use this for initialization
	void Start () {
	
		s_width		= Screen.width	/2;
		s_height	= Screen.height	/2;
		
		transform.position = new Vector3( s_width*0.415f, s_height*-0.3f, 0 );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
