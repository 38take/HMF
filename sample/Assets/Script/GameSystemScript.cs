using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class GameSystemScript : MonoBehaviour {
	
	private bool valid;
	private int actID;
	private int lastAct;
	private int[] score;
	
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
	
	public bool isValid()
	{
		return valid;
	}
	
	public bool isLastAct()
	{
		return ((lastAct - actID) <= 0);
	}
}
