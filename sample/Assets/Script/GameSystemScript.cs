using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class GameSystemScript : MonoBehaviour {
	
	private bool valid;
	private int actID;
	// Use this for initialization
	void Start () {
        FileInfo fi = new FileInfo(Application.dataPath+"/GameData/system.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 ){
            actID = int.Parse(sr.ReadLine());
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
	   	sw.Write((actID + nextAct));    // 改行
	   	sw.Flush();
	   	sw.Close(); 
	}
	
	public int GetActID()
	{
		return actID;
	}
	
	public bool isValid()
	{
		return valid;
	}
}
