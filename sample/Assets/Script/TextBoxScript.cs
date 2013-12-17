using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class TextBoxScript : MonoBehaviour {
	
	int actID;
	int nextAct;
	int numStatement;
	
	int strIdx;
	int charIdx;
	
	int readCnt;
	int READCNT;
	
	int[] balloonArray;
	int[] balloonSize;
	string[] strArray;
	string renderString;
	string renderBase;
	bool initialized;
	bool valid;
	bool textinsert=false;
	
	TextStyleBaseScript TextBox1;
	TextStyleBaseScript TextBox2;
	TextStyleBaseScript TextBox3;
	
	// Use this for initialization
	void Start () {
		TextBox1 = GameObject.Find("Text1").GetComponent<TextStyleBaseScript>();
		TextBox1.SetPos(100.0f,500.0f);
		TextBox2 = GameObject.Find("Text2").GetComponent<TextStyleBaseScript>();
		TextBox2.SetPos(100.0f,600.0f);
		TextBox3 = GameObject.Find("Text3").GetComponent<TextStyleBaseScript>();
		TextBox3.SetPos(100.0f,700.0f);
		//---------------------------//
		//パラメータの初期化
		numStatement 	= 0;
		strIdx 			= 0;
		charIdx			= 0;
		readCnt 		= 0;
		READCNT 		= 2;
		initialized = false;
		valid = true;
		
		initialized = Initialize();
	}
	bool Initialize()
	{
		string[] strBase;
		string[] split;
		ArrayList data = new ArrayList();
		string tmp;
		
		//データIDを取得
		GameSystemScript gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
		int dataID = gamesys.GetActID();
		//会話IDを取得
        FileInfo fi = new FileInfo(Application.dataPath+"/GameData/act.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
		actID = 0;
        while( sr.Peek() != -1 ){
			tmp = sr.ReadLine();
			strBase = tmp.Split(';');
			split = strBase[0].Split(',');
			if(int.Parse(split[0]) == dataID)
			{
				actID = int.Parse(split[1]);
				nextAct = int.Parse(split[2]);
				Debug.Log("actID"+actID);
				Debug.Log("nextAct"+nextAct);
				break;
			}
        }
       	sr.Close();
		//会話データを取得
		char[] tmpCharArray;
        FileInfo fiData = new FileInfo(Application.dataPath+"/GameData/actData/"+actID+".txt");
        StreamReader srData = new StreamReader(fiData.OpenRead(), Encoding.GetEncoding("Shift_JIS"));
        while( srData.Peek() != -1 ){
			tmp = srData.ReadLine();
			strBase = tmp.Split(';');
			split = strBase[0].Split(',');
			data.Add(int.Parse(split[0]));	//Emotion
			data.Add(int.Parse(split[2]));	//Action
			data.Add(int.Parse(split[2]));	//TextBoxID
			data.Add(int.Parse(split[3]));	//TextBoxSize
			data.Add(split[4]);				//Text1
        }
       	srData.Close();
		//会話データ格納
		numStatement = (int)(data.Count/5);//セリフ数
		Debug.Log("talk_num:"+numStatement);
		balloonArray 	= new int[numStatement];
		strArray 		= new string[numStatement];
		for(int i=0; i<numStatement; i++)
		{
			balloonArray[i] = (int)data[i*5+2];
			strArray[i] 	= data[i*5+4].ToString();
			Debug.Log("balloonArray["+i+"]:"+balloonArray[i]);
			Debug.Log("strArray["+i+"]:"+strArray[i]);
		}
		renderBase = strArray[0];//.ToString();
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		if(initialized)
		{
			if(textinsert == true)
			{
				if(strIdx < numStatement)
				{
					if(string.Compare(renderString, renderBase) == 0)
					{
						if(Input.GetMouseButtonDown(0))
						{
							renderString = "";
							strIdx++;
							if(strIdx < numStatement)
								renderBase = strArray[strIdx].ToString();
							else
								valid = false;
							charIdx = 0;
						}
					}
					else
					{
						if(readCnt>0)
							readCnt--;
						else
						{
							renderString += renderBase.Substring(charIdx, 1);
							charIdx=charIdx+1;
							readCnt = READCNT;
						}
					}
				
					//textset
					TextBox1.SetText(renderString);
					//Debug.Log("renderString:"+renderString);
				}
			}
		}
		else
		{
			initialized = Initialize();
		}
	}
	
	
	public bool isValid()
	{
		return valid;	
	}
	
	public int GetNextAct()
	{
		return nextAct;
	}
	
	public int[] GetBalloonArray(){
		return balloonArray;
	}
	
	public int[] GetBalloonSizeArray(){
		return balloonArray;
	}
	
	public string[] GetStrArray(){
		return strArray;
	}
	
	public int GetNumStatement(){
		return numStatement;
	}
}

