using UnityEngine;
using System.Collections;

public class ComboRendererScript : MonoBehaviour {
	
	ArrayList	numArray;
	ArrayList   objArray;
	public GameObject	obj_NumTex;
	Tex2DGUITextureScript	SLogo;
	
	int digit;
	int m_Timer;
	
	Vector2 renderPos;
	
	// Use this for initialization
	void Start () {
	
	}
	private void Init(){
		objArray = new ArrayList();
		numArray = new ArrayList();
		SLogo = this.gameObject.GetComponentInChildren<Tex2DGUITextureScript>();
	}
	public void Release()
	{
		for(int i=0; i<objArray.Count; i++)
			DestroyObject((Object)objArray[i]);
		//自身を削除
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(m_Timer > 0)
		{
			m_Timer--;
		}
		else
		{
			Release();
		}
	}
	
	public void SetData(int combo, int timer, int width, int height, Vector3 playerPos)
	{
		Init();
		//位置計算
		Camera gameCamera = ((GameObject)GameObject.Find("CameraMain")).GetComponent<Camera>();
		Camera orthCamera = ((GameObject)GameObject.Find("Camera2D")).GetComponent<Camera>();
		//文字を発生させる座標（Base_UIPos）をPerspectiveカメラのワールド座標からスクリーン座標に変換
		Vector3 UIPos = gameCamera.WorldToScreenPoint(playerPos);
		//Ｚ（奥行き）の位置は固定
		UIPos.z = 1.0f;
		//文字の座標をスクリーン座標からOrthographicカメラのワールド座標に反映
		//UIPos = orthCamera.ScreenToWorldPoint(UIPos);
		renderPos.x = UIPos.x * DefaultScreen.ParInv.x;
		renderPos.y = UIPos.y * DefaultScreen.ParInv.y;
		SLogo.SetPos(renderPos);
		m_Timer = timer;
		//桁数計算
		int tmpDigit = 0;
		int tmpCombo = combo;
		//各桁に数字設定
		for(tmpDigit=0; tmpCombo>9; tmpDigit++){
			AddDigit(width, height, tmpCombo%10);
			tmpCombo /= 10;
		}
		if(tmpCombo > 0)
			AddDigit(width, height, tmpCombo%10);
		else
			AddDigit(width, height, 0);
	}
	
	//桁増やす処理
	private void AddDigit(int width , int height, int num){
		GameObject obj = (GameObject)Instantiate(obj_NumTex, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		objArray.Add(obj.gameObject);
		numArray.Add(obj.GetComponent<Tex2DGUITextureScript>());
		((Tex2DGUITextureScript)numArray[digit]).SetSize( (float)width, (float)height);
		((Tex2DGUITextureScript)numArray[digit]).SetPos( renderPos.x -(float)((width)*(digit+1)), (renderPos.y));
		((Tex2DGUITextureScript)numArray[digit]).SwitchTexture(num);
		digit++;
	}
}
