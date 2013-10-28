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

	public GameObject target;	
	public GameObject line;
	//public LineScript line;
	
	//線の生成
	private void CreateLine(Vector3 prev, Vector3 point1, Vector3 point2, Vector3 next)
	{
		//line = new GameObject();
		//line.AddComponent(typeof(MeshFilter));
		//LineScript.SetData(prev, point1, point2, next);
		//((GameObject)Instantiate(Resources.Load("Button_UP"))).GetComponent<Button2D>(); 
		LineScript obj = ((GameObject)Instantiate(line)).GetComponent<LineScript>();	
		//line = new LineScript();
		//Instantiate(line);
		obj.SetData(prev, point1, point2, next);
	}
	//ターゲットの生成
	void CreateTarget(Vector3 linePoint1, Vector3 linePoint2, float offset)
	{
	}
	
	// Use this for initialization
	void Start () {
		char state = (char)STATE.NUM_STATE;
		String str;
		String[] strBase;
		String[] split;
		
		ArrayList data = new ArrayList();
		Vector3[] lineData;
		int numPoint; 
		Vector3 dummy = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		//ラインオブジェクト
		//LineScript[] obj;
		//床オブジェクトのサイズ取得（スケール＊5がpositionに入る？）
		GameObject floor = GameObject.Find("Floor");
		float stageHorizontal = floor.transform.localScale.x * 5.0f;
		float stageVertical = floor.transform.localScale.z * 5.0f;
		
		//ステージファイルの読み込み
		FileInfo fi = new FileInfo(Application.dataPath+"/GameData/stage.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 )
		{
			str = sr.ReadLine();
			strBase = str.Split(';');
			split = strBase[0].Split(',');
			Debug.Log(split[0]);
			
			switch(state)
			{
			case (char)STATE.LINE:
				//マーカー目印がきたらラインの生成
				if(split[0] == "Marker")
				{
					//頂点数算出
					numPoint = (int)(data.Count / 3);
					lineData = new Vector3[numPoint];
					for(int i=0; i<numPoint; i++)
					{
						lineData[i].x = (float)data[(i*3)+0] * stageHorizontal;
						lineData[i].y = (float)data[(i*3)+1];
						lineData[i].z = (float)data[(i*3)+2] * stageVertical;
					}
					//必要分ラインを生成
					//obj = new LineScript[numPoint-1];
					Vector3[] inValue = new Vector3[4];
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
						CreateLine(inValue[0], inValue[1], inValue[2], inValue[3]);
					}
					//for(int i=0; i<1000; i++)
					//	CreateLine(dummy, dummy, dummy, dummy);
					state = (char)STATE.TARGET;
					break;
				}
				//数値を格納
				data.Add(float.Parse(split[0]));
				data.Add(0.1f);
				data.Add(float.Parse(split[1]));
				break;
				
			case (char)STATE.TARGET:
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
}
