using UnityEngine;
using System.Collections;

public class Tex2DBaseScript : MonoBehaviour {
	
	public bool		DrawEnable = true;
	public Texture	texture;
	public Color	color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	public Vector2	Position;
	public float	Width;
	public float	Height;
	public int		Depth;
	
	public Vector2	UVPosition;
	public float	UVWidth;
	public float	UVHeight;
	
	public int cnt;
	
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {	
		
		
	}
	
	// 描画.
	public void OnGUI () {
		
		if( !DrawEnable || texture == null ){
			return;
		}
		
		GUI.depth = Depth;
		GUI.color = color;
		
		GUI.DrawTextureWithTexCoords(
			new Rect( 
			Position.x 								* DefaultScreen.Par.x, 
			Position.y								* DefaultScreen.Par.y, 
			Width 									* DefaultScreen.Par.x, 
			Height 									* DefaultScreen.Par.y ),
			
			texture,
			
			new Rect(UVPosition.x,
			UVPosition.y,
			UVWidth,
			UVHeight )
		);
		
		// 元に戻しておく.
		GUI.color = Color.white;
		//GUI.depth = 0;
	}
	
	//座標（左上）取得 
	public virtual Vector2 GetPos (){
		return Position;
	}
	
	public virtual Color GetColor()
	{
		return color;
	}
	
	public virtual Vector2 GetScreenParPos (){
		return new Vector2( Position.x*DefaultScreen.Par.x, Position.y*DefaultScreen.Par.y );
	}
	
	public virtual Vector2 GetSize (){
		return new Vector2( Width, Height );
	}
	
	public virtual Vector2 GetScreenParSize (){
		return new Vector2( Width*DefaultScreen.Par.x, Height*DefaultScreen.Par.y );
	}
	
	public virtual Vector2 GetTexSize (){
		return new Vector2( texture.width, texture.height );
	}
	
	public virtual float GetTexRatio (){
		return (float)texture.width / (float)texture.height;
	}
	
	public virtual void SetPos	( float setX, float setY ){
		Position.x = setX;
		Position.y = setY;
	}
	
	public virtual void SetPos	( Vector2 Pos ){
		Position = Pos;
	}
	
	public virtual void SetSize	( Vector2 Size ){
		Width = Size.x;
		Height = Size.y;
	}
	
	public virtual void SetSize	( float setWidth, float setHeight ){
		Width = setWidth;
		Height = setHeight;
	}
	
	public virtual void SetSizeCenter ( Vector2 Size ){
		Vector2 Offset = new Vector2( Size.x - Width, Size.y - Height );
		Position -= Offset / 2.0f;
		Width = Size.x;
		Height = Size.y;
	}
	
	public virtual void SetColor(Color col)
	{
		color = col;
	}
	
	//Change Texture Size. You can appoint center pos(0.0f~1.0f)
	public virtual void SetSizeFlexiblePoint ( Vector2 Size, Vector2 Point ){
		Vector2 Offset = new Vector2( Size.x - Width, Size.y - Height );
		Position.x -= (Offset.x * Point.x); 
		Position.y -= (Offset.y * Point.y);
		Width = Size.x;
		Height = Size.y;
	}
	
	public void SetTexture ( Texture tex ){
		texture = tex;
	}
	
	public void SetUV ( Vector2 s_UVPos , float s_UVWidth, float s_UVHeight){	
		UVPosition = s_UVPos;
		UVWidth = s_UVWidth;
		UVHeight = s_UVHeight;
	}
	public void SetDepth(int s_Depth){
		Depth = s_Depth;
	}
	
}
