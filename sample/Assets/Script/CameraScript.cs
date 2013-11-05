using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	LineManagerScript		SLineManager;
	PlayerScript			SPlayerScript;
	private	Vector3			TargetPos;
	private	float			timer = 0;
	
	// Use this for initialization
	void Start () {
		SLineManager	= (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");
		SPlayerScript	= (PlayerScript)GameObject.Find("ナイフ5").GetComponent("PlayerScript");		
		transform.position = new Vector3(0,1000,0);
		transform.LookAt(GameObject.Find("Floor").transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
		if( SPlayerScript.m_CameraFlag )
		{
			TargetPos = SLineManager.CalcPlayerPos(SPlayerScript.m_Timer, 0);
			
			// 現在の刃の位置から数秒前の位置にカメラをセットする
			transform.position = SLineManager.CalcPlayerPos(SPlayerScript.m_Timer-60, 0);
			transform.position = new Vector3(	transform.position.x,
												transform.position.y+3.0f,
												transform.position.z );
			transform.LookAt(TargetPos);
		}
		else
		{
			Vector3 start_pos	= new Vector3(1000,1000,0);
			Vector3 end_pos		= new Vector3(0,3,0);
			float	SLERP_TIME	= 180;

			if( timer/SLERP_TIME < 1 )
				// カメラ位置の球面線形
				transform.position = Vector3.Slerp(	start_pos, end_pos, timer/SLERP_TIME );
			else
				SPlayerScript.m_CameraFlag = true;

			timer++;
		}
	}
}
