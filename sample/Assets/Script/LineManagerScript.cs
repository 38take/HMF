using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class LineManagerScript : MonoBehaviour {
	
	public enum JUDGE
	{
		GOOD,
		NORMAL,
		SAFE,
		MISS,
		NUM_JUDGE
	};
	private enum STATE
	{
        LINEWIDTH,
		LINE,
		TARGET,
		STOPPOINT,
		NUM_STATE,
	};
	//各ラインのターゲット配列
	private struct TARGET_ARRAY
	{
		public Vector3[] 	targetPosArray; //ターゲット位置（X:中心ラインのオフセット Y:左右のオフセット）
		public bool[]		pastArray;		//通過したかのフラグ
		public int seek;
	};
	
	//外部ゲームオブジェクト
	public GameObject target;	
	public GameObject line;
	public GameObject lineDead;
	public GameObject hitRenderer;
	
	public GameObject effect_HitSafe;
	public GameObject effect_HitNormal;
	public GameObject effect_HitCritical;
	public GameObject effect_Miss;
	PlayerScript SPlayer;	
	//ステージの構成データ
	int stageID;
	Vector3[] 	lineData;
	int[]	  	lineKind;
	Vector3[] 	lineDir;
	TARGET_ARRAY[] targetArray;
	int[]		stopLineArray;
	int 		stopLineIndex;
	//各種パラメータ
	int numPoint; 
	int numTarget;
	int wherePlayer;
	int wherePlayerOld;
	public float lineWidth; 
	bool 	lastPoint;
	Vector3 playerOffset;
	Vector3 playerOldOffset;
	float   targetWidthInLine;
	Vector3 TargetHitChecker;
	bool    exist = false;
	//スコア
	public int ScoreCritical;
	public int ScoreNormal;
	public int ScoreSafe;
	public int ScoreMiss;
	//リザルトの用パラメータ
	int[] numJudge;
	int   criticalCombo;
	int   criticalComboMax;
	
	//線の生成
	private void CreateLine(Vector3 prev, Vector3 point1, Vector3 point2, Vector3 next, int kind)
	{
		LineScript obj;
		switch(kind)
		{
		case 0:
			obj = ((GameObject)Instantiate(line)).GetComponent<LineScript>();	
			break;
		case 1:
			obj = ((GameObject)Instantiate(lineDead)).GetComponent<LineScript>();	
			break;
		default:
			obj = ((GameObject)Instantiate(line)).GetComponent<LineScript>();
			break;
			
		}
		obj.SetData(prev, point1, point2, next, lineWidth);
	}
	//ターゲットの生成
	private void CreateTarget(int lineID, Vector2 offset)
	{
		//中央線内の位置計算
		Vector3 basePos = lineData[lineID] + (lineData[lineID+1]-lineData[lineID])*offset.x;
		//基準位置での線の方向算出
		Vector3 headDir = lineDir[lineID];
		if(lineID > 0)
			headDir = Vector3.Normalize(lineDir[lineID-1] + lineDir[lineID]);
		Vector3 footDir = Vector3.Normalize(lineDir[lineID+1] + lineDir[lineID]);
		Vector3 dir = headDir*(1.0f-offset.x)+footDir*offset.x;
		//Vector3 dir = lineDir[lineID]*(1.0f - offset.x) + lineDir[lineID+1]*offset.x;
		dir = Vector3.Normalize(dir);
		//算出した方向から左向きのベクトルへ変換
		float tmp = dir.z;
		dir.z = dir.x;
		dir.x = -tmp;
		//位置確定
		Vector3 pos = basePos + dir * (-offset.y) * lineWidth;//1,ofは線の太さ
		pos = new Vector3(pos.x, 0.1f, pos.z);
		//ターゲット生成
		Instantiate(target, pos, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		
	}
	
	// Use this for initialization
	void Start () {
		char state = (char)STATE.NUM_STATE;
		String str;
		String[] strBase;
		String[] split;
		
		ArrayList data = new ArrayList();
		Vector3 dummy = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		
		int lineID = new int();
		int tNum = new int();
		int tCnt = new int();
		GameObject floor = GameObject.Find("Floor");
		float stageHorizontal = floor.transform.localScale.x * 5.0f;
		float stageVertical = floor.transform.localScale.z * 5.0f;
		
		hitRenderer = (GameObject)GameObject.Find("HitTextBox");
		SPlayer = ((GameObject)GameObject.Find("ナイフ5")).GetComponent<PlayerScript>();
		//パラメータ初期化
		numPoint 	= 0;
		numTarget 	= 0;
		wherePlayer = 0;
		wherePlayerOld = 0;
		lineID = tNum = tCnt = -1;
		stopLineIndex = 0;
		//lineWidth = 3.0f;
		lastPoint = false;
		playerOldOffset = new Vector3(0.0f, 0.0f, 0.0f);
		TargetHitChecker = new Vector3(0.1f, 0.3f, 1.0f);
		targetWidthInLine = 5.0f / (lineWidth*2);
		//==================================================
		//ステージファイルの読み込み
		GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
		stageID = 1;
		switch(gamesys.GetActID())
		{
		case 1:			stageID = 0; 	break;	//チュートリアル
		case 2:			stageID = 1;	break;	//ステージ１
		case 7:			stageID = 2;	break;	//ステージ２
		case 12:		stageID = 3;	break;	//ステージ３
		default:		stageID = 1;	break;
		}
		FileInfo fi = new FileInfo(Application.dataPath+"/GameData/stage/stage"+stageID+".txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 )
		{
			str = sr.ReadLine();
			strBase = str.Split(';');
			split = strBase[0].Split(',');
			
			switch(state)
			{
            case (char)STATE.LINEWIDTH:
                if (split[0] == "LinePoint")
                {
                    state = (char)STATE.LINE;
                    break;
                }
                lineWidth = float.Parse(split[0]);
                break;

			case (char)STATE.LINE:
				//マーカー目印がきたらラインの生成
				if(split[0] == "Marker")
				{
					//頂点数算出
					numPoint = (int)(data.Count / 4);
					lineData = new Vector3[numPoint];
					lineKind = new int[numPoint];
					lineDir  = new Vector3[numPoint];//点→点
					for(int i=0; i<numPoint; i++)
					{
						lineData[i].x = (float)data[(i*4)+0] * stageHorizontal;
						lineData[i].y = (float)data[(i*4)+1];
						lineData[i].z = (float)data[(i*4)+2] * stageVertical;
						lineKind[i]   = (int)data[(i*4)+3];
					}
					//必要分ラインを生成
					//obj = new LineScript[numPoint-1];
					Vector3[] inValue = new Vector3[4];
					Vector3 tmp;
					for(int i=0; i<(numPoint-1);i++)
					{
						//引数をセットしていく
						if(i==0)
							inValue[0] = dummy;
						else
							inValue[0] = lineData[i-1];
						inValue[1] = lineData[i];
						inValue[2] = lineData[i+1];	
						if(i==(numPoint-2))
							inValue[3] = dummy;
						else
							inValue[3] = lineData[i+2];					
						//生成関数
						CreateLine(inValue[0], inValue[1], inValue[2], inValue[3], lineKind[i]);
						//線の方向を算出
						if(i>0)
							tmp = lineDir[i-1];
						else
							tmp = new Vector3(0.0f, 0.0f, 0.0f);
						lineDir[i] = Vector3.Normalize((inValue[2] - inValue[1])+tmp);
					}
					lineDir[numPoint-1] = lineDir[numPoint-2];
					//for(int i=0; i<1000; i++)
					//	CreateLine(dummy, dummy, dummy, dummy);
					targetArray = new TARGET_ARRAY[numPoint-1];
					state = (char)STATE.TARGET;
					data.Clear();
					break;
				}
				//数値を格納
				data.Add(float.Parse(split[0]));
				data.Add(0.1f);
				data.Add(float.Parse(split[1]));
				data.Add(int.Parse(split[2]));
				
				break;
				
			case (char)STATE.TARGET:
				if(split[0] == "EOF")
				{
					data.Clear();
					break;
				}
				if(split[0] ==	"StopPoint")
				{
                    state = (char)STATE.STOPPOINT;
                    break;
				}
				//ラインID取得
				if(lineID < 0){
					lineID = int.Parse(split[0]);
					break;
				}
				//ライン内のターゲットの個数取得
				if(tNum < 0){
					tNum = int.Parse(split[0]);
					tCnt = tNum;
					targetArray[lineID].targetPosArray = new Vector3[tCnt];
					targetArray[lineID].pastArray = new bool[tCnt];
					break;
				}
				//ターゲットデータ取得
				data.Add(float.Parse(split[0]));
				data.Add(float.Parse(split[1]));
				tCnt--;
				//ターゲット生成
				if(tCnt <= 0)
				{
					Vector2 offset = new Vector2();
					
					for(int i=0; i<tNum; i++){
						offset.x = (float)data[i*2];
						offset.y = (float)data[i*2+1];
						CreateTarget(lineID, offset);
						
						targetArray[lineID].targetPosArray[i].x = offset.x;
						targetArray[lineID].targetPosArray[i].y = offset.y;
						targetArray[lineID].targetPosArray[i].z = 0.0f;
						targetArray[lineID].pastArray[i]		= false;
					}
					targetArray[lineID].seek = 0;
					
					numTarget += tNum;
					
					data.Clear();
					lineID = -1;
					tNum = -1;
				}
				break;
			case (char)STATE.STOPPOINT:
				if(split[0] == "EOF")
				{
					stopLineArray = new int[data.Count];
					for(int i=0; i<data.Count; i++)
					{
						stopLineArray[i] = (int)data[i];
					}
					data.Clear();
					break;
				}
				//数値を格納
				data.Add(int.Parse(split[0]));
				break;
				
			case (char)STATE.NUM_STATE:
				if(split[0] == "LineWidth")
					state = (char)STATE.LINEWIDTH;
                if(split[0] == "LinePoint")
                    state = (char)STATE.LINE;
				break;
			default:
				break;
			}
       	}
       	sr.Close();
		//Instantiate(obj, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		
		//判定用パラメータ初期化
		numJudge = new int[(int)JUDGE.NUM_JUDGE];
		for(int i=0; i<(int)JUDGE.NUM_JUDGE; i++)
			numJudge[i] = 0;
		criticalCombo = criticalComboMax = 0;
		
		exist = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(stopLineArray != null && stopLineIndex < stopLineArray.Length && wherePlayer > stopLineArray[stopLineIndex])
			stopLineIndex++;
	}
	//ライン上の位置における横向きベクトル算出
	public Vector3 CalcHorizontalDir(int timer)
	{
		float length = (float)timer * 0.1f;
		float lineLength = new float();
		//今いるラインを算出(速度0.1として)
		int lineIdx = new int();
		for(lineIdx=0; lineIdx<numPoint-2; lineIdx++)
		{
			lineLength = Vector3.Distance(lineData[lineIdx],lineData[lineIdx+1]);
			if((length - lineLength) < 0.0f)
				break;
			length -= lineLength;
		}
		if(length > lineLength)
		{
			length = lineLength;
		}
		length = length / lineLength;//中心線のオフセット算出
		//方向算出
		Vector3 dir = lineDir[lineIdx]*(1.0f-length)+lineDir[lineIdx+1]*length;
		dir = Vector3.Normalize(dir);
		float tmp = dir.x;
		dir.x = dir.z;
		dir.z = -tmp;
		return dir;
	}
	//ライン上の位置算出
	public Vector3 CalcPos(int timer, float offset)
	{
		Vector3 ret = new Vector3();
		float length = (float)timer * 0.1f;
		float lineLength = new float();
		//今いるラインを算出(速度0.1として)
		int lineIdx = new int();
		for(lineIdx=0; lineIdx<numPoint-2; lineIdx++)
		{
			lineLength = Vector3.Distance(lineData[lineIdx],lineData[lineIdx+1]);
			if((length - lineLength) < 0.0f)
				break;
			length -= lineLength;
		}
		if(length > lineLength)
		{
			length = lineLength;
		}
		length = length / lineLength;//中心線のオフセット算出
		
		//基準点算出
		Vector3 basePos = lineData[lineIdx] + (lineData[lineIdx+1]-lineData[lineIdx])*length;
		//方向算出
		Vector3 headDir = lineDir[lineIdx];
		if(lineIdx > 0)
			headDir = Vector3.Normalize(lineDir[lineIdx-1] + lineDir[lineIdx]);
		Vector3 footDir = Vector3.Normalize(lineDir[lineIdx+1] + lineDir[lineIdx]);
		Vector3 dir = headDir*(1.0f-length)+footDir*length;
		dir = Vector3.Normalize(dir);
		float tmp = dir.x;
		dir.x = dir.z;
		dir.z = -tmp;
		//位置確定
		ret = basePos + dir * offset * lineWidth;
		
		return ret;
	}
	//ライン上の位置算出（プレイヤーの移動処理内限定）
	public Vector3 CalcPosWithHitCheck(int timer, float offset)
	{
		Vector3 ret = new Vector3();
		float length = (float)timer * 0.1f;
		float lineLength = new float();
		//今いるラインを算出(速度0.1として)
		int lineIdx = new int();
		for(lineIdx=0; lineIdx<numPoint-2; lineIdx++)
		{
			lineLength = Vector3.Distance(lineData[lineIdx],lineData[lineIdx+1]);
			if((length - lineLength) < 0.0f)
				break;
			length -= lineLength;
		}
		if(length > lineLength)
		{
			length = lineLength;
			lastPoint = true;
		}
		length = length / lineLength;//中心線のオフセット算出
		
		//基準点算出
		Vector3 basePos = lineData[lineIdx] + (lineData[lineIdx+1]-lineData[lineIdx])*length;
		//方向算出
		Vector3 headDir = lineDir[lineIdx];
		if(lineIdx > 0)
			headDir = Vector3.Normalize(lineDir[lineIdx-1] + lineDir[lineIdx]);
		Vector3 footDir = Vector3.Normalize(lineDir[lineIdx+1] + lineDir[lineIdx]);
		Vector3 dir = headDir*(1.0f-length)+footDir*length;
		//Vector3 dir = lineDir[lineIdx]*(1.0f-length)+lineDir[lineIdx+1]*length;
		dir = Vector3.Normalize(dir);
		float tmp = dir.x;
		dir.x = dir.z;
		dir.z = -tmp;
		//位置確定
		ret = basePos + dir * offset * lineWidth;
		
		//停止属性のラインまで来たらプレイヤーを停止(チュートリアル用)
		wherePlayer = lineIdx;
		if(stopLineArray != null && stopLineIndex < stopLineArray.Length && wherePlayer == stopLineArray[stopLineIndex])
		{
			SPlayer.Stop();
			//会話スタート
			
			stopLineIndex++;
		}
		//赤線上にいたら通知
		if(lineKind[wherePlayer] == 1)
			SPlayer.CutDeadLine();
		
		playerOffset = new Vector3(length, offset, 0.0f);
		HitCheckPlayerWithTarget();
		
		return ret;
	}
	public int HitCheckPlayerWithTarget()
	{
		int ret = 0;
		int tmp = wherePlayerOld;
		//ターゲットとのあたり判定をとる
		if(wherePlayer != wherePlayerOld)
		{
			wherePlayerOld = wherePlayer;
			//ターゲットの位置におけるプレイヤーの横方向オフセット算出
			float transrate = playerOffset.x + (1.0f - playerOldOffset.x);
			float pos   = 1.0f - playerOldOffset.x;
			transrate = pos / transrate;
			float H = playerOldOffset.y + (playerOffset.y-playerOldOffset.y)*transrate;
			
			ret += HitCheckTarget(tmp, new Vector3(1.0f, H, 0.0f), playerOldOffset);
			ret += HitCheckTarget(wherePlayer, playerOffset, new Vector3(0.0f, H, 0.0f));
		}
		else
			ret = HitCheckTarget(wherePlayer, playerOffset, playerOldOffset);
		playerOldOffset = playerOffset;

		return ret;
	}
	private int HitCheckTarget(int lineID, Vector3 offset, Vector3 oldOffset)
	{
		Vector3 targetPos, p_pos;
		int retValue = 0;
		
		if(targetArray[lineID].targetPosArray != null &&
			targetArray[lineID].targetPosArray.Length > 0)
		{
			for(int i=targetArray[lineID].seek; i<targetArray[lineID].targetPosArray.Length; i++)
			{
				targetPos = targetArray[lineID].targetPosArray[i];
				if( offset.x >= targetPos.x )
				{
					if(oldOffset.x < targetPos.x)
					{
						//ターゲットの位置におけるプレイヤーの横方向オフセット算出
						float transrate = offset.x - oldOffset.x;
						float target   = targetPos.x - oldOffset.x;
						transrate = target / transrate;
						float H = oldOffset.y + (offset.y-oldOffset.y)*transrate;
						
						//判定
						H = H - targetPos.y;
						//クリティカル
						if( H < TargetHitChecker.x * targetWidthInLine &&
							H > -TargetHitChecker.x * targetWidthInLine)
						{
							hitRenderer.guiText.text = "クリティカル！！";
							numJudge[(int)JUDGE.GOOD]++;
							criticalCombo++;
							// エフェクト発生
							p_pos = SPlayer.transform.position;
							p_pos.y += 1.0f;
							GameObject ins_obj =
								(GameObject)Instantiate(effect_HitCritical,
														p_pos,
														SPlayer.transform.rotation	);
							Destroy(ins_obj, 1.0f);
							
							SPlayer.CalcCombo(true);
							SPlayer.CalcScore(ScoreCritical);
							SPlayer.CalcConcentration(5);
						}
						//ノーマル
						else if( H < TargetHitChecker.y * targetWidthInLine &&
								 H > -TargetHitChecker.y * targetWidthInLine)
						{
							hitRenderer.guiText.text = "ノーマル";
							numJudge[(int)JUDGE.NORMAL]++;
							criticalCombo = 0;
							// エフェクト発生
							p_pos = SPlayer.transform.position;
							p_pos.y += 1.0f;
							GameObject ins_obj =
								(GameObject)Instantiate(effect_HitNormal,
														p_pos,
														SPlayer.transform.rotation	);
							Destroy(ins_obj, 1.0f);
							
							SPlayer.CalcCombo(true);
							SPlayer.CalcScore(ScoreNormal);
							SPlayer.CalcConcentration(2);
						}
						//セーフ
						else if( H < TargetHitChecker.z * targetWidthInLine &&
								 H > -TargetHitChecker.z * targetWidthInLine)
						{
							hitRenderer.guiText.text = "セーフ(´・ω・｀)";
							numJudge[(int)JUDGE.SAFE]++;
							criticalCombo = 0;
							// エフェクト発生
							p_pos = SPlayer.transform.position;
							p_pos.y += 1.0f;
							GameObject ins_obj =
								(GameObject)Instantiate(effect_HitSafe,
														p_pos,
														SPlayer.transform.rotation	);
							Destroy(ins_obj, 1.0f);
							
							SPlayer.CalcCombo(false);
							SPlayer.CalcScore(ScoreSafe);
						}
						//ミス
						else
						{
							hitRenderer.guiText.text = "ミス・・・";
							numJudge[(int)JUDGE.MISS]++;
							criticalCombo = 0;
							// エフェクト発生
							p_pos = SPlayer.transform.position;
							p_pos.y += 1.0f;
							GameObject ins_obj =
								(GameObject)Instantiate(effect_Miss,
														p_pos,
														SPlayer.transform.rotation	);
							Destroy(ins_obj, 1.0f);
							
							SPlayer.CalcCombo(false);
							SPlayer.CalcScore(ScoreMiss);
						}
						
						//通過したと判定させる
						targetArray[lineID].pastArray[i] = true;
						targetArray[lineID].seek++;
						Debug.Log("pass");
						//クリティカルのコンボ計算
						if(criticalCombo > criticalComboMax)
							criticalComboMax = criticalCombo;
					}
				}
				else
					break;
			}
		}
		return retValue;
	}
	
	//ラインの本数を取得
	public int GetNumLine()	{	return numPoint-1;	}
	//ターゲットの個数を取得
	public int GetNumTarget()	{	return numTarget;	}
	
	//ラインの始点を取得
	public Vector3 GetLineStartPoint(int id)	{	return lineData[id];	}
	//ラインの方向を取得
	public Vector3 GetLineDirection(int id)	{	return lineDir[id];	}
	//ラインの太さを取得
	public float GetLineWidth()	{	return lineWidth;	}
	//プレイヤーがどのライン上にいるのか取得
	public int GetPlyaerLineID()	{	return wherePlayer;	}
	//プレイヤーが最終地点にいるか取得
	public bool isLastpoint()	{	return lastPoint;	}
	
	//各判定の数を取得
	public int GetNumJudge(int kind)	{	return numJudge[kind];	}
	public int GetNumCriticalCombo()	{	return criticalComboMax;}
	public int GetStageID()				{	return stageID;	}
	public bool isExist()				{	return exist;	}
}
