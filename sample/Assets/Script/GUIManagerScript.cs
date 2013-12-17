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
		TextWin 		= ((GameObject)GameObject.Find("TextWindow")).GetComponent<TextWindowScript>();
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
				MasterManager.SetAction(ActorAct.OnStage);
				MasterManager.SetEmotion(Emotion.Normal);
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
						Debug.Log("AAAAAAAAA:"+NumStatement);
						for(int i = 0;i<NumStatement;i++){
							Debug.Log("TextString["+i+"]:"+TextString[i]);
						}
					
						advstate = ADVState.PLAY;
					}
				}
			}
			break;
		case ADVState.PLAY:
			
			
			break;
		case ADVState.END:
			
			break;
		}
		
	
		//For Debug
		if(Input.GetKeyDown("a")){
			Debug.Log("Press A Key.LoopFlg is True");
			MasterManager.SetLoopFlg(true);
		}
		if(Input.GetKeyDown("q")){
			Debug.Log("Press Q Key.LoopFlg is False");
			MasterManager.SetLoopFlg(false);
		}
		if(Input.GetKeyDown("d")){
			MasterManager.SetAction(ActorAct.Angry);
			MasterManager.SetEmotion(Emotion.Angry);
			MasterManager.SetIdleFlg(false);
			Debug.Log("Press S Key.SetEmo angry");
		}
		if(Input.GetKeyDown("s")){
			MasterManager.SetAction(ActorAct.OnStage);
			MasterManager.SetEmotion(Emotion.Normal);
			MasterManager.SetIdleFlg(false);
			Debug.Log("Press D Key.SetAct Onstage");
		}
		if(Input.GetKeyDown("f")){
			MasterManager.SetAction(ActorAct.Laugh);
			MasterManager.SetEmotion(Emotion.Grad);
			MasterManager.SetIdleFlg(false);
			Debug.Log("Press D Key.SetEmo grad");
		}
		if(Input.GetKeyDown("g")){
			MasterManager.SetAction(ActorAct.Panic);
			MasterManager.SetEmotion(Emotion.Panic);
			MasterManager.SetIdleFlg(false);
			Debug.Log("Press D Key.SetEmo Panic");
		}
		
	}
	
	
	public void SceneStart(){
		SceneStartFlg = true;
	}
	
}
