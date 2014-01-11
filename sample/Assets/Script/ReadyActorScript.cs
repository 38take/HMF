using UnityEngine;
using System.Collections;

public class ReadyActorScript : MonoBehaviour {
	
	private bool valid = true;
	
	private bool initialized = false;
	
	int timer = 30;
	int state = 0;
	bool	count = false;
	bool	label = false;
	
	//外部オブジェクト 
	Tex2DGUITextureScript SCount;
	Tex2DGUITextureScript SStart;
	Tex2DGUITextureScript SRequestNo;
	Tex2DGUITextureScript SRequest;
	

	// Use this for initialization
	void Start () {
		SCount = ((GameObject)GameObject.Find("StartCount")).GetComponent<Tex2DGUITextureScript>();
		SStart = ((GameObject)GameObject.Find("Start")).GetComponent<Tex2DGUITextureScript>();
		SRequestNo = ((GameObject)GameObject.Find("RequestNo")).GetComponent<Tex2DGUITextureScript>();
		SRequest = ((GameObject)GameObject.Find("Request")).GetComponent<Tex2DGUITextureScript>();
		
		SCount.SetRenderFlag(false);
		SStart.SetRenderFlag(false);
		SRequestNo.SetRenderFlag(true);
		SRequest.SetRenderFlag(true);
		SRequest.SetColor(new Color(0.5f, 0.5f, 0.5f, 0.0f));
		SRequestNo.SetColor(new Color(0.5f, 0.5f, 0.5f, 0.0f));
	}
	
	void Initialize()
	{
		SCount.SetSize(362.0f, 234.0f);
		SCount.SetPos(300.0f, 300.0f);
		SCount.RestoreTextureRect();
		SCount.SwitchTexture(state);
		
		SStart.SetSize(550.0f, 250.0f);
		SStart.SetPos(237.0f, 259.0f);
		SStart.RestoreTextureRect();
		
		LineManagerScript SLine = ((GameObject)GameObject.Find("LineManager")).GetComponent<LineManagerScript>();
		SRequest.SwitchTexture(SLine.GetStageID()-1);
		SRequestNo.SwitchTexture(SLine.GetStageID()-1);
		//SStart.SetRenderFlag(false);
		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		Color col1, col2;
		if(valid)
		{
			if(!initialized)
				Initialize();
			else{
				
				if(timer > 0)
					timer--;
				else
				{
					if(count)
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
					else
					{
						col1 = SRequest.GetColor();
						col2 = SRequestNo.GetColor();
						if(label)
						{
							if(col1.a <= 0.0f && col2.a <= 0.0f)
							{
								count = true;
								timer = 60;
								SCount.SetRenderFlag(true);
							}
							else
							{
								col1.a -= 0.01f;
								col2.a -= 0.01f;
								SRequest.SetColor(col1);
								SRequestNo.SetColor(col2);
							}
						}
						else
						{
							//ここ
							if(col2.a < 1.0f)
								col2.a += 0.025f;
							else if(col1.a < 1.0f)
								col1.a += 0.025f;
							else
							{
								label = true;
								timer = 30;
							}	
							SRequest.SetColor(col1);
							SRequestNo.SetColor(col2);
						}
					}
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
