using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	ComboScript				SCombo;
	LineManagerScript		SLineManager;
	CameraScript			SCamera;
	private float			oldMouseX;
	private float			height;
	private float			HEIGHT;
	private int				holdHorizontal;
	private	bool			InitFlag = false;
	private int				concentration;
	private float 			timerAdd;
	private float 			TIMERADD_MAX;
	private float 			TIMERADD_MIN;

	public	GameObject		obj_Particle;
	public  GameObject		obj_CMonitor;
	public	float			m_Offset;
	public	int				m_Timer;
	
	public enum GAME_STATE
	{
		GAME_START,				// ゲーム開始前のカメラ操作状態
		GAME_PLAY,				// ゲームプレイ中の状態
		GAME_END,				// ゲームクリア時のカメラ操作状態
		GAME_COMPLETION,		// 次の状態遷移可能
	}
	
	public	GAME_STATE		m_GameState = GAME_STATE.GAME_START;
	
 	
	// Use this for initialization
	void Start () {
		// 他スクリプトの関数を参照可能にする
		SScore			= (ScoreScript)GameObject.Find("ScoreTextBox").GetComponent("ScoreScript");
		SCombo			= (ComboScript)GameObject.Find("ComboRenderer").GetComponent("ComboScript");
		SLineManager	= (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");
		SCamera			= (CameraScript)GameObject.Find("Main Camera").GetComponent("CameraScript");
		obj_CMonitor    = (GameObject)GameObject.Find("ConcentrateMonitor");
		// FPSを60に設定
		Application.targetFrameRate = 60;
		// 初期位置格納
		oldMouseX = Input.mousePosition.x;
		
		HEIGHT		= 8.0f;
		height		= 0.0f;
		
		timerAdd = 0.0f;
		TIMERADD_MAX = 4.0f;
		TIMERADD_MIN = 1.0f;
		
		m_Timer		= 0;
		m_Offset	= 0.0f;
		
		holdHorizontal = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( m_GameState == GAME_STATE.GAME_PLAY || !InitFlag )
		{
			if( !InitFlag )
			{
				Vector3 startPos = SLineManager.CalcPos(m_Timer-60, 0);
				startPos = new Vector3( startPos.x, startPos.y+3.0f, startPos.z );
				SCamera.SetStartCameraPos( startPos );
				InitFlag = true;
			}

			// デバッグ用
			if(Input.GetKeyDown(KeyCode.Space))
			{
				Vector3 lastPos = SLineManager.CalcPos(m_Timer-60, 0);
				lastPos = new Vector3( lastPos.x, lastPos.y+3.0f, lastPos.z );
				SCamera.SetLastCameraPos( lastPos );
				m_GameState = GAME_STATE.GAME_END;
			}

			float	transX = (Input.mousePosition.x - oldMouseX) * 0.01f;
			
			if(	Input.GetMouseButton(1) &&
				concentration > 0)
			{
				timerAdd += (TIMERADD_MIN - timerAdd) * 0.1f;
				concentration--;
			}
			else
				timerAdd += (TIMERADD_MAX - timerAdd) * 0.1f;
			
			m_Timer += (int)timerAdd;
			obj_CMonitor.guiText.text = concentration.ToString();
			
			//------------------------------------//
			//移動
			if(holdHorizontal <= 0)
				m_Offset += transX;
			else
			{
				holdHorizontal--;
				Debug.Log(holdHorizontal);
			}
			if(m_Offset > 1.0f) m_Offset = 1.0f;
			if(m_Offset < -1.0f) m_Offset = -1.0f;
			Vector3 basePos = SLineManager.CalcPosWithHitCheck(m_Timer, m_Offset);
			if(Input.GetMouseButton(0))
				height += (HEIGHT-height)*0.1f;
			else
				height += (0.0f-height)*0.1f;
			transform.position = new Vector3(basePos.x, basePos.y+height, basePos.z);
			oldMouseX = Input.mousePosition.x;
			//------------------------------------//
			//回転処理
			//向くべき方向を算出
			int inLineID = SLineManager.GetPlyaerLineID();
			Vector3 dir = SLineManager.GetLineDirection(inLineID+1)-SLineManager.GetLineDirection(inLineID);
			dir = Vector3.Normalize(dir);
			
	        // モデルのデフォルト向きによって基準ベクトルは任意調整
			Vector3 vecDefault = new Vector3(1.0f,0.0f,0.0f);
		
		        // 0~360の値が欲しいので２倍
			float rad = Mathf.Atan2(dir.x-vecDefault.x,dir.z-vecDefault.z)*2.0f;
			float deg = rad * Mathf.Rad2Deg -180.0f;
			
		    //一度角度をリセットしてから回転する
			transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		    // X軸基準に回す
			transform.Rotate(310.0f, deg,0);
			
			if( SLineManager.isLastpoint() )
			{
				Vector3 lastPos = SLineManager.CalcPos(m_Timer-60, 0);
				lastPos = new Vector3( lastPos.x, lastPos.y+3.0f, lastPos.z );
				SCamera.SetLastCameraPos( lastPos );
				m_GameState = GAME_STATE.GAME_END;
			}
			
		}
	}
	//切るかミスるかしたときに呼ばれる
	public void CalcCombo(bool hitFlg)
	{
		SCombo.Notice(hitFlg);
	}
	//切るかミスるかしたときに呼ばれる
	public void CalcScore(int score)
	{
		SScore.AddScore(score);
	}
	
	public void CalcConcentration(int Value)
	{
		concentration += Value;
		concentration %= 100;
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "target(Clone)")
		{
			Destroy(collision.gameObject);
			//パーティクル発生
			GameObject ins_obj =
				(GameObject)Instantiate(obj_Particle,
										collision.gameObject.transform.position,
										collision.gameObject.transform.rotation);
			Destroy(ins_obj, 1.0f);
			//SCombo.Notice(true);
		}
		if(collision.gameObject.name == "DeadLine(Clone)")
		{
			holdHorizontal = 120;
		}
	}
}
