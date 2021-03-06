﻿using UnityEngine;
using System.Collections;

public class SceneControllerResult : MonoBehaviour {
	
	bool press;
	Tex2DBaseScript shutterLeft;
	Tex2DBaseScript shutterRight;
	Tex2DGUITextureScript SResultBG;
	TotalResultScript STotalResult;
	float screenWidth;
	float screenHeight;
	float shutterPos;
	int   dummyLoadCnt;//演出用
	
	// Use this for initialization
	void Start () {
		press = false;
		dummyLoadCnt = 0;
		//マウスカーソル
		Screen.lockCursor = true;
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
		
		//リザルトへのアクセス準備
		STotalResult = ((GameObject)GameObject.Find("TotalResult")).GetComponent<TotalResultScript>();
		SResultBG    = ((GameObject)GameObject.Find("ResultBG")).GetComponent<Tex2DGUITextureScript>();
		
		SResultBG.SwitchTexture(0);
		SResultBG.SetRenderFlag(true);
		SResultBG.RestoreTextureRect();
	}
	
	// Update is called once per frame
	void Update () {
		//遷移
		if(dummyLoadCnt > 0)
			dummyLoadCnt++;
		if(dummyLoadCnt > 10)
		{
			GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			Application.LoadLevel("Title");
			gamesys.SystemOutPut(-gamesys.GetActID());
		}
		//シャッターの移動
		if(press)
		{
			shutterPos += ((screenWidth/2.0f)-shutterPos)*0.2f;
			//遷移までのカウンタ設定
			if(shutterPos >= screenWidth/2.01f && dummyLoadCnt <= 0)
				dummyLoadCnt = 1;
		}
		else
		{
			shutterPos += (0.0f-shutterPos)*0.2f;
		}
		//シャッターの初期位置設定
		shutterLeft.SetPos((-(screenWidth/2.0f)+shutterPos), 0.0f);
		shutterRight.SetPos((screenWidth)-shutterPos, 0.0f);
		//マウスクリック判定
		if(STotalResult.isEnd() && !press && Input.GetMouseButton(0) || Input.GetKey(KeyCode.R))
			press = true;
	}
}
