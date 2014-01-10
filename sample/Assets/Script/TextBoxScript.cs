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
	int charIdx2;
	int charIdx3;
	
	int readCnt;
	int READCNT;
	
	int[] emotionArray;
	int[] actionArray;
	int[] balloonArray;
	int[] balloonSize;
	
	int[] InfoWinFlg;
	int[] InfoContain;
	//
	string[] strArray;
	string[] strArray2;
	string[] strArray3;
	//string
	string renderString;
	string renderString2;
	string renderString3;
	string renderBase;
	string renderBase2;
	string renderBase3;
	//outflg
	bool[] outflag;
	
	bool initialized = false;
	bool valid = false;
	bool textinsert=false;
	
	TextStyleBaseScript TextBox1;
	TextStyleBaseScript TextBox2;
	TextStyleBaseScript TextBox3;
	GUIManagerScript	SGUI;
	
	GameSystemScript gamesys;
	
	// Use this for initialization
	void Start () {
		TextBox1 = GameObject.Find("Text1").GetComponent<TextStyleBaseScript>();
		TextBox1.SetPos(60.0f,275.0f);
		TextBox2 = GameObject.Find("Text2").GetComponent<TextStyleBaseScript>();
		TextBox2.SetPos(60.0f,200.0f);
		TextBox3 = GameObject.Find("Text3").GetComponent<TextStyleBaseScript>();
		TextBox3.SetPos(60.0f,120.0f);
		SGUI 	 = GameObject.Find("GUIManager").GetComponent<GUIManagerScript>();
		gamesys = ((GameObject)GameObject.Find("GameSystem")).GetComponent<GameSystemScript>();
		//---------------------------//
		//パラメータの初期化
		numStatement 	= 0;
		strIdx 			= 0;
		charIdx			= 0;
		charIdx2		= 0;
		charIdx3		= 0;
		readCnt 		= 0;
		READCNT 		= 2;
	}
	bool Initialize()
	{
		string[] strBase;
		string[] split;
		ArrayList data = new ArrayList();
		string tmp;
		
		//データIDを取得
		int dataID = gamesys.GetActID();
		Debug.Log("dataID:"+dataID);
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
				//Debug.Log("actID:"+actID);
				//Debug.Log("nextAct:"+nextAct);
				break;
			}
        }
       	sr.Close();
		//会話データを取得
		char[] tmpCharArray;
		
		//GUIText testText2 = ((GameObject)GameObject.Find("test")).GetComponent<GUIText>();
		string path = Application.dataPath+"/GameData/actData/"+actID+".txt";
        FileInfo fiData = new FileInfo(Application.dataPath+"/GameData/actData/"+actID+".txt");
        StreamReader srData = new StreamReader(/*path, */fiData.OpenRead(), Encoding.Default);
        while( srData.Peek() != -1 ){
			tmp = srData.ReadLine();
		//	testText2.text = tmp;
			strBase = tmp.Split(';');
			split = strBase[0].Split(',');
			
			data.Add(int.Parse(split[0]));	//Emotion
			data.Add(int.Parse(split[1]));	//Action
			data.Add(int.Parse(split[2]));	//TextBoxID
			data.Add(int.Parse(split[3]));	//TextBoxSize
			data.Add(split[4]);				//Text1
			data.Add(split[5]);				//Text1
			data.Add(split[6]);				//Text1
			data.Add(int.Parse(split[7]));				//InfoWindowFlg
			data.Add(int.Parse(split[8]));				//InfoWindowContain	
			data.Add(int.Parse(split[9]));				//OutFlag
        }
       	srData.Close();
		//会話データ格納
		numStatement = (int)(data.Count/10);//セリフ数
		//Debug.Log("talk_num:"+numStatement);
		actionArray		= new int[numStatement];
		emotionArray	= new int[numStatement];
		balloonArray 	= new int[numStatement];
		balloonSize		= new int[numStatement];
		strArray 		= new string[numStatement];
		strArray2		= new string[numStatement];
		strArray3		= new string[numStatement];
		InfoWinFlg		= new int[numStatement];
		InfoContain		= new int[numStatement];
		outflag			= new bool[numStatement];
		
		
		for(int i=0; i<numStatement; i++)
		{
			emotionArray[i]	= (int)data[i*10+0];
			actionArray[i]	= (int)data[i*10+1];
			balloonArray[i]	= (int)data[i*10+2];
			balloonSize[i]	= (int)data[i*10+3];
			strArray[i] 	= data[i*10+4].ToString();
			strArray2[i]	= data[i*10+5].ToString();
			strArray3[i]	= data[i*10+6].ToString();
			InfoWinFlg[i]	= (int)data[i*10+7];
			InfoContain[i]	= (int)data[i*10+8];
			if((int)data[i*10+9]==0){
				outflag[i] = false;
			}
			else{
				outflag[i] = true;
			}
			//Debug.Log("OutFlag["+i+"]:"+outflag[i]);
		}
		renderBase = strArray[0];//.ToString();
		renderBase2 = strArray2[0];//.ToString();
		renderBase3 = strArray3[0];//.ToString();
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		if(valid)
		{
			if(initialized)
			{
				switch(balloonSize[strIdx]){
				case 0:
					TextBox1.SetPos(70,((DefaultScreen.Height/10)*2)+20);
					TextBox2.SetPos(70,260);
					TextBox3.SetPos(70,240);
					break;
				case 1:
					TextBox1.SetPos(70,((DefaultScreen.Height/10)*2)+40);
					TextBox2.SetPos(70,((DefaultScreen.Height/10)*2)-10);
					TextBox3.SetPos(70,190);
					break;
				case 2:
					TextBox1.SetPos(70,((DefaultScreen.Height/10)*2)+70);
					TextBox2.SetPos(70,((DefaultScreen.Height/10)*2)+20);
					TextBox3.SetPos(70,((DefaultScreen.Height/10)*2)-30);
					break;
				default:
					Debug.Log("Fukidashi No is Nothing!");
					break;
				}
				
				if(textinsert == true)
				{
					if(strIdx < numStatement)
					{
						if(string.Compare(renderString3, renderBase3) == 0)
						{
							if(Input.GetMouseButtonDown(0))
							{
								//掃ける処理
								if(outflag[strIdx])
									Invalidate();
								renderString = "";
								renderString2 = "";
								renderString3 = "";
								renderBase = "";
								renderBase2 = "";
								renderBase3 = "";
								strIdx++;
								textinsert = false;
								if(strIdx < numStatement){
									renderBase 	= strArray[strIdx].ToString();
									renderBase2 = strArray2[strIdx].ToString();
									renderBase3 = strArray3[strIdx].ToString();
								}else{
									strIdx = 0;
									valid = false;
									initialized = false;
								}
								charIdx = 0;
								charIdx2 = 0;
								charIdx3 = 0;
							}
						}
						else
						{
							if(readCnt>0)
								readCnt--;
							else
							{
								if(string.Compare(renderString,renderBase)!=0){
									renderString += renderBase.Substring(charIdx, 1);
									charIdx=charIdx+1;
								}
								else if(string.Compare(renderString,renderBase)==0 &&
									string.Compare(renderString2,renderBase2)!=0){
									renderString2 += renderBase2.Substring(charIdx2, 1);
									charIdx2=charIdx2+1;
								}
								else if(string.Compare(renderString,renderBase)==0 &&
									string.Compare(renderString2,renderBase2)==0 &&
									string.Compare(renderString3,renderBase3)!=0){
									renderString3 += renderBase3.Substring(charIdx3, 1);
									charIdx3=charIdx3+1;
								}
								readCnt = READCNT;
							}
						}
						//textset
						TextBox1.SetText(renderString);
						TextBox2.SetText(renderString2);
						TextBox3.SetText(renderString3);
						//Debug.Log("renderString:"+renderString);
					}
				}
			}
			else{
				if(gamesys.isValid())
					initialized = Initialize();
			}
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
	
	public int[] GetEmotionArray(){
		return emotionArray;
	}
	
	public int[] GetActionArray(){
		return actionArray;
	}
	
	public int[] GetBalloonArray(){
		return balloonArray;
	}
	
	public int[] GetBalloonSizeArray(){
		return balloonSize;
	}
	
	public int[] GetInfoWinInsertFlg(){
		return InfoWinFlg;
	}
	
	public int[] GetInfoWinContainFlg(){
		return InfoContain;
	}
	
	public string[] GetStrArray(){
		return strArray;
	}
	
	public int GetNumStatement(){
		return numStatement;
	}
	
	public void SetTextInsertFlg(bool s_flg){
		textinsert = s_flg;
	}
	
	public bool GetTextInsertFlg(){
		return textinsert;
	}
	
	public bool GetOutFlag(int id){
		if(id < outflag.Length)
			return outflag[id];
		return false;
	}
	public void Validate()
	{
		valid = true;
		SGUI.Resume();
	}
	public void Invalidate()
	{
		valid = false;//自身を無効化
		SGUI.Stop();
	}
}


