using UnityEngine;
using System.Collections;

public class ConcentrateScript : MonoBehaviour {
	
	Tex2DGUITextureScript SGaugeTexture;
	
	private float			concentration;
	private float			concentrationGauge;
	private float 			CONCENTRATION_MAX;
	private int				concentrationMoveCnt;
	private int				CONCENTRATION_MOVECNT;
	
	public int MaxWidth = 50;
	public int MaxHeight = 468;
	
	public  GameObject		obj_CMonitor;
	// Use this for initialization
	void Start () {
		
		concentration = 0.0f;
		concentrationGauge = 0.0f;
		concentrationMoveCnt = 0;
		CONCENTRATION_MOVECNT = 30;
		CONCENTRATION_MAX = 100.0f;
		
		obj_CMonitor    = (GameObject)GameObject.Find("ConcentrateMonitor");
		SGaugeTexture = ((GameObject)GameObject.Find("GaugeTexture")).GetComponent<Tex2DGUITextureScript>();
		SGaugeTexture.SetPos(SGaugeTexture.GetPos());
		SGaugeTexture.SetSize((float)MaxWidth, (float)MaxHeight);
		SGaugeTexture.SetRenderFlag(true);
	}
	
	// Update is called once per frame
	void Update () {
	
		if( concentration != concentrationGauge )
		{
			if(concentrationMoveCnt > 0)
				concentrationMoveCnt--;
			else if(concentrationMoveCnt == 0)
			{
				concentrationGauge += (concentration - concentrationGauge)*0.2f;
				if(concentration == concentrationGauge)
					concentrationMoveCnt = -1;
			}
			else
				concentrationMoveCnt = CONCENTRATION_MOVECNT;
		}
		SGaugeTexture.SetSize((float)(MaxWidth/2), (concentrationGauge / CONCENTRATION_MAX)*(float)MaxHeight);
		obj_CMonitor.guiText.text = ((int)concentration).ToString() + ":" + ((int)concentrationGauge).ToString();
	}
	
	public bool AddConcentrate(float Value)
	{
		concentration += (float)Value;
		if(concentration >= CONCENTRATION_MAX) 	concentration = CONCENTRATION_MAX;
		if(concentration < 0.0f)				concentration = 0.0f;
		if(concentration != concentrationGauge)	concentrationMoveCnt = CONCENTRATION_MOVECNT;
		
		return (concentration == CONCENTRATION_MAX);
	}
	
	public bool isExist()
	{
		return ((int)concentration > 0);
	}
	public float GetConcentration()
	{
		return concentration;
	}
}
