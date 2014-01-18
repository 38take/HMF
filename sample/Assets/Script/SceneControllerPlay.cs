using UnityEngine;
using System.Collections;

public class SceneControllerPlay : MonoBehaviour {
	
	bool press;
	Tex2DBaseScript shutterLeft;
	Tex2DBaseScript shutterRight;
	ScoreScript		SScore;
	LineManagerScript SLineManager;
	ResultRendererScript SResultRenderer;
	float screenWidth;
	float screenHeight;
	float shutterPos;
	int   dummyLoadCnt;//演出用
	bool toTitle = false;
	
	// Use this for initialization
	void Start () {
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
		
		//スコアスクリプトへのアクセス準備
		SScore = ((GameObject)GameObject.Find("ScoreTextBox")).GetComponent<ScoreScript>();
		//ステージへのアクセス準備
		SLineManager = ((GameObject)GameObject.Find("LineManager")).GetComponent<LineManagerScript>();
		//結果表示スクリプト
		SResultRenderer = ((GameObject)GameObject.Find("ResultRenderer")).GetComponent<ResultRendererScript>();
	}
	
	// Update is called once per frame
	void Update () {
		//遷移
		if(dummyLoadCnt > 0)
			dummyLoadCnt++;
		if(dummyLoadCnt > 10)
		{
			if(toTitle)
				Application.LoadLevel("Title");
			else{
				int score = (int)SScore.GetScore();
				//とりあえず3分割してどこに分類されるかで
				int nextAct = 1 + SResultRenderer.GetNextAct();
				
				GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
				gamesys.SetScore(SLineManager.GetStageID()-1, score);
				if(gamesys.GetActID() == 1)
					nextAct = 1;
				Application.LoadLevel(gamesys.GetNextScene());
				gamesys.SystemOutPut(nextAct);
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
		
		if( Input.GetKeyUp(KeyCode.N) ||
			SResultRenderer.isEnd() && Input.GetMouseButtonDown(0))
		{
			press = true;
		}
		if(!press && Input.GetKey(KeyCode.R))
		{
			press = true;
			toTitle = true;
		}
	}
}
