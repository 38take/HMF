using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class LineManagerScript : MonoBehaviour {
	
	private enum STATE
	{
		LINE,
		TARGET,
		NUM_STATE,
	};
	//各ラインのターゲット配列
	private struct TARGET_ARRAY
	{
		public Vector3[] 	targetPosArray; //ターゲット位置（X:中心ラインのオフセット Y:左右のオフセット）
		public bool[]		pastArray;		//通過したかのフラグ
	};

	public GameObject target;	
	public GameObject line;
	public GameObject lineDead;
	Vector3[] lineData;
	int[]	  lineKind;
	Vector3[] lineDir;
	TARGET_ARRAY[] targetArray;
	int numPoint; 
	int numTarget;
	int wherePlayer;
	float lineWidth; 
	bool 	lastPoint;
	
	//線の生成
	private void CreateLine(Vector3 prev, Vector3 point1, Vector3 point2, Vector3 next, int kind)
	{
		LineScript obj = new LineScript();
		switch(kind)
		{
		case 0:
			obj = ((GameObject)Instantiate(line)).GetComponent<LineScript>();	
			break;
		case 1:
			obj = ((GameObject)Instantiate(lineDead)).GetComponent<LineScript>();	
			break;
		default:
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
		Vector3 dir = lineDir[lineID]*(1.0f - offset.x) + lineDir[lineID+1]*offset.x;
		dir = Vector3.Normalize(dir);
		//算出した方向から左向きのベクトルへ変換
		float tmp = dir.z;
		dir.z = dir.x;
		dir.x = -tmp;
		//位置確定
		Vector3 pos = basePos + dir * (-offset.y) * lineWidth;//1,ofは線の太さ
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
		//パラメータ初期化
		numPoint 	= 0;
		numTarget 	= 0;
		wherePlayer = 0;
		lineID = tNum = tCnt = -1;
		lineWidth = 3.0f;
		lastPoint = false;
		//==================================================
		//ステージファイルの読み込み
		FileInfo fi = new FileInfo(Application.dataPath+"/GameData/stage.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 )
		{
			str = sr.ReadLine();
			strBase = str.Split(';');
			split = strBase[0].Split(',');
			
			switch(state)
			{
			case (char)STATE.LINE:
				//マーカー目印がきたらラインの生成
				if(split[0] == "Marker")
				{
					//頂点数算出
					numPoint = (int)(data.Count / 4);
					lineData = new Vector3[numPoint];
					lineKind = new int[numPoint];
					lineDir  = new Vector3[numPoint];
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
						targetArray[lineID].pastArray[i]		= false;
					}
					
					numTarget += tNum;
					
					data.Clear();
					lineID = -1;
					tNum = -1;
				}
				break;
				
			case (char)STATE.NUM_STATE:
				if(split[0] == "LinePoint")
					state = (char)STATE.LINE;
				break;
			default:
				break;
			}
       	}
       	sr.Close();
		//Instantiate(obj, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Vector3 CalcPlayerPos(int timer, float offset)
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
		length = length / lineLength;
		
		//基準点算出
		Vector3 basePos = lineData[lineIdx] + (lineData[lineIdx+1]-lineData[lineIdx])*length;
		//方向算出
		Vector3 dir = lineDir[lineIdx]*(1.0f-length)+lineDir[lineIdx+1]*length;
		dir = Vector3.Normalize(dir);
		float tmp = dir.x;
		dir.x = dir.z;
		dir.z = -tmp;
		//位置確定
		ret = basePos + dir * offset * lineWidth;
		
		wherePlayer = lineIdx;
		
		return ret;
	}
	
	//ラインの本数を取得
	public int GetNumLine(){
		return numPoint-1;
	}
	//ターゲットの個数を取得
	public int GetNumTarget(){
		return numTarget;
	}
	
	//ラインの始点を取得
	public Vector3 GetLineStartPoint(int id)
	{
		return lineData[id];
	}
	//ラインの方向を取得
	public Vector3 GetLineDirection(int id)
	{
		return lineDir[id];
	}
	//プレイヤーがどのライン上にいるのか取得
	public int GetPlyaerLineID()
	{
		return wherePlayer;
	}
	//プレイヤーが最終地点にいるか取得
	public bool isLastpoint()
	{
		return lastPoint;
	}
}
