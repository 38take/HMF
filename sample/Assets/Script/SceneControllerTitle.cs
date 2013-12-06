using UnityEngine;
using System.Collections;

public class SceneControllerTitle : MonoBehaviour {

	bool press;
	Tex2DBaseScript shutterLeft;
	Tex2DBaseScript shutterRight;
	Tex2DBaseScript SBG_Atelier;
	Tex2DBaseScript SButtonStart;
	float screenWidth;
	float screenHeight;
	float shutterPos;
	Vector2 BGPos;
	int   dummyLoadCnt;//演出用
	
	// Use this for initialization
	void Start () {
		press = false;
		dummyLoadCnt = 0;
		
		//画面サイズ取得
		screenWidth = DefaultScreen.Width;//Screen.width;
		screenHeight = DefaultScreen.Height;//Screen.height;
		
		//シャッターの初期設定
		shutterLeft = ((GameObject)GameObject.Find("ShutterLeft")).GetComponent<Tex2DBaseScript>();
		shutterLeft.SetSize(screenWidth/2.0f, screenHeight);
		shutterLeft.SetPos(0.0f, 0.0f);
		shutterLeft.DrawEnable = true;
		
		shutterRight = ((GameObject)GameObject.Find("ShutterRight")).GetComponent<Tex2DBaseScript>();
		shutterRight.SetSize(screenWidth/2.0f, screenHeight);
		shutterRight.SetPos(screenWidth/2.0f, 0.0f);
		shutterRight.DrawEnable = true;
		
		shutterPos = screenWidth/2.0f;
		//shutterPos = 0.0f;
		
		//スタートボタンの設定
		SButtonStart = ((GameObject)GameObject.Find("StartButton")).GetComponent<Tex2DBaseScript>();
		SButtonStart.DrawEnable = true;
		
		//背景の設定
		//SBG_Atelier = ((GameObject)GameObject.Find("BG_Atelier")).GetComponent<Tex2DBaseScript>();
		//SBG_Atelier.SetSize(1536.0f*(screenWidth/1024.0f), 1280.0f*(screenHeight/768.0f));
		//BGPos = new Vector2(0.0f, 0.0f);
		//SBG_Atelier.SetPos(BGPos);
		//SBG_Atelier.DrawEnable = true;
	}
	
	// Update is called once per frame
	void Update () {
		//遷移
		if(dummyLoadCnt > 0)
			dummyLoadCnt++;
		if(dummyLoadCnt > 10)
		{
			GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
			Application.LoadLevel("Adventure");
			gamesys.SystemOutPut(-gamesys.GetActID());
		}
		//背景のスクロール
		//BGPos.x -= 3.0f;
		//if(BGPos.x <= -256.0f)	BGPos.x = 0.0f;
		//BGPos.y -= 3.0f;
		//if(BGPos.y <= -256.0f)	BGPos.y = 0.0f;
		//SBG_Atelier.SetPos(BGPos);
		
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
			//shutterPos += (0.0f-shutterPos)*0.2f;
		}
		//シャッターの初期位置設定
		shutterLeft.SetPos((-(screenWidth/2.0f)+shutterPos), 0.0f);
		shutterRight.SetPos((screenWidth)-shutterPos, 0.0f);
		//マウスクリック判定
		if(!press && Input.GetMouseButton(0))
			press = true;
	}
}
