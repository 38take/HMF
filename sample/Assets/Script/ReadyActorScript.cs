using UnityEngine;
using System.Collections;

public class ReadyActorScript : MonoBehaviour {
	
	private bool valid = true;
	
	private bool initialized = false;
	
	int timer = 60;
	int state = 0;
	
	//外部オブジェクト 
	Tex2DGUITextureScript SCount;
	Tex2DGUITextureScript SStart;
	

	// Use this for initialization
	void Start () {
		SCount = ((GameObject)GameObject.Find("StartCount")).GetComponent<Tex2DGUITextureScript>();
		SStart = ((GameObject)GameObject.Find("Start")).GetComponent<Tex2DGUITextureScript>();
		
		SCount.SetRenderFlag(false);
		SStart.SetRenderFlag(false);
	}
	
	void Initialize()
	{
		SCount.SetRenderFlag(true);
		SCount.SetSize(362.0f, 234.0f);
		SCount.SetPos(300.0f, 300.0f);
		SCount.RestoreTextureRect();
		SCount.SwitchTexture(state);
		
		SStart.SetSize(550.0f, 250.0f);
		SStart.SetPos(237.0f, 259.0f);
		SStart.RestoreTextureRect();
		//SStart.SetRenderFlag(false);
		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(valid)
		{
			if(!initialized)
				Initialize();
			else{
				if(timer > 0)
					timer--;
				else
				{
					state++;
					if(state < 3)
						SCount.SwitchTexture(state);
					else if(state < 4)
					{
						SCount.SetRenderFlag(false);
						SStart.SetRenderFlag(true);
					}
					else
					{
						SStart.SetRenderFlag(false);
						InValidate();
					}
					//60フレーム分カウント
					timer = 60;
				}
			}
		}
	}
	
	//有効かどうかのチェック 
	public bool isValid()
	{
		return valid;
	}
	//無効化 
	public void InValidate()
	{
		valid = false;
	}
}
