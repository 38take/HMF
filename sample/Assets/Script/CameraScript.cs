using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	LineManagerScript		SLineManager;
	PlayerScript			SPlayerScript;
	private	Vector3			TargetPos, StartPos, LastPos, RelayPos;
	private	float			timer;
	private	bool			StartCameraFlag;
	public	GameObject		obj_TestObject;
	
	int				roteTime = 0;
	Quaternion		BaseRota, Rota;
	
	// Use this for initialization
	void Start () {
		SLineManager	= (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");
		SPlayerScript	= (PlayerScript)GameObject.Find("ナイフ5").GetComponent("PlayerScript");
		
		StartCameraFlag = false;
		timer = 0;
	}

	// Update is called once per frame
	void Update () {

		switch( SPlayerScript.m_GameState )
		{
		// ゲームプレイまでのカメラ操作
		case PlayerScript.GAME_STATE.GAME_START:
			if( PointInterpolationSlerp(new Vector3(0,500,-150), StartPos, 120) >= 0.8 && !StartCameraFlag )
			{
				StartCameraFlag = true;
				RelayPos = transform.position;
				timer = 0;
			}
			else if( StartCameraFlag )
			{
				if( PointInterpolationSlerp(RelayPos, StartPos, 120) >= 1.0 )
				{
					SPlayerScript.m_GameState = PlayerScript.GAME_STATE.GAME_PLAY;
					timer = 0;
				}
				roteTime++;
				
				transform.LookAt(GameObject.Find("ナイフ5").transform.position);
			}
			break;
			
		// ゲームプレイ中のカメラ操作	
		case PlayerScript.GAME_STATE.GAME_PLAY:
			// 刃の座標を求める
			TargetPos = SLineManager.CalcPlayerPos(SPlayerScript.m_Timer, 0);
			
			// 現在の刃の座標から数秒前の位置にカメラをセットする
			transform.position = SLineManager.CalcPlayerPos(SPlayerScript.m_Timer-60, 0);
			transform.position = new Vector3(	transform.position.x,
												transform.position.y+3.0f,
												transform.position.z );
			transform.LookAt(TargetPos);
			
			break;

		// ゲーム終了時のカメラ操作
		case PlayerScript.GAME_STATE.GAME_END:
			if( PointInterpolationSlerp(LastPos, new Vector3(0,500,-100), 120 ) >= 0.75 )
			{
//				Destroy( GameObject.Find("DeadLine(Clone)") );
//				Destroy( GameObject.Find("Line(Clone)") );
//				Destroy( GameObject.Find("target(Clone)") );
//				Destroy( GameObject.Find("Floor") );
				
				// 完成型オブジェクトを生成
				Instantiate(obj_TestObject);
				
				SPlayerScript.m_GameState = PlayerScript.GAME_STATE.GAME_COMPLETION;
				timer = 0;
			}
			break;
			
		}
	}

	// 直線地点補間
	public float	PointInterpolationLerp( Vector3 start_pos, Vector3 end_pos, float MOVE_TIME )
	{
		timer++;
		
		transform.LookAt(GameObject.Find("ナイフ5").transform.position);
		transform.position = Vector3.Lerp( start_pos, end_pos, timer/MOVE_TIME );
		
		return timer/MOVE_TIME;
	}

	// 曲線地点補間
	public float	PointInterpolationSlerp( Vector3 start_pos, Vector3 end_pos, float MOVE_TIME )
	{
		timer++;
		
		transform.LookAt(GameObject.Find("ナイフ5").transform.position);
		transform.position = Vector3.Slerp( start_pos, end_pos, timer/MOVE_TIME );
		
		return timer/MOVE_TIME;
	}
	
	public void SetStartCameraPos( Vector3 pos ){	StartPos = pos; }
	public void SetLastCameraPos( Vector3 pos )	{	LastPos = pos; }
}
