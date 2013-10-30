using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	LineManagerScript		SLineManager;
	PlayerScript			SPlayerScript;
	private	Vector3			TargetPos;
	
	// Use this for initialization
	void Start () {
		SLineManager	= (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");
		SPlayerScript	= (PlayerScript)GameObject.Find("ナイフ5").GetComponent("PlayerScript");		
		transform.position = new Vector3(0,10,0);
	}
	
	// Update is called once per frame
	void Update () {

		TargetPos = SLineManager.CalcPlayerPos(SPlayerScript.m_Timer, 0);
		
		// 現在の刃の位置から数秒前の位置にカメラをセットする
		transform.position = SLineManager.CalcPlayerPos(SPlayerScript.m_Timer-90, 0);
		transform.position = new Vector3(	transform.position.x,
											transform.position.y+5,
											transform.position.z );
		transform.LookAt(TargetPos);
	}
}
