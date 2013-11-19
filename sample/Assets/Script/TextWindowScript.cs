
using UnityEngine;
using System.Collections;

public class TextWindowScript : MonoBehaviour {

	GameObject texwin;
	Tex2DBaseScript tex2d;
	Vector2 Position;
	Vector2 Size;
	Vector2 UVPosition;
	Vector2 UVSize;
	Vector2	MaxSize;
	int		FrameCount;
	int		MaxFrame;
	int		WinState;
	
	// initializate
	void Start() {
		//Find GameObject
		texwin = GameObject.Find("TextWindow");
		tex2d = texwin.GetComponent<Tex2DBaseScript>();
		//Set Pos & Size & UV
		Position.x = 0.0f;Position.y = DefaultScreen.Height/2.0f;;
		Size.x = 100.0f;Size.y = 50.0f;//0.0f&0.0f
		UVPosition.x = 0.0f;UVPosition.y = 0.666666f;
		UVSize.x = 1.0f;UVSize.y = 0.33333f;
		tex2d.SetPos(Position.x,Position.y);
		tex2d.SetSize(Size.x,Size.y);
		tex2d.SetUV(UVPosition,UVSize.x,UVSize.y);
		//Set MaxSize
		MaxSize.x = DefaultScreen.Width;
		MaxSize.y = DefaultScreen.Height/2.0f;
		//Set Frame
		MaxFrame = 10;
		WinState = 0;
	}
	
	void Update(){
		//
		switch(WinState){
		case 0:
		if(MaxFrame != FrameCount){	
			Size.x = (MaxSize.x/MaxFrame)*FrameCount;
			Size.y = (MaxSize.y/MaxFrame)*FrameCount; 
			tex2d.SetSize(Size.x,Size.y);
			FrameCount++;
		}
			break;
		case 1:
			break;
		case 2:
			break;
		default:
			break;
		}
	}
	
	void SetFrameCount(int FCount){
		MaxFrame = FCount;
	}
	
	void SetWindowType(int Type){
		WinState = Type;
	}
}
