using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class LineManagerScript : MonoBehaviour {
	
	private enum STATE
	{
		LINE,
		TARGET,
		NUM_STATE,
	};

	public GameObject target;	
	public GameObject line;
	
	//線の生成
	void CreateLine(Vector3 point1, Vector3 point2, Vector3 point3)
	{
	}
	//ターゲットの生成
	void CreateTarget(Vector3 linePoint1, Vector3 linePoint2, float offset)
	{
	}
	
	// Use this for initialization
	void Start () {
		char state = (char)STATE.LINE;
		String str;
		float stageHorizontal;
		float stageVertical;
		
		//ステージファイルの読み込み
		FileInfo fi = new FileInfo(Application.dataPath+"/stage.txt");
        StreamReader sr = new StreamReader(fi.OpenRead());
        while( sr.Peek() != -1 )
		{
			str = sr.ReadLine();
			switch(state)
			{
			case (char)STATE.LINE:
				
				break;
			case (char)STATE.TARGET:
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
