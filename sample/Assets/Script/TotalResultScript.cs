using UnityEngine;
using System.Collections;

public class TotalResultScript : MonoBehaviour {
	
	//描画段階定義
	public enum RESULT_SEEQ
	{
		LOGO,
		SCORE1,
		SCORE2,
		SCORE3,
		GAUGE,
		TITLE,
		NUM_SEEQ,
	};
	char state;
	//各種画像へのアクセス用
	Tex2DGUITextureScript SResultLogo;
	Tex2DGUITextureScript[] SScoreLogo;
	Tex2DGUITextureScript SEstimationLogo;
	Tex2DGUITextureScript SGoodIcon;
	Tex2DGUITextureScript SBadIcon;
	Tex2DGUITextureScript STitleLogo;
	Tex2DGUITextureScript STitle;
	bool[] dispScore;
	
	//外部オブジェクト
	public GameObject	obj_ScoreRenderer;
	ScoreRendererScript SScoreRenderer;
	//各種パラメータ
	private int[] score;
	
	//
	private bool initiarized = false;
	private bool end		 = false;
	
	
	void Init()
	{
		GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
		//各ステージのスコア取得
		score = new int[3];
		score[0] = gamesys.GetScore(0);
		score[1] = gamesys.GetScore(1);
		score[2] = gamesys.GetScore(2);
		//各画像へのアクセス準備
		SResultLogo 	= ((GameObject)GameObject.Find("ResultLogo")).GetComponent<Tex2DGUITextureScript>();
		SScoreLogo		= new Tex2DGUITextureScript[3];
		SScoreLogo[0] 	= ((GameObject)GameObject.Find("ScoreLabel1")).GetComponent<Tex2DGUITextureScript>();
		SScoreLogo[1] 	= ((GameObject)GameObject.Find("ScoreLabel2")).GetComponent<Tex2DGUITextureScript>();
		SScoreLogo[2] 	= ((GameObject)GameObject.Find("ScoreLabel3")).GetComponent<Tex2DGUITextureScript>();
		SEstimationLogo = ((GameObject)GameObject.Find("AtelierEstimation")).GetComponent<Tex2DGUITextureScript>();
		SGoodIcon 		= ((GameObject)GameObject.Find("GoodIcon")).GetComponent<Tex2DGUITextureScript>();
		SBadIcon 		= ((GameObject)GameObject.Find("BadIcon")).GetComponent<Tex2DGUITextureScript>();
		STitleLogo		= ((GameObject)GameObject.Find("TitleFrame")).GetComponent<Tex2DGUITextureScript>();
		STitle			= ((GameObject)GameObject.Find("Title")).GetComponent<Tex2DGUITextureScript>();
		
		//各画像はいったん非表示
		SResultLogo 	.SetRenderFlag(false);
		SScoreLogo[0] 	.SetRenderFlag(false);
		SScoreLogo[1] 	.SetRenderFlag(false);
		SScoreLogo[2] 	.SetRenderFlag(false);
		SEstimationLogo .SetRenderFlag(false);
		SGoodIcon 		.SetRenderFlag(false);
		SBadIcon 		.SetRenderFlag(false);
		STitleLogo		.SetRenderFlag(false);
		STitle			.SetRenderFlag(false);
		
		//パラメータ初期化
		dispScore = new bool[3];
		dispScore[0] = dispScore[1] = dispScore[2] = false;
				
		initiarized = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!initiarized)
			Init();
		
		switch(state)
		{
		case (char)RESULT_SEEQ.LOGO:
			if(!ActLogo())
				state = (char)RESULT_SEEQ.SCORE1;
			break;
		case (char)RESULT_SEEQ.SCORE1:
			if(!ActScore(0))
				state = (char)RESULT_SEEQ.SCORE2;
			break;
		case (char)RESULT_SEEQ.SCORE2:
			if(!ActScore(1))
				state = (char)RESULT_SEEQ.SCORE3;
			break;
		case (char)RESULT_SEEQ.SCORE3:
			if(!ActScore(2))
				state = (char)RESULT_SEEQ.GAUGE;
			break;
		case (char)RESULT_SEEQ.GAUGE:
			if(!ActGauge())
				state = (char)RESULT_SEEQ.TITLE;
			break;
		case (char)RESULT_SEEQ.TITLE:
			if(!ActTitle())
				state = (char)RESULT_SEEQ.NUM_SEEQ;
			break;
		case (char)RESULT_SEEQ.NUM_SEEQ:
				end = true;
			break;
		default:
			break;
		}
	}
	//ロゴ
	bool ActLogo()
	{
		SResultLogo.RestoreTextureRect();
		SResultLogo.SetRenderFlag(true);
		
		if(Input.GetMouseButtonDown(0))
			return false;
		return true;
	}
	//スコア
	bool ActScore(int id)
	{
		SScoreLogo[id].RestoreTextureRect();
		SScoreLogo[id].SetRenderFlag(true);
		//数値表示初期化
		if(!dispScore[id])
		{
			GameObject obj = (GameObject)Instantiate(obj_ScoreRenderer, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			SScoreRenderer = obj.GetComponent<ScoreRendererScript>();
			SScoreRenderer.SetPos(800.0f, SScoreLogo[id].GetPos().y + SScoreLogo[id].GetSize().y, true);
			SScoreRenderer.Validate(score[id]);
			dispScore[id] = true;
		}
		//クリックでスキップ
		if(Input.GetMouseButtonDown(0))
			SScoreRenderer.SkipAct();
		//終了判定
		if(!SScoreRenderer.isValid())
			return false;
		return true;
	}
	//評判ゲージ
	bool ActGauge()
	{
		SEstimationLogo.RestoreTextureRect();
		SEstimationLogo.SetRenderFlag(true);
		SBadIcon.RestoreTextureRect();
		SBadIcon.SetRenderFlag(true);
		SGoodIcon.RestoreTextureRect();
		SGoodIcon.SetRenderFlag(true);
		
		if(Input.GetMouseButtonDown(0))
			return false;
		return true;
	}
	//称号
	bool ActTitle()
	{
		STitleLogo.RestoreTextureRect();
		STitleLogo.SetRenderFlag(true);
		STitle.RestoreTextureRect();
		STitle.SetRenderFlag(true);
		
		if(Input.GetMouseButtonDown(0))
			return false;
		return true;
	}
	
	//終了判定取得
	public bool isEnd()	{	return end;	}
}
