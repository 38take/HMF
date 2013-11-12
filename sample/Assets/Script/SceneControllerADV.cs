using UnityEngine;
using System.Collections;

public class SceneControllerADV : MonoBehaviour {
	
	TextBoxScript StextBox;
	bool press;
	Tex2DBaseScript shutterLeft;
	Tex2DBaseScript shutterRight;
	float screenWidth;
	float screenHeight;
	float shutterPos;
	int   dummyLoadCnt;//演出用
	
	// Use this for initialization
	void Start () {
		StextBox = ((GameObject)GameObject.Find("TextBox")).GetComponent<TextBoxScript>();
		
		press = false;
		dummyLoadCnt = 0;
		
		//画面サイズ取得
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		//シャッターの初期設定
		shutterLeft = ((GameObject)GameObject.Find("ShutterLeft")).GetComponent<Tex2DBaseScript>();
		shutterLeft.SetSize(screenWidth/2.0f, screenHeight);
		shutterLeft.SetPos(0.0f, 0.0f);
		
		shutterRight = ((GameObject)GameObject.Find("ShutterRight")).GetComponent<Tex2DBaseScript>();
		shutterRight.SetSize(screenWidth/2.0f, screenHeight);
		shutterRight.SetPos(screenWidth/2.0f, 0.0f);
		
		shutterPos = screenWidth/2.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//遷移
		if(dummyLoadCnt > 0)
			dummyLoadCnt++;
		if(dummyLoadCnt > 10)
		{
			GameSystemScript gameSystem = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			if(gameSystem.isLastAct())
			{
				Application.LoadLevel("Result");
				gameSystem.SystemOutPut(1);
			}
			else
			{
				Application.LoadLevel("Play");
				gameSystem.SystemOutPut(1);
			}
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
		
		if(!StextBox.isValid())
			press = true;
	}
}
