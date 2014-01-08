using UnityEngine;
using System.Collections;

public enum ADVState{
	WAIT,
	PLAY,
	STOP,
	END
};

public class GUIManagerScript : MonoBehaviour {
	
	MasterActScript 		MasterManager;
	TextBoxScript			TextBox;
	TextWindowScript 		TextWin;
	InfoWindowManager		InfoWin;
	ActorAct 				Act;
	bool SceneStartFlg 	= false;
	bool ActFlg 		= false;
	bool IdleFlg 		= false;
	bool StartFlg 		= false;
	bool initialized	= false;
	//Receive Data By TextBoxScript
	int 		NumStatement;
	int[]		TextWindowArray;
	int[]		TextWindowSizeArray;
	int[] 		ActionTypeArray;
	int[]		EmotionTypeArray;
	int[]		InfoWindowFlg;
	int[]		InfoWindowContain;
	//Counter for WAIT
	int 		SceneCounter = 0;
	//State of ADV
	ADVState 	advstate;
	//ActDataIndex
	int 		ActIndexCnt;
	
	
	// Use this for initialization
	void Start () {
		//Get MasterActScript of GUIManager
		MasterManager 	= ((GameObject)GameObject.Find("GUIManager")).GetComponent<MasterActScript>();
		TextWin 		= ((GameObject)GameObject.Find("GUIManager")).GetComponent<TextWindowScript>();
		TextBox 		= ((GameObject)GameObject.Find("TextBox")).GetComponent<TextBoxScript>();	
		InfoWin			= ((GameObject)GameObject.Find("InfoWindowManager")).GetComponent<InfoWindowManager>();
	}
	
	//初期化関数(チュートリアル用に処理を分けてます) 
	void Initialize()
	{
		//Get Information//
		NumStatement 		= TextBox.GetNumStatement();
		TextWindowArray 	= TextBox.GetBalloonArray();
		TextWindowSizeArray = TextBox.GetBalloonSizeArray();
		ActionTypeArray		= TextBox.GetActionArray();
		EmotionTypeArray	= TextBox.GetEmotionArray();
		InfoWindowFlg		= TextBox.GetInfoWinInsertFlg();
		InfoWindowContain	= TextBox.GetInfoWinContainFlg();
		
		///////////////////
		ActIndexCnt = 0;
		
		initialized = true;
	}
	
	
	//Use This for Reflesh
	void Refresh(){
		//Refreshing
//		Debug.Log (FileOpe.GetADVStateNum());
	}
	
	// Update is called once per frame
	void Update () {
		
	bool Idle;
	switch(advstate){
			
		case ADVState.WAIT:
			//Wait
			if(SceneCounter != 40){
				SceneCounter++;
			}
			//Action
			else{
				//MasterActScript: SetAction()-SetEmotion-()-SetIdleFlg()
				MasterManager.SetAction(0);
				MasterManager.SetEmotion(0);
				if(ActFlg == false){
					MasterManager.SetIdleFlg(false);
					ActFlg = true;
				}else{
					IdleFlg = MasterManager.GetIdleFlg();
					if(IdleFlg==true){
						Initialize();
						advstate = ADVState.PLAY;
					}
				}
			}
			break;
		case ADVState.PLAY:
			//if(TextWin.GetWindowState()==0){
			switch(TextWin.GetWindowState()){
			case 0:
			if(ActIndexCnt!=NumStatement){
				if(MasterManager.GetIdleFlg()==true){
					//master
					MasterManager.SetAction(ActionTypeArray[ActIndexCnt]);
					MasterManager.SetEmotion(EmotionTypeArray[ActIndexCnt]);
					MasterManager.SetLoopFlg(true);
					MasterManager.SetIdleFlg(false);
					//testwindow
					TextWin.SetWindowType(TextWindowArray[ActIndexCnt]);
					TextWin.SetWindowSize(TextWindowSizeArray[ActIndexCnt]);
					TextWin.SetWindowState(1);
					//InfoWindow
					if(InfoWindowFlg[ActIndexCnt] != 0){
						InfoWin.SetWindowContent(InfoWindowContain[ActIndexCnt]);
						InfoWin.SetWindowState(1);
					}
				}
			}
				break;
			case 1:
				TextBox.SetTextInsertFlg(true);
				break;
			case 2:
				if(TextBox.GetTextInsertFlg()==false){
					MasterManager.SetLoopFlg(false);
					MasterManager.SetIdleFlg(true);
					TextWin.SetWindowState(3);
					InfoWin.SetWindowState(3);
					ActIndexCnt++;
				}
				break;
			case 3:
					TextWin.SetWindowState(3);
					InfoWin.SetWindowState(3);
				break;
			default:
				break;
			}
			break;
		case ADVState.STOP:
			
			break;
			
		case ADVState.END:
		
			break;
		}
	}
	
	public void Stop()
	{
		advstate = ADVState.STOP;
		MasterManager.SetIdleFlg(true);
		MasterManager.SetLoopFlg(false);
		MasterManager.ChangeAction(1);//シショーを画面外に
		TextWin.SetWindowState(3);
		InfoWin.SetWindowState(3);
	}
	public void Resume()
	{
		if(advstate == ADVState.STOP)
		{
			if(!initialized)
				Initialize();
			advstate = ADVState.PLAY;
			//SceneCounter = 39;
			MasterManager.ChangeAction(0);//シショーを画面内に
		}
	}
}
