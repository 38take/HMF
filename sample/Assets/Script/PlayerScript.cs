using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	ComboScript				SCombo;
	LineManagerScript		SLineManager;
	CameraScript			SCamera;
	ConcentrateScript		ConcentrateGauge;
	CutEdgeManagerScript    SCutEdge;
	private float			oldMouseX;
	private float			height;
	public  float			UPHeight;
	private int				upCnt;
	private int				holdHorizontal;
	private	bool			InitFlag = false;
	private	int				timer_comp;
	
	private float 			timerAdd;
	public  float 			TIMERADD_MAX;
	public  float 			TIMERADD_MIN;
	
	public	GameObject		obj_ClearObject;
	public	GameObject		obj_Bomb;
	public	GameObject		effect_Clear;
	public GameObject		obj_Edge;

	public	float			m_Offset;
	private	float			m_OffsetPrev;
	private	float			m_OffsetBottom;
	public	float			m_Timer;
	private float			m_TimerPrev;
	private float			m_TimerBottom;
	public  int				CreateEdgeSpan;
	//private CutEdgeScript	SCutEdge;
	public  bool			CreateCutEdge;
	private bool			stop = false;
	
	//リザルト用パラメータ
	int numBomb;
	
	public enum GAME_STATE
	{
		GAME_START,				// ゲーム開始前のカメラ操作状態
		GAME_PLAY,				// ゲームプレイ中の状態
		GAME_END,				// ゲームクリア時のカメラ操作状態
		GAME_COMPLETION,		// 次の状態遷移可能
	}
	public enum ACHIEVEMENT_FLAG
	{
		
		NUM_ACHIEVEMENT
	};
	
	public	GAME_STATE		m_GameState = GAME_STATE.GAME_START;
	
 	
	// Use this for initialization
	void Start () {
		// 他スクリプトの関数を参照可能にする
		SScore			= (ScoreScript)GameObject.Find("ScoreTextBox").GetComponent("ScoreScript");
		SCombo			= (ComboScript)GameObject.Find("ComboManager").GetComponent("ComboScript");
		SLineManager	= (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");
		SCamera			= (CameraScript)GameObject.Find("CameraMain").GetComponent("CameraScript");
		ConcentrateGauge= ((GameObject)GameObject.Find("Gauge")).GetComponent<ConcentrateScript>();
		SCutEdge		= ((GameObject)GameObject.Find("CutEdgeManager")).GetComponent<CutEdgeManagerScript>();
		// FPSを60に設定
		Application.targetFrameRate = 60;
		// 初期位置格納
		oldMouseX = Input.mousePosition.x;
		
		//UPHeight	= 8.0f;
		height		= 0.0f;
		upCnt       = 0;
		
		timerAdd = 0.0f;
		//TIMERADD_MAX = 4.0f;
		//TIMERADD_MIN = 1.0f;
		timer_comp = 0;
		
		m_Timer		= 0.0f;
		m_TimerPrev = m_TimerBottom = 0.0f;
		m_Offset	= 0.0f;
		
		holdHorizontal = 0;
		
		numBomb = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( m_GameState == GAME_STATE.GAME_PLAY || !InitFlag )
		{
			if( !InitFlag )
			{
				Vector3 startPos = SLineManager.CalcPos((int)m_Timer-60, 0);
				startPos = new Vector3( startPos.x, startPos.y+3.0f, startPos.z );
				SCamera.SetStartCameraPos( startPos );
				InitFlag = true;
			}

			// デバッグ用
			if(Input.GetKeyDown(KeyCode.F10))
			{
				Vector3 lastPos = SLineManager.CalcPos((int)m_Timer-60, 0);
				lastPos = new Vector3( lastPos.x, lastPos.y+3.0f, lastPos.z );
				SCamera.SetLastCameraPos( lastPos );
				m_GameState = GAME_STATE.GAME_END;
			}
			//
			if(stop)
			{
				if(Input.GetMouseButtonUp(0))
					stop = false;
			}
			else
			{
				//------------------------------------//
				//集中モード処理
				if(	Input.GetMouseButton(1) &&
					ConcentrateGauge.isExist())
				{
					timerAdd += (TIMERADD_MIN - timerAdd) * 0.1f;
					ConcentrateGauge.AddConcentrate(-1.0f);
				}
				else
					timerAdd += (TIMERADD_MAX - timerAdd) * 0.1f;
				
				m_Timer += timerAdd;
				
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
				Vector3 basePos = SLineManager.CalcPosWithHitCheck((int)m_Timer, m_Offset);
				if(Input.GetMouseButton(0))
					height += (UPHeight-height)*0.1f;
				else
					height += (0.0f-height)*0.1f;
				basePos = new Vector3(basePos.x, basePos.y+height, basePos.z);
				transform.position = basePos;
				oldMouseX = Input.mousePosition.x;
				//------------------------------------//
				//切り口生成
				if( CreateCutEdge && ((int)(m_Timer - m_TimerPrev)) > CreateEdgeSpan)
				{
					SCutEdge.AddPoint(SLineManager.CalcPos((int)m_Timer, m_Offset));
					m_TimerPrev = m_Timer;
				}
			}
			//------------------------------------//
			//回転処理
			//向くべき方向を算出
	//		int inLineID = SLineManager.GetPlyaerLineID();
	//		Vector3 dir = SLineManager.GetLineDirection(inLineID+1)-SLineManager.GetLineDirection(inLineID);
	//		dir = Vector3.Normalize(dir);
	//		
	//        // モデルのデフォルト向きによって基準ベクトルは任意調整
	//		Vector3 vecDefault = new Vector3(1.0f,0.0f,0.0f);
	//	
	//	        // 0~360の値が欲しいので２倍
	//		float rad = Mathf.Atan2(dir.x-vecDefault.x,dir.z-vecDefault.z)*2.0f;
	//		float deg = rad * Mathf.Rad2Deg -180.0f;
	//		
	//	    //一度角度をリセットしてから回転する
	//		transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
	//	    // X軸基準に回す
	//		//transform.Rotate(310.0f, deg,0);
	//		transform.Rotate(90.0f, deg,0);
	//		
			//------------------------------------//
			//終了判定
			if( SLineManager.isLastpoint() )
			{
				Vector3 lastPos = SLineManager.CalcPos((int)m_Timer-60, 0);
				lastPos = new Vector3( lastPos.x, lastPos.y+3.0f, lastPos.z );
				SCamera.SetLastCameraPos( lastPos );
				m_GameState = GAME_STATE.GAME_END;
				
			}
			
		}
		else if( m_GameState == GAME_STATE.GAME_COMPLETION )
		{
			// 完成型オブジェクトを生成
			if( timer_comp == 0 )
			{
				GameObject ins_obj =
					(GameObject)Instantiate(effect_Clear,
											GameObject.Find("Floor").transform.position,
											GameObject.Find("Floor").transform.rotation	);
				Destroy(ins_obj, 2.0f);
			}
			if( timer_comp == 60 )
			{
				Instantiate(obj_ClearObject);
			}
			if(timer_comp == 90)
			{
				ResultRendererScript SResult = ((GameObject)GameObject.Find("ResultRenderer")).GetComponent<ResultRendererScript>();
				SResult.Validate();
			}
			timer_comp++;
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
		if(ConcentrateGauge.AddConcentrate((float)Value)) 
		{
			GameObject obj = (GameObject)Instantiate(obj_Bomb, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			ConcentrateGauge.AddConcentrate(-ConcentrateGauge.GetConcentration());
			numBomb++;
		}
	}
	
	//ゲッタ
	public int GetNumBomb(){	return numBomb;	}
	public int GetNumUp(){	return upCnt;	}
	//停止・移動
	public void Stop()	{	stop = true;	}
	public void Move()	{	stop = false;	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "target(Clone)")
		{
			Destroy(collision.gameObject);
		}
		if(collision.gameObject.name == "DeadLine(Clone)")
		{
			holdHorizontal = 120;
		}
	}
}
