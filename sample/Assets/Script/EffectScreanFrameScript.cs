using UnityEngine;
using System.Collections;

public class EffectScreanFrameScript : MonoBehaviour {
	
	private	float	s_width, s_height;
	
	// Use this for initialization
	void Start () {
	
		s_width		= Screen.width	/2;
		s_height	= Screen.height	/2;
		
		
		GameObject.Find("EffectScreanFrame/EffectDown").transform.position =
			new Vector3( 0, s_height*-0.6f, 0 );

		GameObject.Find("EffectScreanFrame/EffectLeft").transform.position =
			new Vector3( s_width*-0.55f, 0, 0 );
		GameObject.Find("EffectScreanFrame/EffectLeftDown").transform.position =
			new Vector3( s_width*-0.55f, s_width*-0.6f, 0 );		
		GameObject.Find("EffectScreanFrame/EffectLeftUp").transform.position =
			new Vector3( s_width*-0.55f, s_width*0.6f, 0 );
		
		GameObject.Find("EffectScreanFrame/EffectRight").transform.position =
			new Vector3( s_width*0.55f, 0, 0 );
		GameObject.Find("EffectScreanFrame/EffectRightDown").transform.position =
			new Vector3( s_width*0.55f, s_width*-0.6f, 0 );		
		GameObject.Find("EffectScreanFrame/EffectRightUp").transform.position =
			new Vector3( s_width*0.55f, s_width*0.6f, 0 );
				
		GameObject.Find("EffectScreanFrame/EffectUp").transform.position =
			new Vector3( 0, s_width*0.6f, 0 );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
