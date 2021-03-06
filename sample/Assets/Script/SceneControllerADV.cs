﻿using UnityEngine;
using System.Collections;

public class SceneControllerADV : MonoBehaviour {
	
	TextBoxScript StextBox;
	bool press;
	
	bool initialized = false;
	
	Tex2DBaseScript shutterLeft;
	Tex2DBaseScript shutterRight;
	SyuCyuSenController SyutyuSen;
	int timer;
	int id;
	
	float screenWidth;
	float screenHeight;
	float shutterFlg;
	float shutterPos;
	float masterPos;
	int   dummyLoadCnt;//演出用
	bool toTitle = false;
	

	// Use this for initialization
	void Start () {
		StextBox = ((GameObject)GameObject.Find("TextBox")).GetComponent<TextBoxScript>();
	
		SyutyuSen = ((GameObject)GameObject.Find("SyuChooSen")).GetComponent<SyuCyuSenController>();
		
		press = false;
		dummyLoadCnt = 0;
		//マウスカーソル
		//Screen.lockCursor = true;
		Screen.showCursor = false;
		
		//画面サイズ取得
		screenWidth  = 1024.0f;//Screen.width;
		screenHeight = 768.0f;//Screen.height;
		
		//シャッターの初期設定
		shutterLeft = ((GameObject)GameObject.Find("ShutterLeft")).GetComponent<Tex2DBaseScript>();
		shutterLeft.SetSize(screenWidth/2.0f, screenHeight);
		shutterLeft.SetPos(0.0f, 0.0f);
		
		shutterRight = ((GameObject)GameObject.Find("ShutterRight")).GetComponent<Tex2DBaseScript>();
		shutterRight.SetSize(screenWidth/2.0f, screenHeight);
		shutterRight.SetPos(screenWidth/2.0f, 0.0f);
		
		shutterPos = screenWidth/2.0f;
	
	}
	
	void Initialize()
	{
		StextBox.Validate();
		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!initialized)
			Initialize();
		//遷移
		if(dummyLoadCnt > 0)
			dummyLoadCnt++;
		if(dummyLoadCnt > 10)
		{
			if(toTitle)
				Application.LoadLevel("Title");
			else{
				GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
				Application.LoadLevel(gamesys.GetNextScene());
				gamesys.SystemOutPut(StextBox.GetNextAct());
			}
		}
		//シャッターの移動
		if(press)
		{
			shutterPos += ((screenWidth/2.0f)-shutterPos)*0.2f;
			//遷移までのカウンタ設定
			if(shutterPos >= screenWidth/2.01f && dummyLoadCnt <= 0){
				dummyLoadCnt = 1;
			}
		}
		else
		{
			shutterPos += (0.0f-shutterPos)*0.2f;
		}
		if(shutterPos <= 0.05f){
			//shutter open!!
			
		}
		//シャッターの初期位置設定
		shutterLeft.SetPos((-(screenWidth/2.0f)+shutterPos), 0.0f);
		shutterRight.SetPos((screenWidth)-shutterPos, 0.0f);
		
		if(	!StextBox.isValid() ||
			Input.GetKey(KeyCode.N))//Input.GetMouseButtonDown(1))
			press = true;
		
		if (Input.GetKey(KeyCode.A))
    	{
    	    SyutyuSen.SyuCyuSenSwitch(true);
		}
		if (Input.GetKey(KeyCode.S))
    	{
    	    SyutyuSen.SyuCyuSenSwitch(false);
    	}
		if(!press && Input.GetKey(KeyCode.R))
		{
			press = true;
			toTitle = true;
		}

	}
	
	
}
