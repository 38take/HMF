
using UnityEngine;
using System.Collections;

public class TextWindowScript : MonoBehaviour {

	GameObject texwin;
	Tex2DBaseScript tex2d;
	Tex2DBaseScript TextWindow;
	Vector2 Position;
	Vector2 Size;
	Vector2 UVPosition;
	Vector2 UVSize;	
	
	// initializate
	void Start() {
		texwin = GameObject.Find("TextWindow");
		tex2d = texwin.GetComponent<Tex2DBaseScript>();
		TextWindow = new Tex2DBaseScript();
		//Set Size
		tex2d.SetPos(0.0f,0.0f);
		tex2d.SetSize(20.0f,20.0f);
		UVPosition.x = 0.0f;UVPosition.y = 0.0f;
		UVSize.x = 1.0f;UVSize.y = 1.0f;
		tex2d.SetUV(UVPosition,UVSize.x,UVSize.y);
	//	Size.x=100.0f;Size.y=200.0f;
	}
	
	void Update(){
	//	tex2d.SetSizeFlexiblePoint(,);
	//	Size.x += 1.0f;Size.y +=1.0f; 

	}
	
}
