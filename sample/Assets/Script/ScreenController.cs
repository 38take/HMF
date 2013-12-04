using UnityEngine;
using System.Collections;

public class ScreenController : MonoBehaviour {
	
	int	OldWidth;
	int OldHeight;

	// Use this for initialization
	void Start () {
		DefaultScreen.Par.x = (float)Screen.width / (float)DefaultScreen.Width;
		DefaultScreen.Par.y = (float)Screen.height / (float)DefaultScreen.Height;
		DefaultScreen.ParInv.x =  (float)DefaultScreen.Width  / (float)Screen.width ;
		DefaultScreen.ParInv.y =  (float)DefaultScreen.Height / (float)Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	
		if( OldWidth != Screen.width || OldHeight != Screen.height ) {
			OldWidth = Screen.width;
			OldHeight = Screen.height;
			DefaultScreen.Par.x = (float)Screen.width / (float)DefaultScreen.Width;
			DefaultScreen.Par.y = (float)Screen.height / (float)DefaultScreen.Height;
			DefaultScreen.ParInv.x =  (float)DefaultScreen.Width  / (float)Screen.width ;
			DefaultScreen.ParInv.y =  (float)DefaultScreen.Height / (float)Screen.height;
			DefaultScreen.FontPar = (DefaultScreen.Par.x + DefaultScreen.Par.y) / 2.0f;
		}
	}
}
