using UnityEngine;
using System.Collections;

public class ResultRendererScript : MonoBehaviour {
	
	public enum RESULT_SEEQ
	{
		WAIT,
		PANEL,
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
	bool deadLine;
	
	//各種画像へのアクセス用
	Tex2DGUITextureScript SPanel;
	Tex2DGUITextureScript SResultLogo;
	Tex2DGUITextureScript SScoreLogo;
	Tex2DGUITextureScript SComboLogo;
	Tex2DGUITextureScript SRank;
	Tex2DGUITextureScript SGaugeUI;
	Tex2DGUITextureScript SGauge;
	ScoreScript			  SScore;
	public GameObject obj_Achievement;
	ArrayList AchievementArray;
	
	//外部オブジェクト
	public GameObject	obj_ScoreRenderer;
	ScoreRendererScript SScoreRenderer;
	
	private bool valid;
	private bool end;
	public int  waitCnt = 60;
	public int  PanelCount = 30;
	private int  panelCnt;
	private bool dispScore = false;
	private bool dispCombo = false;
	private int  achievementIdx;
	private int  achievementDispSpan;
	private int  ACHIEVEMENT_DISP_SPAN = 10;
	
	//各項目の表示位置（インスペクタで編集できるように）
	public Vector2 posAchievement;
	
