using UnityEngine;
using System.Collections;

public class ComboScript : MonoBehaviour {
	
	int numCombo;
	int validCnt;
	int VALID_CNT;
	GameObject comboText;
	
	// Use this for initialization
	void Start () {
		numCombo = 0;
		comboText = (GameObject)GameObject.Find("ComboTextBox");
	}
	
	// Update is called once per frame
	void Update (){
		
		comboText.guiText.text = "Combo : " + numCombo.ToString();
	}
	
	//きれたかどうかでコンボ数変更
	public void Notice(bool cut)
	{
		if(cut)	numCombo++;
		else 	numCombo = 0;
	}
	
	public int GetNumCombo()
	{
		return numCombo;
	}
}
