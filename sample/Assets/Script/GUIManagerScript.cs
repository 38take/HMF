using UnityEngine;
using System.Collections;

public enum ADVState{
	WAIT,
	PLAY,
	END
};

public class GUIManagerScript : MonoBehaviour {
	
	MasterActScript 	MasterManager;
	TextBoxScript		TextBox;
	TextWindowScript 	TextWin;
	ActorAct 			Act;
	bool SceneStartFlg 	= false;
	bool ActFlg 		= false;
	bool IdleFlg 		= false;
	bool StartFlg 		= false;
	//Receive Data By TextBoxScript
	int 		NumStatement;
	int[]		TextWindowArray;
	int[]		TextWindowSizeArray;
	int[] 		ActionTypeArray;
	int[]		EmotionTypeArray;
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
	}
	
	//Use This for Reflesh
	void Refresh(){
		//Refreshing
		
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
						//Get Information//
						NumStatement 		= TextBox.GetNumStatement();
						TextWindowArray 	= TextBox.GetBalloonArray();
						TextWindowSizeArray = TextBox.GetBalloonSizeArray();
						ActionTypeArray		= TextBox.GetActionArray();
						EmotionTypeArray	= TextBox.GetEmotionArray();
						
						//for Debug
		
						///////////////////
						ActIndexCnt = 0;
						
						advstate = ADVState.PLAY;
					}
				}
			}
			break;
			case ADVState.PLAY:
				//if(TextWin.GetWindowState()==0){
				switch(TextWin.GetWindowState()){
				case 0:
				//Debug.Log ("aaaaa");
				//Debug.Log("NumStateMent:"+NumStatement);
				if(ActIndexCnt!=NumStatement){
					if(MasterManager.GetIdleFlg()==true){
						//Set Information
						//Debug.Log("ActIndex:"+ActIndexCnt);
						//Debug.Log("ActionTypeArray["+ActIndexCnt+"]:"+ActionTypeArray[ActIndexCnt]);
						//Debug.Log("EmotionTypeArray["+ActIndexCnt+"]:"+EmotionTypeArray[ActIndexCnt]);
						//Debug.Log("TextWindowArray["+ActIndexCnt+"]:"+TextWindowArray[ActIndexCnt]);
						//Debug.Log("TextWindowSizeArray["+ActIndexCnt+"]:"+TextWindowSizeArray[ActIndexCnt]);
						//master
						MasterManager.SetAction(ActionTypeArray[ActIndexCnt]);
						MasterManager.SetEmotion(EmotionTypeArray[ActIndexCnt]);
						MasterManager.SetLoopFlg(true);
						MasterManager.SetIdleFlg(false);
						//testwindow
						TextWin.SetWindowType(TextWindowArray[ActIndexCnt]);
						TextWin.SetWindowSize(TextWindowSizeArray[ActIndexCnt]);
						TextWin.SetWindowState(1);
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
						ActIndexCnt++;
					}
					break;
				case 3:
					break;
				default:
					break;
				}
				break;
			case ADVState.END:
			
				break;
			}
		}
		
	//}
	
	
}
