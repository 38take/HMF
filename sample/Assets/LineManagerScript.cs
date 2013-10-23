using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class LineManagerScript : MonoBehaviour {
	

	public GameObject target;	
	public GameObject line;
	// Use this for initialization
	void Start () {
		FileInfo fi = new FileInfo(Application.dataPath+"/stage.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 ){
            print( sr.ReadLine() );
            }
       	sr.Close();
		//Instantiate(obj, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
