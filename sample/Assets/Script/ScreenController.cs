using UnityEngine;
using System.Collections;

public class ScreenContlloer : MonoBehaviour {
	
	int	OldWidth;
	int OldHeight;

	// Use this for initialization
	void Start () {
		DefaultScreen.Par.x = (float)Screen.width / (float)DefaultScreen.Width;
		DefaultScreen.Par.y = (float)Screen.height / (float)DefaultScreen.Height;
	}
	
	// Update is called once per frame
	void Update () {
	
		if( OldWidth != Screen.width || OldHeight != Screen.height ) {
			OldWidth = Screen.width;
			OldHeight = Screen.height;
			DefaultScreen.Par.x = (float)Screen.width / (float)DefaultScreen.Width;
			DefaultScreen.Par.y = (float)Screen.height / (float)DefaultScreen.Height;
			DefaultScreen.FontPar = (DefaultScreen.Par.x + DefaultScreen.Par.y) / 2.0f;
		}
	}
}