	// Use this for initialization
	void Start () {
		state = (char)RESULT_SEEQ.WAIT;
		valid = false;
		end = false;
		panelCnt = PanelCount;
		//変数初期化
		numJudgeKind = (int)LineManagerScript.JUDGE.NUM_JUDGE;
		numJudge = new int[numJudgeKind];
		//各画像へのアクセス準備
		SPanel		= ((GameObject)GameObject.Find("panel")).GetComponent<Tex2DGUITextureScript>();
		SResultLogo = ((GameObject)GameObject.Find("ResultLogo")).GetComponent<Tex2DGUITextureScript>();
		SScoreLogo 	= ((GameObject)GameObject.Find("ResultScore")).GetComponent<Tex2DGUITextureScript>();
		SComboLogo 	= ((GameObject)GameObject.Find("ResultCombo")).GetComponent<Tex2DGUITextureScript>();
		SRank 		= ((GameObject)GameObject.Find("ResultRank")).GetComponent<Tex2DGUITextureScript>();
		SGauge		= ((GameObject)GameObject.Find("GaugeTexture")).GetComponent<Tex2DGUITextureScript>();
		SGaugeUI	= ((GameObject)GameObject.Find("UI")).GetComponent<Tex2DGUITextureScript>();
		SScore		= (ScoreScript)GameObject.Find("ScoreTextBox").GetComponent("ScoreScript");
		//アチーブメントの表示
		AchievementArray = new ArrayList();
	}
	// Update is called once per frame
	void Update () {
		if(valid)
		{
			switch(state)
			{
			case (char)RESULT_SEEQ.WAIT:
				if(waitCnt > 0)
					waitCnt--;
				else
				{
					SPanel.SetRenderFlag(true);
					SPanel.SetColor(new Color(0.5f, 0.5f, 0.5f, 0.0f));
					state = (char)RESULT_SEEQ.PANEL;
				}
				break;
			case (char)RESULT_SEEQ.PANEL:
				Color col = SPanel.GetColor();
				if(panelCnt >0)
					panelCnt--;
				else{
					state = (char)RESULT_SEEQ.LOGO;
				}
				col.a = 0.5f - (0.5f*(float)((float)panelCnt/(float)PanelCount));
				SPanel.SetColor(col);
				float alpha = (float)((float)panelCnt/(float)PanelCount);
				SGauge.SetColor(new Color(0.5f, 0.5f, 0.5f, alpha));
				SGaugeUI.SetColor(new Color(0.5f, 0.5f, 0.5f, alpha));
				SScore.SetColor(new Color(0.5f, 0.5f, 0.5f, alpha));
				break;
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
		Vector2 work;
		if(achievementDispSpan > 0)
			achievementDispSpan--;
		else
		{
			if(achievementIdx < AchievementArray.Count)
				achievementIdx++;
			achievementDispSpan = ACHIEVEMENT_DISP_SPAN;
		}
		for(int i=0; i<achievementIdx; i++)
		{
			work = ((Tex2DGUITextureScript)AchievementArray[i]).GetPos();
			((Tex2DGUITextureScript)AchievementArray[i]).SetRenderFlag(true);
			((Tex2DGUITextureScript)AchievementArray[i]).SetPos(work.x+((posAchievement.x - work.x)*0.1f), 
																work.y, false);
		}
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
		//ラインアウトしていないか
		deadLine = false;
		//各判定の数
		for(int i=0; i<numJudgeKind; i++)
			numJudge[i] = SLineManager.GetNumJudge(i);
		//クリティカルコンボ数
		criticalCombo = SLineManager.GetNumCriticalCombo();
		
		//アチーブメントの判定
		CheckAchievement();
		achievementIdx = 0;
	}
	//アチーブメントの判定
	void CheckAchievement()
	{
		//判定用計算
		int numCut = numJudge[(int)LineManagerScript.JUDGE.GOOD] +
			         numJudge[(int)LineManagerScript.JUDGE.NORMAL] +
			         numJudge[(int)LineManagerScript.JUDGE.SAFE] ;
			
		//配列クリア
		AchievementArray.Clear();
		//順番に判定していく
		if(numJudge[(int)LineManagerScript.JUDGE.MISS] == 0)	AddAchievement(0);
		
		if(	numCut == 0)	AddAchievement(1);
	
		if(!deadLine)		AddAchievement(2);
		
		if(bomb > 6)		AddAchievement(3);
		else if(bomb > 4)	AddAchievement(4);
		else if(bomb > 2)	AddAchievement(5);
		
		if(		combo > 100)	AddAchievement(6);
		else if(combo > 80) 	AddAchievement(7);		
		else if(combo > 60) 	AddAchievement(8);
		else if(combo > 40) 	AddAchievement(9);
		else if(combo > 20) 	AddAchievement(10);
		
		if(		numCut > 100)	AddAchievement(11);
		else if(numCut > 80) 	AddAchievement(12);		
		else if(numCut > 50) 	AddAchievement(13);
		else if(numCut > 30) 	AddAchievement(14);
		else if(numCut > 10) 	AddAchievement(15);
		
		if(	 		criticalCombo> 100)	AddAchievement(16);
		else  if(  	criticalCombo> 80) 	AddAchievement(17);	
		else  if(  	criticalCombo> 50) 	AddAchievement(18);
		else  if(  	criticalCombo> 30) 	AddAchievement(19);
		else  if(  	criticalCombo> 10) 	AddAchievement(20);
		
		if(	 	  numJudge[(int)LineManagerScript.JUDGE.GOOD] > 100)	AddAchievement(21);
		else  if( numJudge[(int)LineManagerScript.JUDGE.GOOD] > 80) 	AddAchievement(22);		
		else  if( numJudge[(int)LineManagerScript.JUDGE.GOOD] > 60) 	AddAchievement(23);
		else  if( numJudge[(int)LineManagerScript.JUDGE.GOOD] > 40) 	AddAchievement(24);
		else  if( numJudge[(int)LineManagerScript.JUDGE.GOOD] > 20) 	AddAchievement(25);
		else  if( numJudge[(int)LineManagerScript.JUDGE.GOOD] > 10) 	AddAchievement(26);
		
		if(		upCnt >= 30 )	AddAchievement(29);
		else if(upCnt >= 15)	AddAchievement(28);
		else if(upCnt >= 10)	AddAchievement(27);
		
		//複合ボーナス
		if(criticalCombo> 100 && numJudge[(int)LineManagerScript.JUDGE.GOOD] > 100) AddAchievement(30);
		if(combo > 100 && numJudge[(int)LineManagerScript.JUDGE.GOOD] > 100)		AddAchievement(31);
		if(numJudge[(int)LineManagerScript.JUDGE.MISS]==0 && !deadLine && bomb>6)	AddAchievement(32);
		if(!deadLine && upCnt >= 15)												AddAchievement(33);
	}
	void AddAchievement(int id)
	{
		int idx = AchievementArray.Count;
		GameObject obj = (GameObject)Instantiate(obj_Achievement, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		AchievementArray.Add(obj.GetComponent<Tex2DGUITextureScript>());
		((Tex2DGUITextureScript)AchievementArray[idx]).SetPos(posAchievement.x + 300, posAchievement.y+(65*idx), true);
		((Tex2DGUITextureScript)AchievementArray[idx]).SwitchTexture(id);
		((Tex2DGUITextureScript)AchievementArray[idx]).SetRenderFlag(false);
	}
	
	public void Validate()
	{
		valid = true;
		GatherParameters();
	}
	public bool isEnd()	{	return end;	}
}
