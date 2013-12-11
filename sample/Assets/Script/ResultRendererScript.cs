using UnityEngine;
using System.Collections;

public class ResultRendererScript : MonoBehaviour {
	
	public enum RESULT_SEEQ
	{
		LOGO,
		SCORE,
		COMBO,
		ACHIEVEMENT,
		RANK,
		NUM_SEEQ,
	};
	char state;
	//判定用数値格納用変数
	int score;
	int combo;
	int criticalCombo;
	int bomb;
	int upCnt;
	int numJudgeKind;
	int[] numJudge;
	
	//各種画像へのアクセス用
	Tex2DGUITextureScript SResultLogo;
	Tex2DGUITextureScript SScoreLogo;
	Tex2DGUITextureScript SComboLogo;
	Tex2DGUITextureScript SRank;
	
	//外部オブジェクト
	public GameObject	obj_ScoreRenderer;
	ScoreRendererScript SScoreRenderer;
	
	private bool valid;
	private bool end;
	private bool dispScore = false;
	private bool dispCombo = false;
	
	// Use this for initialization
	void Start () {
		state = (char)RESULT_SEEQ.LOGO;
		valid = false;
		end = false;
		//変数初期化
		numJudgeKind = (int)LineManagerScript.JUDGE.NUM_JUDGE;
		numJudge = new int[numJudgeKind];
		//各画像へのアクセス準備
		SResultLogo = ((GameObject)GameObject.Find("ResultLogo")).GetComponent<Tex2DGUITextureScript>();
		SScoreLogo 	= ((GameObject)GameObject.Find("ResultScore")).GetComponent<Tex2DGUITextureScript>();
		SComboLogo 	= ((GameObject)GameObject.Find("ResultCombo")).GetComponent<Tex2DGUITextureScript>();
		SRank 		= ((GameObject)GameObject.Find("ResultRank")).GetComponent<Tex2DGUITextureScript>();
	}
	// Update is called once per frame
	void Update () {
		if(valid)
		{
			switch(state)
			{
			case (char)RESULT_SEEQ.LOGO:
				if(!ActLogo())
					state = (char)RESULT_SEEQ.SCORE;
				break;
			case (char)RESULT_SEEQ.SCORE:
				if(!ActScore())
					state = (char)RESULT_SEEQ.COMBO;
				break;
			case (char)RESULT_SEEQ.COMBO:
				if(!ActCombo())
					state = (char)RESULT_SEEQ.ACHIEVEMENT;
				break;
			case (char)RESULT_SEEQ.ACHIEVEMENT:
				if(!ActAchievement())
					state = (char)RESULT_SEEQ.RANK;
				break;
			case (char)RESULT_SEEQ.RANK:
				if(!ActRank())
					state = (char)RESULT_SEEQ.NUM_SEEQ;
				break;
			case (char)RESULT_SEEQ.NUM_SEEQ:
					end = true;
				break;
			default:
				break;
			}
		}
		else
		{
			SResultLogo.SetRenderFlag(false);
			SScoreLogo.SetRenderFlag(false);
			SComboLogo.SetRenderFlag(false);
			SRank.SetRenderFlag(false);
		}	
	}
	
	//リザルトロゴ表示
	bool ActLogo()
	{
		SResultLogo.RestoreTextureRect();
		SResultLogo.SetRenderFlag(true);
		
		if(Input.GetMouseButtonDown(0))
			return false;
		return true;
	}
	//スコア表示
	bool ActScore()
	{
		SScoreLogo.RestoreTextureRect();
		SScoreLogo.SetRenderFlag(true);
		//数値表示初期化
		if(!dispScore)
		{
			GameObject obj = (GameObject)Instantiate(obj_ScoreRenderer, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			SScoreRenderer = obj.GetComponent<ScoreRendererScript>();
			SScoreRenderer.SetPos(800.0f, SScoreLogo.GetPos().y + SScoreLogo.GetSize().y, true);
			SScoreRenderer.Validate(score);
			dispScore = true;
		}
		//クリックでスキップ
		if(Input.GetMouseButtonDown(0))
			SScoreRenderer.SkipAct();
		//終了判定
		if(!SScoreRenderer.isValid())
			return false;
		return true;
	}
	//コンボ数表示
	bool ActCombo()
	{
		SComboLogo.RestoreTextureRect();
		SComboLogo.SetRenderFlag(true);
		//数値表示初期化
		if(!dispCombo)
		{
			GameObject obj = (GameObject)Instantiate(obj_ScoreRenderer, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			SScoreRenderer = obj.GetComponent<ScoreRendererScript>();
			SScoreRenderer.SetPos(800.0f, SComboLogo.GetPos().y+SComboLogo.GetSize().y, true);
			SScoreRenderer.Validate(combo);
			dispCombo = true;
		}
		//クリックでスキップ
		if(Input.GetMouseButtonDown(0))
			SScoreRenderer.SkipAct();
		//終了判定
		if(!SScoreRenderer.isValid())
			return false;
		return true;
	}
	//アチーブメントボーナス表示
	bool ActAchievement()
	{
		if(Input.GetMouseButtonDown(0))
			return false;
		return true;
	}
	//ランク表示
	bool ActRank()
	{
		SRank.SwitchTexture(1);
		SRank.RestoreTextureRect();
		SRank.SetRenderFlag(true);
		
		if(Input.GetMouseButtonDown(0))
			return false;
		return true;
	}
	
	//パラメータの収集
	void GatherParameters()
	{
		ComboScript SCombo = ((GameObject)GameObject.Find ("ComboManager")).GetComponent<ComboScript>();
		ScoreScript SScore = ((GameObject)GameObject.Find ("ScoreTextBox")).GetComponent<ScoreScript>();
		PlayerScript SPlayer = ((GameObject)GameObject.Find ("ナイフ5")).GetComponent<PlayerScript>();
		LineManagerScript SLineManager = ((GameObject)GameObject.Find ("LineManager")).GetComponent<LineManagerScript>();
		
		//最大コンボ数
		combo = SCombo.GetMaxCombo();
		//スコア
		score = (int)SScore.GetScore();
		//ボム使用回数
		bomb = SPlayer.GetNumBomb();
		//持ち上げた回数
		upCnt = SPlayer.GetNumUp();
		//各判定の数
		for(int i=0; i<numJudgeKind; i++)
			numJudge[i] = SLineManager.GetNumJudge(i);
		//クリティカルコンボ数
		criticalCombo = SLineManager.GetNumCriticalCombo();
	}
	
	public void Validate()
	{
		valid = true;
		GatherParameters();
	}
	public bool isEnd()	{	return end;	}
}
