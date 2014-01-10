using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class ScoreScript : MonoBehaviour {
	
	ArrayList	numArray;
	public GameObject	obj_NumTex;
	int digit;	
	
	public Vector2 RenderPos;
	public int Width;
	public int Height;
	
	private	float Score = 0.0f;
	private	float RenderScore = 0.0f;
	
	private bool initiarized = false;
	
	
	void Init()
	{
		digit = 0;
		numArray = new ArrayList();
		AddDigit();
		initiarized = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!initiarized)
			Init();
		
		//描画用スコア処理
		int addScore = 1;
		if(Score > RenderScore)
		{
			if(Score - RenderScore > 1000)		addScore = 100;
			else if(Score - RenderScore > 100)	addScore = 10;
			RenderScore += addScore;
		}
		//桁数計算
		int tmpDigit = 1;
		int tmpScore = (int)RenderScore;
		for(;tmpScore > 9; tmpDigit++){tmpScore /= 10;}
		for(;tmpDigit > digit;){	AddDigit();	}
		//各桁に数字設定
		tmpScore = (int)RenderScore;
		int num = 0;
		for(tmpDigit=0; tmpScore>9; tmpDigit++){
			num = tmpScore%10;
			((Tex2DGUITextureScript)numArray[tmpDigit]).SwitchTexture(num);
			tmpScore /= 10;
		}
		if(tmpScore > 0)
			((Tex2DGUITextureScript)numArray[tmpDigit]).SwitchTexture(tmpScore%10);
		else
			((Tex2DGUITextureScript)numArray[tmpDigit]).SwitchTexture(0);
	}
	
	public void AddScore( float scr )
	{
		Score += scr;	
	}
	public void Test()
	{
		Debug.Log("てす");
	}
	//スコアの取得
	public float GetScore()
	{
		return Score;
	}
	
	//桁増やす処理
	private void AddDigit(){
		GameObject obj = (GameObject)Instantiate(obj_NumTex, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		numArray.Add(obj.GetComponent<Tex2DGUITextureScript>());
		((Tex2DGUITextureScript)numArray[digit]).SetSize( (float)Width, (float)Height);
		((Tex2DGUITextureScript)numArray[digit]).SetPos( RenderPos.x - (float)((Width+10)*digit), RenderPos.y);
		((Tex2DGUITextureScript)numArray[digit]).SetRenderFlag(true);
		digit++;
	}
	
	public void SetColor(Color col)
	{
		for(int i=0; i<numArray.Count; i++)
		{
			((Tex2DGUITextureScript)numArray[i]).SetColor(col);
		}
	}
}
