using UnityEngine;
using System.Collections;

public class SyuCyuSenController: MonoBehaviour {
	
	Tex2DGUITextureScript SyuCyuSen;
	int timer;
	int id;
	bool switchflg;

	// Use this for initialization
	void Start () {
		
		SyuCyuSen = ((GameObject)GameObject.Find("SyuChooSen")).GetComponent<Tex2DGUITextureScript>();
		timer = 3;
		id = 0;
		switchflg = false;
		SyuCyuSen.SetRenderFlag(switchflg);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(switchflg == true){

			if(timer > 0){
				timer--;
				
			}else{
				id=(id+1)%2;
				SyuCyuSen.SwitchTexture(id);
				timer = 2;
			}
			
		}
		
	}
	
	public void SyuCyuSenSwitch(bool flg){
		switchflg = flg;
		SyuCyuSen.SetRenderFlag(switchflg);
	}
	
	
}

