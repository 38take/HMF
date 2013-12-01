﻿using UnityEngine;
using System.Collections;

public class Tex2DGUITextureScript : MonoBehaviour {
	
	public Texture[]		m_Textures;
	private int				m_TexID;
	
	public bool		DrawEnable = true;
	public Color	color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	public Vector2	Position;
	public float	Width;
	public float	Height;
	
	public Vector2	UVPosition;
	public float	UVWidth;
	public float	UVHeight;
	
	public int cnt;
	
	// Use this for initialization
	void Start () {
		m_TexID = 0;
		guiTexture.color = color;
		guiTexture.texture = m_Textures[m_TexID];
		
		RestoreTextureRect();
	}

	// Update is called once per frame
	void Update () {	
		
		
	}
	
	//--------------------------------------------------------------------//
	//ゲッター
	//座標（左上）取得 
	public virtual Vector2 GetPos (){
		return Position;
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
		return new Vector2( m_Textures[m_TexID].width, m_Textures[m_TexID].height );
	}
	
	public virtual float GetTexRatio (){
		return (float)m_Textures[m_TexID].width / (float)m_Textures[m_TexID].height;
	}
	
	//--------------------------------------------------------------------//
	//セッター
	public virtual void SetPos	( float setX, float setY ){
		Position.x = setX;
		Position.y = setY;
		RestoreTextureRect();
	}
	
	public virtual void SetPos	( Vector2 Pos ){
		Position = Pos;
		RestoreTextureRect();
	}
	
	public virtual void SetSize	( Vector2 Size ){
		Width = Size.x;
		Height = Size.y;
		RestoreTextureRect();
	}
	
	public virtual void SetSize	( float setWidth, float setHeight ){
		Width = setWidth;
		Height = setHeight;
		RestoreTextureRect();
	}
	
	public virtual void SetSizeCenter ( Vector2 Size ){
		Vector2 Offset = new Vector2( Size.x - Width, Size.y - Height );
		Position -= Offset / 2.0f;
		Width = Size.x;
		Height = Size.y;
		RestoreTextureRect();
	}
	
	//Change Texture Size. You can appoint center pos(0.0f~1.0f)
	public virtual void SetSizeFlexiblePoint ( Vector2 Size, Vector2 Point ){
		Vector2 Offset = new Vector2( Size.x - Width, Size.y - Height );
		Position.x -= (Offset.x * Point.x); 
		Position.y -= (Offset.y * Point.y);
		Width = Size.x;
		Height = Size.y;
		RestoreTextureRect();
	}
	
	public void SwitchTexture ( int id ){
		if(id < m_Textures.Length) {
			m_TexID = id;
			guiTexture.texture = m_Textures[m_TexID];
		}
	}
	
	private void RestoreTextureRect(){
		guiTexture.pixelInset = new Rect(	Position.x 	* DefaultScreen.Par.x, 
											Position.y 	* DefaultScreen.Par.y,
											Width 		* DefaultScreen.Par.x, 	
											Height 		* DefaultScreen.Par.y); 
	}
}