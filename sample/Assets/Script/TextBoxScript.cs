using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class TextBoxScript : MonoBehaviour {
	
	int actID;
	int numStatement;
	int strIdx;
	int[] balloonArray;
	string[] strArray;
	string renderString;
	string renderBase;
	bool initialized;
	
	
	// Use this for initialization
	void Start () {
		//---------------------------//
		//パラメータの初期化
		numStatement 	= 0;
		strIdx 			= 0;
		initialized = false;
		
		Initialize();
	}
	void Initialize()
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
        while( sr.Peek() != -1 ){
			tmp = sr.ReadLine();
			strBase = tmp.Split(';');
			split = strBase[0].Split(',');
			if(int.Parse(split[0]) == dataID)
			{
				actID = int.Parse(split[1]);
				break;
			}
        }
       	sr.Close();
		//会話データを取得
        FileInfo fiData = new FileInfo(Application.dataPath+"/GameData/actData/"+actID+".txt");
        StreamReader srData = new StreamReader(fiData.OpenRead());
        while( srData.Peek() != -1 ){
			tmp = srData.ReadLine();
			strBase = tmp.Split(';');
			split = strBase[0].Split(',');
			data.Add(int.Parse(split[0]));	//吹き出しID
			data.Add(split[1]);				//文章
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
		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(initialized)
		{
			int charIdx = 0;
			if(strIdx < numStatement)
			{
				if(string.Compare(renderString, renderBase) == 0 && Input.GetKeyUp(KeyCode.Return))
				{
					strIdx++;
					renderBase = strArray[strIdx].ToString();
					charIdx = 0;
				}
				else
				{
					renderString = renderBase.Substring(0, charIdx);
					charIdx++;
				}
				
				//テキスト表示
				//GUIText gui = ((GameObject)GameObject.Find("TextBox")).GetComponent<GUIText>();
				guiText.text = renderString;
			}
		}
		else
		{
			Initialize();
		}
		Debug.Log(numStatement+","+renderBase);
	}
}
