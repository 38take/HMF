using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	LineManagerScript		SLineManager;
	PlayerScript			SPlayerScript;
	private	Vector3			TargetPos, StartPos, LastPos, RelayPos;
	private	float			timer;
	private	bool			StartCameraFlag;
	private	Quaternion		StartRota, EndRota;
	private	float			Rota_timer;
	private float			offset;
	public 	float 			MaxDistance = 90.0f;
	public 	float 			MinDistance = 30.0f;
	private float           distanceValue = 70.0f;
	private float			distance = 70.0f;
	
	Vector3	shake;
	bool	shakeFlg;
	int		shakeDistance;
	int		SHAKEDISTANCE;
	float	shakeWidth;
	float	lineWidth;
	
	public bool followCamera;
	
	// Use this for initialization
	void Start () {
		SLineManager	= (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");
		SPlayerScript	= (PlayerScript)GameObject.Find("ナイフ5").GetComponent("PlayerScript");
		
		StartCameraFlag = false;
		timer = 0;
		Rota_timer = 0;
		shake = new Vector3(0.0f, 0.0f, 0.0f);
		shakeFlg = false;
		shakeDistance = 0;
		SHAKEDISTANCE = 200;
		shakeWidth = 0.2f;
		offset = 0;
	}

	// Update is called once per frame
	void Update () {
		
		Vector3	workPos;
		
		switch( SPlayerScript.m_GameState )
		{
		// ゲームプレイまでのカメラ操作
		case PlayerScript.GAME_STATE.GAME_START:
			if( PointInterpolationSlerp(new Vector3(0,250,-200), StartPos, 120) >= 0.8 && !StartCameraFlag )
			{
				StartCameraFlag = true;
				StartRota	= transform.rotation;
				RelayPos	= transform.position;
				workPos		= transform.position;
								
				transform.position = SLineManager.CalcPos(((int)SPlayerScript.m_Timer)-(int)distance, 0);
				transform.position = new Vector3(	transform.position.x,
													transform.position.y+3.0f,
													transform.position.z );
				transform.LookAt(GameObject.Find("ナイフ5").transform.position);
				EndRota		= transform.rotation;
				transform.position = workPos;
				transform.LookAt(GameObject.Find("Floor").transform.position);
				timer = 0;
			}
			else if( StartCameraFlag )
			{
				if( PointInterpolationSlerp(RelayPos, StartPos, 120) >= 1.0 )
				{
					SPlayerScript.m_GameState = PlayerScript.GAME_STATE.GAME_PLAY;
					timer = 0;
					lineWidth = SLineManager.GetLineWidth();
				}
				
				Rota_timer++;
				transform.rotation = Quaternion.Slerp( StartRota, EndRota, Rota_timer/80 );
			}
			break;
			
		// ゲームプレイ中のカメラ操作	
		case PlayerScript.GAME_STATE.GAME_PLAY:
			//カメラのブレ計算
			shake.x = ((float)Random.Range(-shakeDistance, shakeDistance) / (float)SHAKEDISTANCE * shakeWidth)/lineWidth ;
			shake.y = ((float)Random.Range(-shakeDistance, shakeDistance) / (float)SHAKEDISTANCE * shakeWidth);
			if(shakeDistance > 0) shakeDistance--;
			
			//プレイヤーの左右移動に合わせて動く
			if(followCamera)
			{
				offset += ( SPlayerScript.m_Offset - offset )*0.2f;
				shake.x += offset;
			}
			// 刃の座標を求める
			TargetPos = SLineManager.CalcPos((int)SPlayerScript.m_Timer, shake.x);
			TargetPos = new Vector3(TargetPos.x, TargetPos.y+shake.y, TargetPos.z);
				
			// 現在の刃の座標から数秒前の位置にカメラをセットする
			distance += (distanceValue-distance) * 0.1f;
			transform.position = SLineManager.CalcPos(((int)SPlayerScript.m_Timer-(int)distance), shake.x);
			transform.position = new Vector3(	transform.position.x,
												transform.position.y+1.0f+(3.0f*(distance/MaxDistance))+shake.y,
												transform.position.z );
			//注視点の算出
			TargetPos = SLineManager.CalcPos((int)SPlayerScript.m_Timer+(int)(MaxDistance-distance), shake.x);
			transform.LookAt(TargetPos);
			
			break;

		// ゲーム終了時のカメラ操作
		case PlayerScript.GAME_STATE.GAME_END:
			if( PointInterpolationSlerp(LastPos, new Vector3(0,250,-200), 120 ) >= 1 )
			{
//				Destroy( GameObject.Find("DeadLine(Clone)") );
//				Destroy( GameObject.Find("Line(Clone)") );
//				Destroy( GameObject.Find("target(Clone)") );
				Destroy( GameObject.Find("Floor") );
				SLineManager.DestroyLine();
				SLineManager.DestroyTarget();
				
				SPlayerScript.m_GameState = PlayerScript.GAME_STATE.GAME_COMPLETION;
				
				timer = 0;
			}
			break;
		}
	}

	//Playerとの距離を計算
	public void CalcDistance(bool hit)
	{
		if(hit)
		{
			distanceValue += (MaxDistance - (MaxDistance-distanceValue)) * 0.1f;
			if(distanceValue > MaxDistance) distanceValue = MaxDistance;
		}
		else
		{
			distanceValue = MinDistance;
		}
	}
	// 直線地点補間
	public float	PointInterpolationLerp( Vector3 start_pos, Vector3 end_pos, float MOVE_TIME )
	{
		timer++;
		
		transform.LookAt(GameObject.Find("Floor").transform.position);
		transform.position = Vector3.Lerp( start_pos, end_pos, timer/MOVE_TIME );
		
		return timer/MOVE_TIME;
	}

	// 曲線地点補間
	public float	PointInterpolationSlerp( Vector3 start_pos, Vector3 end_pos, float MOVE_TIME )
	{
		timer++;
		
		transform.LookAt(GameObject.Find("Floor").transform.position);
		transform.position = Vector3.Slerp( start_pos, end_pos, timer/MOVE_TIME );
		
		return timer/MOVE_TIME;
	}
	
	public void SetStartCameraPos( Vector3 pos ){	StartPos = pos; }
	public void SetLastCameraPos( Vector3 pos )	{	LastPos = pos; }
	public void SetShakeFlag(){ shakeFlg = true; shakeDistance = SHAKEDISTANCE; }
}
