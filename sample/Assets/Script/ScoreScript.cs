using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {
	
	private	float Score = 0.0f;
	// Use this for initialization
	void Start () {
		guiText.text = "スコア：" + Score;
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = "スコア：" + Score;
	}
	
	public void AddScore( float scr )
	{
		Score += scr;
	}
	public void Test()
	{
		Debug.Log("てす");
	}
	//スコアの取得
	public float GetScore()
	{
		return Score;
	}
}
