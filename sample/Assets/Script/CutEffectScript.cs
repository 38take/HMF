using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]
public class CutEffectScript : MonoBehaviour {
	
	Mesh mesh;
	MeshFilter meshFilter;
	Mesh meshLeft;
	MeshFilter meshFilterLeft;
	
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0.0f, 0.1f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetData(Vector3 lnear, Vector3 lfar, Vector3 rnear, Vector3 rfar)
	{
		mesh 			= new Mesh();
		meshFilter 		= (MeshFilter)GetComponent("MeshFilter");
		mesh.Clear();
		
		//頂点位置の設定
		vertices = new Vector3[4];
		vertices[0] = lfar;
		vertices[1] = lnear;
		vertices[2] = rfar;
		vertices[3] = rnear;
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
	}
}
