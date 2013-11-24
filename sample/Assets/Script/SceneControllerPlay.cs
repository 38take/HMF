using UnityEngine;
using System.Collections;

public class SceneControllerPlay : MonoBehaviour {
	
	bool press;
	Tex2DBaseScript shutterLeft;
	Tex2DBaseScript shutterRight;
	ScoreScript		SScore;
	LineManagerScript SLineManager;
	float screenWidth;
	float screenHeight;
	float shutterPos;
	int   dummyLoadCnt;//演出用
	
	// Use this for initialization
	void Start () {
		press = false;
		dummyLoadCnt = 0;
		
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
	}
	
	// Update is called once per frame
	void Update () {
		//遷移
		if(dummyLoadCnt > 0)
			dummyLoadCnt++;
		if(dummyLoadCnt > 10)
		{
			//スコアから分岐先を算出
			//ターゲット数取得
			int targetNum = SLineManager.GetNumTarget();
			int maxScore = targetNum * 100;//今はこれで
			int score = (int)SScore.GetScore();
			//とりあえず3分割してどこに分類されるかで
			int nextAct;
			if(score < (maxScore/3))
				nextAct = 1;
			else if(score < ((maxScore/3)*2))
				nextAct = 2;
			else 
				nextAct = 3;
			
			GameSystemScript gameSystem = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			if(gameSystem.isLastAct())
			{
				Application.LoadLevel("Result");
				gameSystem.SystemOutPut(nextAct);
			}
			else
			{
				Application.LoadLevel("Adventure");
				gameSystem.SystemOutPut(nextAct);
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
		
		if( Input.GetMouseButtonDown(1) ||
			SLineManager.isLastpoint() && Input.GetMouseButtonDown(0))
		{
			press = true;
		}
	}
}
