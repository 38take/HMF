
using UnityEngine;
using System.Collections;

public enum INFO_STATE{
	IDLE,
	ON,
	RUN,
	OFF
};

public enum CONTENTS_STATE{
	LAMP,
	SHIP,
	OHTER
};


public class InfoWindowManager : MonoBehaviour {

	INFO_STATE		Info_State;
	INFO_STATE		Info_State_sub;
	CONTENTS_STATE	Cont_State;
	CONTENTS_STATE	Cont_State_Sub = CONTENTS_STATE.LAMP;
	
	public int		FrameCount = 0;
	int				MaxFrame = 20;
	int				MaxFrame_Sub = 20;
	
	Tex2DBaseScript	InfoWin;
	Tex2DBaseScript InfoCon;
	
	public Vector2	Position;
	public Vector2	Offset;
	public Vector2	WinSize;
	public Vector2	ConSize;
	public Vector2	WinUVPos;
	public Vector2	WinUVSize;
	public Vector2	ConUVPos;
	public Vector2	ConUVSize;	
	
	public int		INFO_STATEData = 0;
	public bool		IdleFlg = true;
	public bool		LoopFlg = false;
	public int		count = 0;
	
	// Use this for initialization
	void Start () {
		//
		Cont_State = CONTENTS_STATE.LAMP;
		//GameObject setting ActorTexture is Find.
		InfoWin = ((GameObject)GameObject.Find("InfoWindow")).GetComponent<Tex2DBaseScript>();
		InfoCon = ((GameObject)GameObject.Find ("InfoContent")).GetComponent<Tex2DBaseScript>();
		//Refreshing
		Reflesh();
		MaxFrame = MaxFrame_Sub = 10;
	}
	
	//Refresh Func
	void Reflesh(){
		//Setting Value
		WinSize.x = 800.0f;WinSize.y = 600.0f;
		Position.x = -WinSize.x;Position.y = 00.0f;
		Offset.x = 95.0f;Offset.y = 95.0f;
		ConSize.x = 600.0f;ConSize.y = 400.0f;
		
		InfoWin.SetSize(WinSize.x,WinSize.y);
		InfoWin.SetPos(Position.x,Position.y);
		InfoWin.SetUV(new Vector2(0.0f,0.0f),1.0f,1.0f);
		InfoWin.SetDepth(4);
		
		InfoCon.SetSize(WinSize.x,WinSize.y);
		InfoCon.SetPos(Position.x+Offset.x,Position.y+Offset.y);
		InfoCon.SetDepth(3);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log ("IdleFlg"+IdleFlg);
				
		//INFO_STATE
		MoveWindow(Info_State);
		
		//set Size of Actor
		InfoWin.SetSize(WinSize.x,WinSize.y);
		InfoWin.SetPos(Position.x,Position.y);
		InfoCon.SetSize(ConSize.x,ConSize.y);
		InfoCon.SetPos(Position.x+Offset.x,Position.y+Offset.y);
		
		count++;		
	}
	
	//Move Actor
	void MoveWindow(INFO_STATE Act){
		switch(Act){
		case INFO_STATE.IDLE:
			if(IdleFlg == false){
				Info_State = Info_State_sub;
				//ChangeContents();
				FrameCount = 0;
				//MaxFrame = 10;
			}
			break;
		case INFO_STATE.ON:
		/*	Debug.Log("InfoState equal ON");
			Position.x -= (0.0f + Position.x)* 0.2f;
			if(Position.x > 0.0f){
				CountResetAndLoopCheck();
			}*/
			if(MaxFrame != FrameCount){	
				Position.x = -WinSize.x +((WinSize.x/MaxFrame)*FrameCount);
				InfoWin.SetPos(Position.x,Position.y);
				InfoCon.SetPos(Position.x+Offset.x,Position.y+Offset.y);
			}else{
				Position.x = 0.0f;
				InfoWin.SetPos(Position.x,Position.y);
				InfoCon.SetPos(Position.x+Offset.x,Position.y+Offset.y);
				FrameCount=0;
				Info_State = INFO_STATE.RUN;
			}
			FrameCount++;
				break;
		case INFO_STATE.RUN:
			break;
		case INFO_STATE.OFF:
			if(-WinSize.x < Position.x){	
				Position.x =  -((WinSize.x/MaxFrame)*FrameCount); 
				InfoWin.SetPos(Position.x,Position.y);
				InfoCon.SetPos(Position.x+Offset.x,Position.y+Offset.y);
				FrameCount++;
			}else{
				FrameCount = 0;
				Position.x = (-(WinSize.x));
				Info_State = INFO_STATE.IDLE;
			}
			break;
		default:
			break;
		}
	}
	
	public void SetFrameCount(int FCount){
		MaxFrame = FCount;
	}
	
	public int GetWindowState(){
		switch(Info_State){
		case INFO_STATE.IDLE:
			return 0;
			break;
		case INFO_STATE.ON:
			return 1;
			break;
		case INFO_STATE.RUN:
			return 2;
			break;
		case INFO_STATE.OFF:
			return 3;
			break;
		default:
			return 99;
			break;
		}
		return 99;
	}
	
	public void SetWindowState(int wState){
		switch(wState){
			case 0:
				Info_State = INFO_STATE.IDLE;
				break;
			case 1:
				Info_State = INFO_STATE.ON;	
				break;
			case 2:
				Info_State = INFO_STATE.RUN;
				break;
			case 3:
				Info_State = INFO_STATE.OFF;
				break;
			default:
				break;
		}
	}
	
	public void OffWindowState(){
		Info_State = INFO_STATE.OFF;
	}
	
/*public void SetWindowSize(int wSize){
		switch(wSize){
		case 0:
			MaxSize.x = DefaultScreen.Width+100.0f;
			MaxSize.y = 150.0f;
	//		Debug.Log ("SetWindowSize: Small");
			break;
		case 1:
			MaxSize.x = DefaultScreen.Width+100.0f;
			MaxSize.y = 250.0f;
	//		Debug.Log ("SetWindowSize: Middle");
			break;
		case 2:
			MaxSize.x = DefaultScreen.Width+100.0f;
			MaxSize.y = 350.0f;
	//		Debug.Log ("SetWindowSize: Big");
			break;
		default:
			break;
		}
	//	Debug.Log ("MaxSize.y:"+MaxSize.y);
		
	}*/
	
	/*public void SetWindowType(int wType){	
		switch(wType){
		case 0:
			UVPosition.x = 0.00000f;UVPosition.y = 0.66666f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			WinType = INFO_TYPE.NORMAL;
			break;
		case 1:
			UVPosition.x = 0.00000f;UVPosition.y = 0.00000f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			WinType = INFO_TYPE.SCREAM;
			break;
		case 2:
			UVPosition.x = 0.00000f;UVPosition.y = 0.33333f;
			UVSize.x = 1.00000f;UVSize.y = 0.33333f;
			WinType = INFO_TYPE.HEARTED;
			break;
		default:
			break;
		}
		tex2d.SetUV(UVPosition,UVSize.x,UVSize.y);
	}*/
	
	public Vector2 GetWindowPos(){
		return Position;
	}
	
}
