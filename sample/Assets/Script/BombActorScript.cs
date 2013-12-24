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
	GameObject objR;
	GameObject objL;
	
	
	//切り抜きスタンプ
	public	GameObject	obj_Stamp;
	int stampCntLeft;
	int	stampCntRight;
	//エフェクト
	public	GameObject	effect_Cut;
	int effectCntLeft;
	int effectCntRight;
	
	//パラメータ
	int timer;
	float timerAdd;
	float TIMERADD_MAX;
	// Use this for initialization
	void Start () {
		SLineManager = ((GameObject)GameObject.Find("LineManager")).GetComponent<LineManagerScript>();
		SPlayer		 = ((GameObject)GameObject.Find("ナイフ5")).GetComponent<PlayerScript>();
	//	objR = (GameObject)Instantiate(obj_RightEffect, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
	//	objL = (GameObject)Instantiate(obj_LeftEffect, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
	//	SRightEffect = objR.GetComponent<BillboardScript>();
	//	SLeftEffect = objL.GetComponent<BillboardScript>();
		//パラメータ初期化
		timer 			= 0;
		timerAdd 	 	= 1.0f;
		TIMERADD_MAX 	= 5.0f;
		
		//乱数初期化
		stampCntLeft 	= 0;
		stampCntRight 	= 0;
		effectCntLeft 	= Random.Range(10, 20);
		effectCntRight 	= Random.Range(10, 20);
		
		//カメラ揺らす
		CameraScript SCamera = ((GameObject)GameObject.Find("CameraMain")).GetComponent<CameraScript>();
		SCamera.SetShakeFlag();
	}
	
	// Update is called once per frame
	void Update () {
		//タイマー計算
		timerAdd += (TIMERADD_MAX-timerAdd) * 0.001f;
		timer += (int)timerAdd;
		//位置・横向きベクトル算出
		Vector3 bombPos = SLineManager.CalcPos((int)SPlayer.m_Timer+timer, 0.0f);
		Vector3 bombDir = SLineManager.CalcHorizontalDir((int)SPlayer.m_Timer+timer);
		float lineWidth = SLineManager.GetLineWidth();
		
		//パーティクルセット
	//	SRightEffect.SetPosition(bombPos+(bombDir*lineWidth));
	//	SLeftEffect.SetPosition(bombPos-(bombDir*lineWidth));
		
		//スタンプを張り付ける
		GameObject objSR;
		GameObject objSL;
		if(stampCntRight > 0)
			stampCntRight--;
		else
		{ 
			objSR = (GameObject)Instantiate(obj_Stamp, bombPos+(bombDir*lineWidth), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			stampCntRight = Random.Range(10, 20);
		}
		if(stampCntLeft > 0)
			stampCntLeft--;
		else
		{ 
			objSL = (GameObject)Instantiate(obj_Stamp, bombPos-(bombDir*lineWidth), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			stampCntLeft = Random.Range(10, 20);
		}
		//エフェクトを生成する
		GameObject objER;
		GameObject objEL;
		if(effectCntRight > 0)
			effectCntRight--;
		else
		{ 
			objER = (GameObject)Instantiate(effect_Cut, bombPos+(bombDir*lineWidth), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			effectCntRight = Random.Range(10, 20);
			Destroy(objER, 1.0f);
		}
		if(effectCntLeft > 0)
			effectCntLeft--;
		else
		{ 
			objEL = (GameObject)Instantiate(effect_Cut, bombPos-(bombDir*lineWidth), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			effectCntLeft = Random.Range(10, 20);
			Destroy(objEL, 1.0f);
		}
		
		//タイマーが規定値を超えるとDestroy
		if(timer > 200)
		{
			DestroyObject(objR);
			DestroyObject(objL);
			DestroyObject(gameObject);
		}
	}
}
