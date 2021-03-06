﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]

public class CutEdgeScript : MonoBehaviour {
	
	Mesh mesh;
	MeshFilter meshFilter;
	Mesh meshLeft;
	MeshFilter meshFilterLeft;
	
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;
	
	Vector3 bottomDir;	//後ろ側の点の方向
	Vector3 bottomDirPure;
	Vector3 topDir;		//先端側の点の方向
	
	bool	adjustBottom;
	bool	adjustTop;
	
	public float MaxWidth;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0.0f, 0.15f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMesh();
	}
	private void UpdateMesh()
	{
		if(mesh != null)
		{	
			//頂点位置の設定
			if(adjustBottom)
			{
				vertices[0] = vertices[0] + (bottomDir * 0.1f);
				vertices[2] = vertices[2] + (-bottomDir * 0.1f);
				if(Vector3.Distance(vertices[0], vertices[2]) > MaxWidth)
					adjustBottom = false;
			}
			if(adjustTop)
			{
				vertices[1] = vertices[1] + (topDir  * 0.1f);
				vertices[3] = vertices[3] + (-topDir  * 0.1f);
				if(Vector3.Distance(vertices[1], vertices[3]) > MaxWidth)
					adjustTop = false;
			}
			//頂点・インデックス・UV値の設定
			mesh.vertices = vertices;
			//法線・当たり判定の再計算
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();
			//メッシュの設定
			meshFilter.sharedMesh = mesh;
		}
	}
	
	public void SetMesh(Vector3 currentPos, Vector3 prevPos, Vector3 bottomPos)
	{
		mesh 			= new Mesh();
		meshFilter 		= (MeshFilter)GetComponent("MeshFilter");
		mesh.Clear();
		//各ポイントでの方向を算出
		Vector3 bottomdir 	= Vector3.Normalize(prevPos - bottomPos);
		Vector3 prevDir 	= Vector3.Normalize(currentPos - prevPos);		
		bottomDir = Vector3.Normalize(bottomdir + prevDir);
		bottomDirPure = bottomDir;
		
		float tmp;
		tmp = bottomDir.z;
		bottomDir.z = bottomDir.x;
		bottomDir.x = -tmp;
		bottomDir.y = 0.0f;
		
		//頂点位置の設定
		vertices = new Vector3[4];
		vertices[0] = prevPos;
		vertices[1] = currentPos;
		vertices[2] = prevPos;
		vertices[3] = currentPos;
		//インデックス
		triangles = new int[6];
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 2;
		triangles[4] = 1;
		triangles[5] = 3;
		//UV値
		uvs = new Vector2[4];
		uvs[0] = Vector2.zero;
		uvs[1] = Vector2.up;
		uvs[2] = Vector2.right;
		uvs[3] = new Vector2(1.0f, 1.0f);
		//頂点・インデックス・UV値の設定
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		//法線・当たり判定の再計算
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		//メッシュの設定
		meshFilter.sharedMesh = mesh;
		meshFilter.sharedMesh.name = "SimpleMesh";
		
		adjustBottom = true;
		adjustTop    = false;
	}
	public void AdjustMesh(Vector3 topPos, Vector3 currentPos)
	{
		topDir 	= Vector3.Normalize(topPos - currentPos);
		topDir  = Vector3.Normalize(topDir + bottomDirPure);
		
		float tmp;
		tmp = topDir.z;
		topDir.z = topDir.x;
		topDir.x = -tmp;
		topDir.y = 0.0f;
		
		//adjustBottom = true;
		adjustTop    = true;
	}
}
