using UnityEngine;
using System.Collections;

public enum ActorAct{
	Idle,
	OnStage,
	OutStage,
	Laugh,
	Angry,
	Panic
}

public enum Emotion{
	Normal,
	Grad,
	Angry,
	Panic
}

public class MasterActScript : MonoBehaviour {
	
	ActorAct		CharaAct;
	ActorAct		CharaAct_Sub;
	Emotion			CharaEmo;
	Emotion			CharaEmo_Sub = Emotion.Normal;
	
	int				MaxFrame = 20;
	int				MaxFrame_Sub = 20;
	
	Tex2DBaseScript	ActorBody;
	Tex2DBaseScript ActorHead;
	
	public Vector2	Position;
	public Vector2	Offset;
	public Vector2	BodySize;
	public Vector2	HeadSize;
	public Vector2	BodyUVPos;
	public Vector2	BodyUVSize;
	public Vector2	HeadUVPos;
	public Vector2	HeadUVSize;	
	
	public int		ActorActData = 0;
	public bool		IdleFlg = true;
	public bool		LoopFlg = false;
	public int		count = 0;
	
	// Use this for initialization
	void Start () {
		//
		CharaEmo = Emotion.Normal;
		//GameObject setting ActorTexture is Find.
		ActorBody = ((GameObject)GameObject.Find("MasterBody")).GetComponent<Tex2DBaseScript>();
		ActorHead = ((GameObject)GameObject.Find ("MasterHead")).GetComponent<Tex2DBaseScript>();
		//Refreshing
		Reflesh();
		MaxFrame = MaxFrame_Sub = 0;
	}
	
	//Refresh Func
	void Reflesh(){
		//Setting Value
		Position.x = DefaultScreen.Width;Position.y = 50.0f;
		BodySize.x = 300.0f;BodySize.y = 800.0f;
		Offset.x = 70.0f;Offset.y = 0.0f;
		HeadSize.x = 220.0f;HeadSize.y = 220.0f;
		
		ActorBody.SetSize(BodySize.x,BodySize.y);
		ActorBody.SetPos(Position.x,Position.y);
		ActorBody.SetDepth(5);
		
		ActorHead.SetSize(BodySize.x,BodySize.y);
		ActorHead.SetPos(Position.x+Offset.x,Position.y+Offset.y);
		ActorHead.SetDepth(5);
		
		ChangeEmotionUV();
		
	}
	
	// Update is called once per frame
	void Update () {
				
		//ActorAct
		MoveActor(CharaAct);
		
		//set Size of Actor
		ActorBody.SetSize(BodySize.x,BodySize.y);
		ActorBody.SetPos(Position.x,Position.y);
		ActorHead.SetSize(HeadSize.x,HeadSize.y);
		ActorHead.SetPos(Position.x+Offset.x,Position.y+Offset.y);
		
		count++;		
	}
	
	//Move Actor
	void MoveActor(ActorAct Act){
		switch(Act){
		case ActorAct.Idle:
			if(IdleFlg == false){
				CharaAct = CharaAct_Sub;
				ChangeEmotionUV();
				count = 0;
				MaxFrame = MaxFrame_Sub;
			}
			break;
		case ActorAct.OnStage:
			Position.x += (730.0f - Position.x)* 0.2f;
			if(Position.x < 730.5f){
				CountResetAndLoopCheck();
			}
				break;
		case ActorAct.OutStage:
			if(Position.x >= DefaultScreen.Width)
				CountResetAndLoopCheck();
			break;
		case ActorAct.Laugh:
			Position.y -= (((MaxFrame/2))-count);
			if(Position.y > 50.0f)
				Position.y = 50.0f;
			if(MaxFrame == count)
				CountResetAndLoopCheck();
			break;
		case ActorAct.Angry:
			Position.y -= (((MaxFrame/2))-count);
			if(Position.y > 50.0f)
				Position.y = 50.0f;
			if(MaxFrame < count)
				CountResetAndLoopCheck();
			break;
		case ActorAct.Panic:
			Position.x += (((MaxFrame/2)+1)-count);
			if(MaxFrame < count)
				CountResetAndLoopCheck();
			break;
		default:
			break;
		}
	}
	
	void CountResetAndLoopCheck(){
		count = 0;
		if(LoopFlg == false){
			CharaAct = ActorAct.Idle;
			IdleFlg = true;
//			Debug.Log("IdleFlg is true");
		}
	}
	
	//Change UV by Emotion
	public void ChangeEmotionUV(){
		switch(CharaEmo_Sub){
		case Emotion.Normal:
			ActorHead.SetUV(new Vector2(0.5f,0.0f),0.25f,1.0f);
			ActorBody.SetUV(new Vector2(0.0f,0.0f),0.5f,1.0f);
			//Debug.Log("MasterEmotion is Normal.");
			break;
		case Emotion.Grad:
			ActorHead.SetUV(new Vector2(0.25f,0.0f),0.25f,1.0f);
			ActorBody.SetUV(new Vector2(0.0f,0.0f),0.5f,1.0f);
			SetMaxFrame(20);
			//Debug.Log("MasterEmotion is Grad.");
			break;
		case Emotion.Angry:
			ActorHead.SetUV(new Vector2(0.0f,0.0f),0.25f,1.0f);
			ActorBody.SetUV(new Vector2(0.0f,0.0f),0.5f,1.0f);
			SetMaxFrame(10);
			//Debug.Log("MasterEmotion is Angry.");
			break;
		case Emotion.Panic:
			ActorHead.SetUV(new Vector2(0.75f,0.0f),0.25f,1.0f);
			ActorBody.SetUV(new Vector2(0.5f,0.0f),0.5f,1.0f);
			//Debug.Log("MasterEmotion is Panic.");
			SetMaxFrame(10);
			break;
		}
	}
	
	//Setter of Action
	public void SetAction(int sAct){
		switch(sAct){
		case 0:
			CharaAct_Sub = ActorAct.OnStage;
			break;
		case 1:
			CharaAct_Sub = ActorAct.OutStage;
			break;
		case 2:
			CharaAct_Sub = ActorAct.Laugh;
			break;
		case 3:
			CharaAct_Sub = ActorAct.Angry;
			break;
		case 4:
			CharaAct_Sub = ActorAct.Panic;
			break;
		default:
			break;
		}
	}
	//Getter of Action
	public ActorAct GetAction(){
		return CharaAct;
	}
	
	//Setter of Emotion
	public void SetEmotion(int sEmo){
		switch(sEmo){
		case 0:
			CharaEmo_Sub = Emotion.Normal;
			break;
		case 1:
			CharaEmo_Sub = Emotion.Grad;
			break;
		case 2:
			CharaEmo_Sub = Emotion.Angry;
			break;
		case 3:
			CharaEmo_Sub = Emotion.Panic;
			break;
		default:
			break;
		}
	}
	//Getter of Emotion
	public Emotion GetEmotion(){
		return CharaEmo;
	}
	
	//Setter of MaxFrame
	public void SetMaxFrame(int max){
		MaxFrame_Sub = max;
	}
	//Getter of MaxFrame
	public int GetMaxFrame(){
		return MaxFrame;
	}
	
	//Setter of Loopflg
	public void SetLoopFlg(bool Loop){
		LoopFlg = Loop;
	}
	//Getter of LoopFlg
	public bool GetLoopFlg(){
		return LoopFlg;
	}
	
	//Setter of IdleFlg
	public void SetIdleFlg(bool Idle){
		IdleFlg = Idle;
	}
	//Getter of IdleFlg
	public bool GetIdleFlg(){
		return IdleFlg;
	}
	
}
