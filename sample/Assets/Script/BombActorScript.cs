using UnityEngine;
using System.Collections;

public class BombActorScript : MonoBehaviour {
	
	//外部オブジェクト
	LineManagerScript SLineManager;
	PlayerScript	  SPlayer;
	
	//エフェクト
	public	GameObject	obj_RightEffect;
	public	GameObject	obj_LeftEffect;
	BillboardScript SRightEffect;
	BillboardScript SLeftEffect;
	
	//切り抜きスタンプ
	public	GameObject	obj_Stamp;
	int stampCntLeft;
	int	stampCntRight;
	
	//パラメータ
	int timer;
	float timerAdd;
	float TIMERADD_MAX;
	// Use this for initialization
	void Start () {
		SLineManager = ((GameObject)GameObject.Find("LineManager")).GetComponent<LineManagerScript>();
		SPlayer		 = ((GameObject)GameObject.Find("ナイフ5")).GetComponent<PlayerScript>();
		GameObject objR = (GameObject)Instantiate(obj_RightEffect, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		GameObject objL = (GameObject)Instantiate(obj_LeftEffect, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		SRightEffect = objR.GetComponent<BillboardScript>();
		SLeftEffect = objL.GetComponent<BillboardScript>();
		//パラメータ初期化
		timer 			= 0;
		timerAdd 	 	= 1.0f;
		TIMERADD_MAX 	= 5.0f;
		
		//乱数初期化
		stampCntLeft = 0;
		stampCntRight = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//タイマー計算
		timerAdd += (TIMERADD_MAX-timerAdd) * 0.001f;
		timer += (int)timerAdd;
		//位置・横向きベクトル算出
		Vector3 bombPos = SLineManager.CalcPos(SPlayer.m_Timer+timer, 0.0f);
		Vector3 bombDir = SLineManager.CalcHorizontalDir(SPlayer.m_Timer+timer);
		float lineWidth = SLineManager.GetLineWidth();
		
		//パーティクルセット
		SRightEffect.SetPosition(bombPos+(bombDir*lineWidth));
		SLeftEffect.SetPosition(bombPos-(bombDir*lineWidth));
		
		//スタンプを張り付ける
		GameObject obj;
		if(stampCntRight > 0)
			stampCntRight--;
		else
		{ 
			obj = (GameObject)Instantiate(obj_Stamp, bombPos+(bombDir*lineWidth), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			stampCntRight = Random.Range(100, 150);
		}
		if(stampCntLeft > 0)
			stampCntLeft--;
		else
		{ 
			obj = (GameObject)Instantiate(obj_Stamp, bombPos-(bombDir*lineWidth), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			stampCntLeft = Random.Range(100, 150);
		}
	}
}
