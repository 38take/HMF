
using UnityEngine;
using System.Collections;

public enum WINDOW_STATE{
	IDLE,
	ON,
	RUN,
	OFF
};
public enum WINDOW_SIZE{
	SMALL,
	MIDIUM,
	BIG
};
public enum WINDOW_TYPE{
	NORMAL,
	SCREAM,
	HEARTED
};

public class TextWindowScript : MonoBehaviour {

	public Tex2DBaseScript 	tex2d;
	public Vector2 		Position;
	public Vector2 		Size = new Vector2(0.0f,0.0f);
	public Vector2 		UVPosition;
	public Vector2 		UVSize;
	public Vector2		MaxSize;
	public int			FrameCount;
	public int			MaxFrame;
	public bool			WindowSetFlg = false;
	public WINDOW_STATE	WinState = WINDOW_STATE.ON;
	public WINDOW_SIZE 	WinSize = WINDOW_SIZE.SMALL;
	public WINDOW_TYPE	WinType = WINDOW_TYPE.HEARTED;
	
	// initializate
	void Start() {
		//Find GameObject
		tex2d = ((GameObject)GameObject.Find("TextWindow")).GetComponent<Tex2DBaseScript>();
		//Set Pos & Size & UV
		Position.x = 0.0f;Position.y = DefaultScreen.Height/2.0f;;
		//SetWindowSize(0);
		//SetWindowType(0);
		tex2d.SetPos(Position.x,Position.y);
		tex2d.SetSize(Size.x,Size.y);
		tex2d.SetUV(UVPosition,UVSize.x,UVSize.y);
		//Set Frame
		MaxFrame = 10;
		WinState = 0;
	}
	
	void Update(){
		//SetWindowType(WinType);
		RunningWindow();
	}
	
	void RunningWindow(){
		//switch(ON(SCALE UP)/RUN/SCALE DOWN/OFF)
		switch(WinState){
		case WINDOW_STATE.IDLE:
			break;
		case WINDOW_STATE.ON:
			if(MaxFrame != FrameCount){	
				Size.x = (MaxSize.x/MaxFrame)*FrameCount;
				Size.y = (MaxSize.y/MaxFrame)*FrameCount; 
				tex2d.SetSize(Size.x,Size.y);
				FrameCount++;
			}else{
				//FrameCount = 0;
			}
			break;
		//RUN
		case WINDOW_STATE.RUN:
			break;
		//SCALE_DOWN
		case WINDOW_STATE.OFF:
			if(Size.x >= 0.0f){	
				Size.x = MaxSize.x - ((MaxSize.x/MaxFrame)*FrameCount);
				Size.y = MaxSize.y - ((MaxSize.y/MaxFrame)*FrameCount); 
				tex2d.SetSize(Size.x,Size.y);
				FrameCount++;
			}else{
				Size.x = 0.0f;Size.y = 0.0f;
				FrameCount = 0;
				WinState = WINDOW_STATE.IDLE;
			}
			break;
		default:
			break;
		}
	}
	
	public void SetFrameCount(int FCount){
		MaxFrame = FCount;
	}
	
	public WINDOW_STATE GetWindowState(){
		return WinState;
	}
	
	public void SetWindowState(WINDOW_STATE wState){
		WinState = wState;
	}
	
	public void OffWindowState(){
		WinState = WINDOW_STATE.OFF;
	}
	
/*	void SetWindowSize(WINDOW_SIZE wSize){
		switch(wSize){
		case WINDOW_SIZE.SMALL:
			MaxSize.x = DefaultScreen.Width;
			MaxSize.y = 150.0f;
			break;
		case WINDOW_SIZE.MIDIUM:
			MaxSize.x = DefaultScreen.Width;
			MaxSize.y = 250.0f;		
			break;
		case WINDOW_SIZE.BIG:
			MaxSize.x = DefaultScreen.Width;
			MaxSize.y = 350.0f;
			break;
		}
	}*/
	
	public void SetWindowSize(int wSize){
		switch(wSize){
		case 0:
			MaxSize.x = DefaultScreen.Width;
			MaxSize.y = 150.0f;
			break;
		case 1:
			MaxSize.x = DefaultScreen.Width;
			MaxSize.y = 250.0f;		
			break;
		case 2:
			MaxSize.x = DefaultScreen.Width;
			MaxSize.y = 350.0f;
			break;
		default:
			break;
		}
	}
	
/*	public void SetWindowType(WINDOW_TYPE wType){	
		switch(wType){
		case WINDOW_TYPE.NORMAL:
			UVPosition.x = 0.00000f;UVPosition.y = 0.66666f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			break;
		case WINDOW_TYPE.SCREAM:
			UVPosition.x = 0.00000f;UVPosition.y = 0.00000f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			break;
		case WINDOW_TYPE.HEARTED:
			UVPosition.x = 0.00000f;UVPosition.y = 0.33333f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			break;
		}
	}
*/
	
	public void SetWindowType(int wType){	
		switch(wType){
		case 0:
			UVPosition.x = 0.00000f;UVPosition.y = 0.66666f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			break;
		case 1:
			UVPosition.x = 0.00000f;UVPosition.y = 0.00000f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			break;
		case 2:
			UVPosition.x = 0.00000f;UVPosition.y = 0.33333f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			break;
		default:
			break;
		}
	}
	
}
