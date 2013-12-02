using UnityEngine;
using System.Collections;

public class ComboScript : MonoBehaviour {
	
	public GameObject	obj_ComboRenderer;
	ComboRendererScript	SCombeRenderer;
	public int	renderTimer;
	public int Width;
	public int Height;
	
	int numCombo;
	int validCnt;
	int VALID_CNT;
	GameObject comboText;
	
	// Use this for initialization
	void Start () {
		numCombo = 0;
		comboText = (GameObject)GameObject.Find("ComboTextBox");
		SCombeRenderer = null;
	}
	
	// Update is called once per frame
	void Update (){
		comboText.guiText.text = "Combo : " + numCombo.ToString();
	}
	
	//きれたかどうかでコンボ数変更
	public void Notice(bool cut)
	{
		if(SCombeRenderer != null) 
		{
			SCombeRenderer.Release();
			SCombeRenderer = null;
		}
		
		if(cut)	
		{
			numCombo++;
			GameObject obj = (GameObject)Instantiate(obj_ComboRenderer, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
			
			SCombeRenderer = obj.GetComponent<ComboRendererScript>();
			SCombeRenderer.SetData(numCombo, renderTimer, Width, Height);
		}
		else 	
			numCombo = 0;
	}
	
	public int GetNumCombo()
	{
		return numCombo;
	}
}
