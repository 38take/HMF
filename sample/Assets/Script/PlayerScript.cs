using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	ComboScript				SCombo;
	LineManagerScript		SLineManager;
	CameraScript			SCamera;
	ParticleSystem			ConcentrateGauge;
	private float			oldMouseX;
	private float			height;
	private float			HEIGHT;
	private int				holdHorizontal;
	private	bool			InitFlag = false;
	private float			concentration;
	private float			concentrationGauge;
	private float 			CONCENTRATION_MAX;
	private int				concentrationMoveCnt;
	private int				CONCENTRATION_MOVECNT;
	
	private float 			timerAdd;
	private float 			TIMERADD_MAX;
	private float 			TIMERADD_MIN;

	public	GameObject		obj_Particle;
	public  GameObject		obj_CMonitor;
	public	GameObject		obj_Bomb;
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
		ConcentrateGauge= ((GameObject)GameObject.Find("GaugeRenderer")).GetComponent<ParticleSystem>();
		// FPSを60に設定
		Application.targetFrameRate = 60;
		// 初期位置格納
		oldMouseX = Input.mousePosition.x;
		
		HEIGHT		= 8.0f;
		height		= 0.0f;
		
		timerAdd = 0.0f;
		TIMERADD_MAX = 4.0f;
		TIMERADD_MIN = 1.0f;
		
		concentration = 0.0f;
		concentrationGauge = 0.0f;
		concentrationMoveCnt = 0;
		CONCENTRATION_MOVECNT = 30;
		CONCENTRATION_MAX = 100.0f;
		
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

			//------------------------------------//
			//集中力ゲージ処理
			if( concentration != concentrationGauge )
			{
				if(concentrationMoveCnt > 0)
					concentrationMoveCnt--;
				else if(concentrationMoveCnt == 0)
				{
					concentrationGauge += (concentration - concentrationGauge)*0.2f;
					if(concentration == concentrationGauge)
						concentrationMoveCnt = -1;
				}
				else
					concentrationMoveCnt = CONCENTRATION_MOVECNT;
			}
			ConcentrateGauge.startLifetime = (concentration / CONCENTRATION_MAX)*20.0f;
			//集中モード処理
			if(	Input.GetMouseButton(1) &&
				concentration > 0)
			{
				timerAdd += (TIMERADD_MIN - timerAdd) * 0.1f;
				concentration-=1.0f;
				if(concentration < 0.0f)concentration = 0.0f;
			}
			else
				timerAdd += (TIMERADD_MAX - timerAdd) * 0.1f;
			
			m_Timer += (int)timerAdd;
			obj_CMonitor.guiText.text = ((int)concentration).ToString() + ":" + ((int)concentrationGauge).ToString();
			
			//------------------------------------//
			//移動
			float	transX = (Input.mousePosition.x - oldMouseX) * 0.01f;
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
	//コンボ計算
	public void CalcCombo(bool hitFlg)
	{
		SCombo.Notice(hitFlg);
	}
	//スコア計算
	public void CalcScore(int score)
	{
		SScore.AddScore(score);
	}
	//集中力計算
	public void CalcConcentration(int Value)
	{
		concentration += (float)Value;
		if(concentration >= CONCENTRATION_MAX) 
		{
			concentration = CONCENTRATION_MAX;
			
			GameObject obj = (GameObject)Instantiate(obj_Bomb, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			concentration = 0;
		}
		if(concentration != concentrationGauge)
			concentrationMoveCnt = CONCENTRATION_MOVECNT;
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
		}
		if(collision.gameObject.name == "DeadLine(Clone)")
		{
			holdHorizontal = 120;
		}
	}
}
