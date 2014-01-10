using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class GameSystemScript : MonoBehaviour {
	
	private bool valid = false;
	private int actID = 0;
	private int lastAct;
	private int[] score;
	private String nextScene;
	
	// Use this for initialization
	void Start () {
		score = new int[3];
		
		FileInfo fi = new FileInfo(Application.dataPath+"/GameData/system.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 ){
            actID 	= int.Parse(sr.ReadLine());
			lastAct = int.Parse(sr.ReadLine());
			score[0] = int.Parse(sr.ReadLine());
			score[1] = int.Parse(sr.ReadLine());
			score[2] = int.Parse(sr.ReadLine());
            }
       	sr.Close();
		//次のシーンIDを確認
		string[] strBase;
		string[] split;
		string   tmp;
        fi = new FileInfo(Application.dataPath+"/GameData/act.txt");
        sr = new StreamReader(fi.OpenRead());
		int nextSceneID = 0;
        while( sr.Peek() != -1 ){
			tmp = sr.ReadLine();
			strBase = tmp.Split(';');
			split = strBase[0].Split(',');
			if(int.Parse(split[0]) == actID)
			{
				nextSceneID = int.Parse(split[3]);
				break;
			}
        }
       	sr.Close();
		switch(nextSceneID)
		{
		case 0:	nextScene = "Adventure";
			break;
		case 1:	nextScene = "Play";
			break;
		case 2:	nextScene = "Result";
			break;
		}
		
		valid = true;
	}
	
	public void SystemOutPut(int nextAct)
	{
    	FileInfo fi = new FileInfo(Application.dataPath+"/GameData/system.txt");
		if(fi.Exists)
			fi.Delete();
	    //write
	   	StreamWriter sw = fi.CreateText();
	   	//sw.Write(text);      // 未改行
	   	sw.WriteLine((actID + nextAct));    // 改行
	   	sw.WriteLine(lastAct);    // 改行
	   	sw.WriteLine(score[0]);    // 改行
	   	sw.WriteLine(score[1]);    // 改行
	   	sw.WriteLine(score[2]);    // 改行
		
	   	sw.Flush();
	   	sw.Close(); 
	}
	
	public int GetActID()
	{
		return actID;
	}
	public int GetScore(int id)
	{
		return score[id];
	}
	public void SetScore(int id, int value)
	{
		if(id >= 0)
			score[id] = value;
	}
	public string GetNextScene()
	{
		return nextScene;
	}
	
	public bool isValid()
	{
		return valid;
	}
	
	public bool isLastAct()
	{
		return ((lastAct - actID) <= 0);
	}
}
