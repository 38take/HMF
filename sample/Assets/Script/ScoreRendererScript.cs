using UnityEngine;
using System.Collections;

public class ScoreRendererScript : MonoBehaviour {
	
	int Score;
	int RenderScore = 0;
	int currentDigitScore = 0;
	int currentDigitNum = 0;
	int currentDigit = 0;
	bool valid = false;
	bool skip = false;
	
	ArrayList	numArray;
	public GameObject	obj_NumTex;
	int digit;	
	
	public Vector2 	Position;
	public float	Width;
	public float	Height;
	
	//================================================//
	//プライベート関数
	//更新
	void Start(){
	}
	void Update () {
		if(valid)
		{
			if(skip)
			{
				while(valid)
					valid = ScoreAct();
			}
			else
				valid = ScoreAct();
		}
	}
	//数字出していく演出 出し切ったらfalse
	private bool ScoreAct()
	{
		//
		if(currentDigitNum < currentDigitScore)
		{
			currentDigitNum++;
			if(currentDigitNum > 0)
				((Tex2DGUITextureScript)numArray[(digit-1)]).SwitchTexture(currentDigitNum%10);
			else
				((Tex2DGUITextureScript)numArray[(digit-1)]).SwitchTexture(0);
		}
		else
		{
			int filter = 1;
			for(int i=0; i<digit-1; i++)
				filter *= 10;
			RenderScore += currentDigitScore*filter;
			if(RenderScore >= Score)
				return false;
			AddDigit();
			filter = 10;
			for(int i=0; i<digit-2; i++)
				filter *= 10;
			currentDigitScore =  Score%(filter*10)/filter;
			currentDigitNum = 0;
		}
		
		return true;
	}
	//桁増やす処理
	private void AddDigit(){
		GameObject obj = (GameObject)Instantiate(obj_NumTex, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		numArray.Add(obj.GetComponent<Tex2DGUITextureScript>());
		((Tex2DGUITextureScript)numArray[digit]).SetSize( (float)Width, (float)Height);
		((Tex2DGUITextureScript)numArray[digit]).SetPos( Position.x - (float)((Width+10)*digit)-Width, Position.y);
		((Tex2DGUITextureScript)numArray[digit]).SwitchTexture(0);
		((Tex2DGUITextureScript)numArray[digit]).SetRenderFlag(true);
		digit = digit+1;
	}
	
	//================================================//
	//パブリック関数
	public void Validate(int score)
	{
		digit = 0;
		numArray = new ArrayList();
		AddDigit();
		
		Score = score;
		RenderScore = 0;
		currentDigitNum = 0;
		currentDigitScore = Score % 10;
		valid = true;
	}
	//位置セット　Validateを呼ぶ前に実行しておく
	public void SetPos(float setX, float setY, bool convert)
	{
		Position.x = setX;
		if(convert)	Position.y = DefaultScreen.Height - setY;
		else 		Position.y = setY;
	}
	//1文字のサイズセット Validateを呼ぶ前に実行しておく
	public void SetSize(float width, float height)
	{
		Width = width;
		Height = height;
	}
	
	public bool isValid() {	return valid;	}
	public void SkipAct() {	skip = true;	}
}
