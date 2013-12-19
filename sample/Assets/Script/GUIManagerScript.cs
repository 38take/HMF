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
	string[] 	TextString;
	//Counter for WAIT
	int 		SceneCounter = 0;
	//State of ADV
	ADVState 	advstate;
	
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
//		if (StartFlg==true){
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
							TextString 			= TextBox.GetStrArray();
							TextWindowArray 	= TextBox.GetBalloonArray();
							TextWindowSizeArray = TextBox.GetBalloonSizeArray();
							///////////////////
					
							advstate = ADVState.PLAY;
						}
					}
				}
				break;
				case ADVState.PLAY:
					//if(TextWin.GetWindowState()==0){
					switch(TextWin.GetWindowState()){
					case 0:
						//Set Information
						TextWin.SetWindowState(1);
						TextWin.SetWindowSize(1);
						MasterManager.SetAction(0);
						MasterManager.SetEmotion(3);
						MasterManager.SetLoopFlg(true);
						MasterManager.SetIdleFlg(false);
						break;
					case 1:
						break;
					case 2:
						if(TextBox.GetTextInsertFlg()==false){
					
						}
						break;
					case 3:
						break;
					default:
						break;
					}
				//}else if(TextWin.GetWindowState()==2){
				//	TextBox.SetTextInsertFlg(true);
				//}
				break;
			case ADVState.END:
			
				break;
			}
		}
		
	//}
	
	
}
