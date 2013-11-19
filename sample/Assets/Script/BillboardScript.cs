using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {
	
	 // ゲームオブジェクトをビルボード化させる対象のカメラ
	public Camera m_Camera;
	// true の場合ゲームオブジェクトはクリッピング平面のカメラの前に配置されます。
	public bool PositionInFrontOfCamera;
	// PositionInFrontOfCamera が true の場合のオブジェクトのオフセット幅
	public float Offset = 0.001f;

	// Use this for initialization
	void Start () {
	}
	
	void Awake(){
		// カメラが指定されてない場合はメインカメラを使用
		if (m_Camera == null) m_Camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update (){
		// カメラの forward ベクトルを取得して正規化
		var vec = m_Camera.transform.forward;
		vec.Normalize();
		// ゲームオブジェクトのポジションをカメラのクリッピング平面のすぐ内側にセットしてカメラビューをブロックするようにする
		if (this.PositionInFrontOfCamera)
			this.transform.position =
				m_Camera.transform.position + (vec * (m_Camera.nearClipPlane + this.Offset));
		// ゲームオブジェクトの向きがカメラの方へ向くようにする
		this.transform.LookAt(this.transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
	}
}
