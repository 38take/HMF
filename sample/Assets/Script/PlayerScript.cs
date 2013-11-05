using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	LineManagerScript		SLineManager;
	private float			oldMouseX;
	private float			height;
	private float			HEIGHT;
	private int				holdHorizontal;

	public	GameObject		obj_Particle;
	
	public	bool			m_CameraFlag = false;
	public	float			m_Offset;
	public	int				m_Timer;
 	
	// Use this for initialization
	void Start () {
		// 他スクリプトの関数を参照可能にする
		SScore = (ScoreScript)GameObject.Find("ScoreTextBox").GetComponent("ScoreScript");		
		SLineManager = (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");		
		// FPSを60に設定
		Application.targetFrameRate = 60;
		// 初期位置格納
		transform.position = new Vector3(0.0f,10.0f,0.0f);
		oldMouseX = Input.mousePosition.x;

		HEIGHT = 8.0f;
		height = 0.0f;
		
		m_Timer = 0;
		m_Offset = 0.0f;
		
		holdHorizontal = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if( m_CameraFlag )
		{
			float	transX = (Input.mousePosition.x - oldMouseX) * 0.01f;
			m_Timer+=4;
			
			//------------------------------------//
			//移動
			if(holdHorizontal <= 0)
				m_Offset += transX;
			else
				holdHorizontal--;
			Debug.Log(holdHorizontal);
			if(m_Offset > 1.0f) m_Offset = 1.0f;
			if(m_Offset < -1.0f) m_Offset = -1.0f;
			Vector3 basePos = SLineManager.CalcPlayerPos(m_Timer, m_Offset);
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
		}
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "target(Clone)")
		{
			SScore.AddScore(100);
			Destroy(collision.gameObject);
			
			GameObject ins_obj =
				(GameObject)Instantiate(obj_Particle,
										collision.gameObject.transform.position,
										collision.gameObject.transform.rotation);
			Destroy(ins_obj, 1.0f);
		}
		else if(collision.gameObject.name == "DeadLine(Clone)")
		{
			holdHorizontal = 120;
		}
	}
}
