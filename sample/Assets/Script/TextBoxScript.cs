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
	string[] strArray;
	string renderString;
	string renderBase;
	bool initialized;
	bool valid;
	
	
	// Use this for initialization
	void Start () {
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
			data.Add(int.Parse(split[0]));	//吹き出しID
			data.Add(split[1]);				//文章
			Debug.Log("log:"+split[1].ToString());
        }
       	srData.Close();
		//会話データ格納
		numStatement = (int)(data.Count/2);//セリフ数
		balloonArray 	= new int[numStatement];
		strArray 		= new string[numStatement];
		for(int i=0; i<numStatement; i++)
		{
			balloonArray[i] = (int)data[i*2];
			strArray[i] 	= data[i*2+1].ToString();
		}
		renderBase = strArray[0];//.ToString();
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		if(initialized)
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
				
				//テキスト表示
				guiText.text = renderString;
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
}
