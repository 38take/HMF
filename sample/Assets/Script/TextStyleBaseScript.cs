using UnityEngine;
using System.Collections;

public class TextStyleBaseScript : MonoBehaviour {
	
	public bool		DrawEnable = true;
	public string	text;
	public Color	color = new Color( 0.2f, 0.2f, 0.2f, 1.0f );
	public GUIStyle Style = new GUIStyle();
	public Vector2	Position;
	public int		Depth = 5;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		SetText("grgerg");
	}
	
	void OnGUI	(){
		if( !DrawEnable ) return;
		
		GUIStyle style = new GUIStyle( Style );
		style.fontSize = (int)((float)style.fontSize * DefaultScreen.FontPar);
		
		GUI.depth = Depth;
		GUI.color = color;
		GUI.Label( new Rect( 
			Position.x 							* DefaultScreen.Par.x, 
			(DefaultScreen.Height-Position.y)	* DefaultScreen.Par.y, 
			0.0f, 
			0.0f ),
			text,
			style);
		// 元に戻しておく.
		GUI.color = Color.white;
	}
	
	public virtual Vector2 GetPos (){
		return Position;
	}
	
	public virtual int GetFontSize (){
		return Style.fontSize;
	}
	
	public virtual void SetPos	( float setX, float setY ){
		Position.x = setX;
		Position.y = setY;
	}
	
	public virtual void SetPos	( Vector2 Pos ){
		Position = Pos;
	}
	
	public virtual void SetFontSize	( int setFontSize ){
		Style.fontSize = setFontSize;
	}
	
	public virtual void SetText ( string Str ){
		text = Str;
	}
}
