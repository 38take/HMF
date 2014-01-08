
using UnityEngine;
using System.Collections;

public enum INFO_STATE{
	IDLE,
	ON,
	RUN,
	OFF
};

public class InfoWindowManager : MonoBehaviour {

	INFO_STATE		Info_State;
	INFO_STATE		Info_State_sub;
	int				Cont_State=0;
	
	
	public int		FrameCount = 0;
	int				MaxFrame = 20;
	int				MaxFrame_Sub = 20;
	
	Tex2DBaseScript	InfoWin;
	Tex2DBaseScript InfoCon;
	Tex2DGUITextureScript InfoContain;

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
		//GameObject setting ActorTexture is Find.
		InfoWin = ((GameObject)GameObject.Find("InfoWindow")).GetComponent<Tex2DBaseScript>();
		InfoContain	= ((GameObject)GameObject.Find("InfoTexture")).GetComponent<Tex2DGUITextureScript>();

		//Refreshing
		Reflesh();
		MaxFrame = MaxFrame_Sub = 10;
	}
	
	//Refresh Func
	void Reflesh(){
		//Setting Value
		WinSize.x = 760.0f;WinSize.y = 510.0f;
		Position.x = -WinSize.x;Position.y = 0.0f;
		Offset.x = 95.0f;Offset.y = 95.0f;
		ConSize.x = 600.0f;ConSize.y = 400.0f;
		
		InfoWin.SetSize(WinSize.x,WinSize.y);
		InfoWin.SetPos(Position.x,Position.y);
		InfoWin.SetUV(new Vector2(0.0f,0.0f),1.0f,1.0f);
		InfoWin.SetDepth(4);
		
		InfoContain.SetPos(Position.x + Offset.x,Position.y + Offset.y);
		InfoContain.SetSize(580.0f,360.0f);
		InfoContain.SwitchTexture(0);
		InfoContain.SetRenderFlag(true);
		
	}
	
	// Update is called once per frame
	void Update () {
				
		//INFO_STATE
		MoveWindow(Info_State);
		
		//set Size of Actor
		InfoWin.SetSize(WinSize.x,WinSize.y);
		InfoWin.SetPos(Position.x,Position.y);
		
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
				InfoContain.SwitchTexture(Cont_State);
				//MaxFrame = 10;
			}
			break;
		case INFO_STATE.ON:
			if(MaxFrame != FrameCount){	
				Position.x = -WinSize.x +((WinSize.x/MaxFrame)*FrameCount);
				InfoWin.SetPos(Position.x,Position.y);
				InfoContain.SetPos(Position.x + Offset.x,Position.y + Offset.y);
			}else{
				Position.x = 0.0f;
				InfoWin.SetPos(Position.x,Position.y);
				InfoContain.SetPos(Position.x + Offset.x,Position.y + Offset.y);
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
				InfoContain.SetPos(Position.x + Offset.x,Position.y + Offset.y);
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
	
	public void SetWindowContent(int Cont){
		Cont_State = Cont;
	}
	
	public void OffWindowState(){
		Info_State = INFO_STATE.OFF;
	}
	
	public Vector2 GetWindowPos(){
		return Position;
	}
	
}