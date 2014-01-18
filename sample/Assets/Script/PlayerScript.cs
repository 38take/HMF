using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	ComboScript				SCombo;
	LineManagerScript		SLineManager;
	CameraScript			SCamera;
	ConcentrateScript		ConcentrateGauge;
	CutEdgeManagerScript    SCutEdge;
	TextBoxScript			SText;
	ReadyActorScript		SReady;
	SyuCyuSenController		SyuCyuSen;
	private float			oldMouseX;
	private float			height;
	public  float			UPHeight;
	private bool			cutting;
	public float			MouseSensitivity = 0.001f;
	private int				upCnt;
	private int				holdHorizontal;
	public  int				HoldTime = 120;
	private	bool			InitFlag = false;
	private	int				timer_comp;
	
	private float 			timerAdd;
	public  float 			TIMERADD_MAX;
	public  float 			TIMERADD_MIN;
	
	private	GameObject		Effect_Frame;
	private	GameObject		Effect_CutDust;
	public	GameObject		obj_ClearObject_0;
	public	GameObject		obj_ClearObject_1;
	public	GameObject		obj_ClearObject_2;
	public	GameObject		obj_ClearObject_3;
	
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
	
	public bool				MoveByAngle = false;
	public float			MaxAngle = 45.0f;
	public float			MaxSlideSpeed = 1.0f;
	private float			angle = 0.0f;
	
	public bool 			ExistSpeed = false;
	public float			MaxSpeed = 1.0f;
	public float			MinSpeed = 0.1f;
	private float			speed;
	private float			speedValue;
	
	public float			c_angle = 0.0f;		// クリア後カメラ動作用_ラジアン値

	
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
		SText			= ((GameObject)GameObject.Find("TextBox")).GetComponent<TextBoxScript>();
		SReady			= ((GameObject)GameObject.Find("ReadyActor")).GetComponent<ReadyActorScript>();
		SyuCyuSen		= ((GameObject)GameObject.Find("SyuCyuSen")).GetComponent<SyuCyuSenController>();
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
		
		speed = 1.0f;
		speedValue = 1.0f;
		
		holdHorizontal = 0;
		
		numBomb = 0;

		Effect_Frame = GameObject.Find("EffectScreanFrame");
		Effect_Frame.SetActiveRecursively(false);
		Effect_CutDust = GameObject.Find("ナイフ5/EffectCutDust");
		Effect_CutDust.SetActiveRecursively(true);
	}
	
	// Update is called once per frame
	void Update () {
		
		if( m_GameState == GAME_STATE.GAME_PLAY || !InitFlag)
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
			if(stop)
			{
				if(!SText.isValid())
					stop = false;
			}
			else
			{
				//------------------------------------//
				//集中モード処理
				float addValue;
				if(Input.GetKey(KeyCode.C))
					CalcConcentration(1);
				if(	Input.GetMouseButton(1) &&
					ConcentrateGauge.isExist())
				{
					//Effect_Frame.SetActiveRecursively(true);
					timerAdd += (TIMERADD_MIN - timerAdd) * 0.1f;
					ConcentrateGauge.AddConcentrate(-0.1f);
					SyuCyuSen.SyuCyuSenSwitch(true);
				}
				else
				{
					timerAdd += (TIMERADD_MAX - timerAdd) * 0.1f;
					//Effect_Frame.SetActiveRecursively(false);
					SyuCyuSen.SyuCyuSenSwitch(false);
				}
				addValue = timerAdd;
				if(ExistSpeed)
				{
					speed += (speedValue - speed) * 0.1f;
					addValue *= speed;
				}
				//カウントが終わっていればタイマーを加算 
				if(!SReady.isValid())
				{
					m_Timer += addValue;
				}
				
				//------------------------------------//
				//移動
				float	transX = (Input.mousePosition.x - oldMouseX) * MouseSensitivity;
				//ペナルティ食らってなければ移動
				if(holdHorizontal <= 0)
				{
					angle += transX*50.0f;
					if(angle > MaxAngle) angle = MaxAngle;
					if(angle <-MaxAngle) angle = -MaxAngle;
					//角度で制御するか判定
					if(MoveByAngle)
						m_Offset += MaxSlideSpeed * (angle / MaxAngle);
					else
						m_Offset += transX;
				}
				else if(holdHorizontal > 0)
				{
					holdHorizontal--;
					Debug.Log(holdHorizontal);
				}
				if(m_Offset > 1.0f) m_Offset = 1.0f;
				if(m_Offset < -1.0f) m_Offset = -1.0f;
				Vector3 basePos = SLineManager.CalcPosWithHitCheck((int)m_Timer, m_Offset);
				if(!Input.GetMouseButton(0))
					height += (UPHeight-height)*0.1f;
				else
					height += (0.0f-height)*0.1f;
				
				//刃が上がってるか判定
				bool tmpCutting = ( height <= 0.5f );
				if(cutting && !tmpCutting && SCutEdge!=null)
				{
					SCutEdge.StopEdgeOpen();
					SCutEdge = null;
				}
				if(!cutting && tmpCutting)
					SCutEdge = ((GameObject)Instantiate(obj_Edge)).GetComponent<CutEdgeManagerScript>();
				cutting = tmpCutting;
				Effect_CutDust.SetActiveRecursively(cutting);//エフェクト
				
				basePos = new Vector3(basePos.x, basePos.y+height, basePos.z);
				transform.position = basePos;
				oldMouseX = Input.mousePosition.x;
				//------------------------------------//
				//切り口生成
				if( CreateCutEdge && cutting &&((int)(m_Timer - m_TimerPrev)) > CreateEdgeSpan)
				{
					SCutEdge.AddPoint(SLineManager.CalcPos((int)m_Timer, m_Offset));
					m_TimerPrev = m_Timer;
				}
				//タイマー加算時に切っている間はスコア加減算
				if(cutting && !SReady.isValid())
				{
					if(SLineManager.isDeadLine())	SScore.AddScore(0.0f);
					else 							SScore.AddScore(0.1f);
				}
			}
			//------------------------------------//
			//回転処理
			//向くべき方向を算出
			int inLineID = SLineManager.GetPlyaerLineID();
			Vector3 dir = SLineManager.GetLineStartPoint(inLineID+1)-SLineManager.GetLineStartPoint(inLineID);
			dir = Vector3.Normalize(dir);
			
	        // モデルのデフォルト向きによって基準ベクトルは任意調整
			Vector3 vecDefault = new Vector3(1.0f,0.0f,0.0f);
		
		        // 0~360の値が欲しいので２倍
			float rad = Mathf.Atan2(dir.x-vecDefault.x,dir.z-vecDefault.z)*2.0f;
			float deg = rad * Mathf.Rad2Deg -180.0f;
			
		    //一度角度をリセットしてから回転する
			transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		    // X軸基準に回す
			transform.Rotate(0, deg, 0);
			transform.Rotate(angle, 0, 0);
			transform.Rotate(0, 0, 10.0f + (MaxAngle-10.0f)*(speed / MaxSpeed) );
			
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
			// 光のエフェクト発生
			if( timer_comp == 0 )
			{	
				GameObject.Find("EffectGauge").SetActiveRecursively(false);
				
				GameObject ins_obj =
					(GameObject)Instantiate(effect_Clear);
				Destroy(ins_obj, 2.0f);
			}
			// 完成型オブジェクトを生成
			if( timer_comp == 60 )
			{
				int stageID;
				GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();

				stageID = 1;
				switch(gamesys.GetActID())
				{
				case 1:			stageID = 0; 	Instantiate(obj_ClearObject_0);	break;	//チュートリアル
				case 2:			stageID = 1;	Instantiate(obj_ClearObject_1); SText.Invalidate();	break;	//ステージ１
				case 7:			stageID = 2;	Instantiate(obj_ClearObject_2); SText.Invalidate();	break;	//ステージ２
				case 12:		stageID = 3;	Instantiate(obj_ClearObject_3); SText.Invalidate();	break;	//ステージ３
				default:		stageID = 1;	SText.Invalidate();	break;
				}
			}
			if(timer_comp == 90)
			{
				ResultRendererScript SResult = ((GameObject)GameObject.Find("ResultRenderer")).GetComponent<ResultRendererScript>();
				SResult.Validate();				
			}
			if(timer_comp > 60)
			{
				Vector3 pos =  new Vector3(0.0f, 0.0f, 0.0f);

				// オブジェクトの周りをカメラが円運動する
				GameObject.Find("CameraMain").transform.position =
					new Vector3( pos.x + Mathf.Cos(c_angle) * 200,
								 pos.y + 200,
								 pos.z + Mathf.Sin(c_angle) * 200 );
				
				GameObject.Find("CameraMain").transform.LookAt(pos);
				
				c_angle += 0.01f;
			}
			timer_comp++;
		}
	}
	//コンボ計算
	public void CalcCombo(bool hitFlg)
	{
		SCombo.Notice(hitFlg);
		SCamera.CalcDistance(hitFlg);
		//速度計算
		if(hitFlg)
			speedValue += (MaxSpeed - (MaxSpeed - speedValue)) * 0.01f;
		else 
			speedValue = MinSpeed;
		if(speedValue > MaxSpeed) speedValue = MaxSpeed;
	}
	//スコア計算
	public void CalcScore(int score)
	{
		SScore.AddScore((float)score);
	}
	//集中力計算
	public void CalcConcentration(int Value)
	{
		if(ConcentrateGauge.AddConcentrate((float)Value)) 
		{
			CalcScore(2000);
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
	//赤いとこ切ってる通知
	public void CutDeadLine()
	{
		if(height < 0.5f)
			holdHorizontal = HoldTime;
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "target(Clone)")
		{
			Destroy(collision.gameObject);
		}
	}
	
	public bool isCutting()
	{
		return cutting;
	}
}
