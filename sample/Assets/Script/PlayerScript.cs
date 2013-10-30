using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	LineManagerScript		SLineManager;
	private	Vector3			oldPos;
	private float			oldMouseX;
	private float			Speed;
	private float			RightTime,LeftTime;
	private	float			TransX_Limit = 10;
	private float			TransX_LimitTime = 5;
//	private bool			InitFall = false;
	
	public	GameObject		obj_Particle;
	
	public	float			m_Offset;
	public	int				m_Timer;
 	
	// Use this for initialization
	void Start () {
		// 他スクリプトの関数を参照可能にする
		SScore = (ScoreScript)GameObject.Find("ScoreTextBox").GetComponent("ScoreScript");		
		SLineManager = (LineManagerScript)GameObject.Find("LineManager").GetComponent("LineManagerScript");		
		// FPSを60に設定
		Application.targetFrameRate = 60;
		// FPS * 制限移動秒数
		RightTime = LeftTime = Application.targetFrameRate * TransX_LimitTime;
		// 初期位置格納
		transform.position = new Vector3(0.0f,10.0f,0.0f);
		oldMouseX = Input.mousePosition.x;
		Speed = 7.5f;
		
		m_Timer = 0;
		m_Offset = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		float	transX = (Input.mousePosition.x - oldMouseX) * 0.01f;
		m_Timer++;
/*
		float	transX = 0;
		float	mouseX = Input.mousePosition.x - oldMouseX;
		
		if( Mathf.Abs(mouseX) <= TransX_Limit )
		{
			transX = mouseX * 2.0f;
		}
		else
		{
			if( Mathf.Sin(mouseX) < 0 )
				transX =  TransX_Limit * 2.0f;
			else
				transX = -TransX_Limit * 2.0f;
		}
*/
		m_Offset += transX;
		if(m_Offset > 1.0f) m_Offset = 1.0f;
		if(m_Offset < -1.0f) m_Offset = -1.0f;
		// 右方向に移動する時
		//if( transX < 0 )
		//{
		//	if(offset)
		//	//if( RightTime != 0 )
		//	//{
		//	//	rigidbody.velocity =
		//	//		new Vector3(transX, rigidbody.velocity.y, rigidbody.velocity.z);
		//	//
		//	//	RightTime	--;
		//	//	LeftTime	++;
		//	//}
		//}
		// 左方向に移動する時
		//else if( transX > 0 )
		//{
		//	
		//	if( LeftTime != 0 )
		//	{
		//		rigidbody.velocity =
		//			new Vector3(transX, rigidbody.velocity.y, rigidbody.velocity.z);
		//		
		//		RightTime	++;
		//		LeftTime	--;
		//	}
		//}
		
		//if( LeftTime == 0 || RightTime == 0 )
		//	rigidbody.velocity =
		//		new Vector3(0, rigidbody.velocity.y, Speed);
		//else
		//	rigidbody.velocity =
		//		new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, Speed);

		
/*		
		if( InitFall )
			rigidbody.velocity = new Vector3(transX, rigidbody.velocity.y, Speed);

		// 下方向にレイを飛ばす
		if( Physics.Raycast(transform.position,Vector3.down,5) )
		{
			oldPos = transform.position;
			
			if( !InitFall )
				InitFall = true;
		}
		else
		{
			if( InitFall )
			{
				transform.position = oldPos;
			}
		}
*/		
		
		//if(transform.position.z >= 15.0f)
		//{
		//	RightTime = LeftTime = Application.targetFrameRate * TransX_LimitTime;
		//	transform.position = new Vector3(	0,
		//										transform.position.y,
		//										-30.0f);
		//}
		transform.position =SLineManager.CalcPlayerPos(m_Timer, m_Offset);
		oldMouseX = Input.mousePosition.x;
	
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
	}
}
