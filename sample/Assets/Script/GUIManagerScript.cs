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
	ActorAct Act;
	bool SceneStartFlg 	= false;
	bool ActFlg 		= false;
	bool IdleFlg 		= false;
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
						TextString 			= TextBox.GetStrArray();
						TextWindowArray 	= TextBox.GetBalloonArray();
						TextWindowSizeArray = TextBox.GetBalloonSizeArray();
						///////////////////
						/*Debug.Log("AAAAAAAAA:"+NumStatement);
						for(int i = 0;i<NumStatement;i++){
							Debug.Log("TextString["+i+"]:"+TextString[i]);
						}*/
					
						advstate = ADVState.PLAY;
					}
				}
			}
			break;
		case ADVState.PLAY:
			if(TextWin.GetWindowState()==0){
				switch(TextWin.GetWindowState()){
				case 0:
					TextWin.SetWindowState(1);
					TextWin.SetWindowSize(2);
					MasterManager.SetAction(1);
					MasterManager.SetEmotion(1);
					MasterManager.SetLoopFlg(true);
					MasterManager.SetIdleFlg(false);
				Debug.Log("Press S Key.SetEmo angry");
					break;
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				default:
					break;
				}
			}else if(TextWin.GetWindowState()==2){
				TextBox.SetTextInsertFlg(true);
			}
			break;
		case ADVState.END:
			
			break;
		}
		
	}
	
	
}
