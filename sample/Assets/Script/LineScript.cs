using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]

public class LineScript : MonoBehaviour {
	
	Mesh mesh;
	MeshFilter meshFilter;
	
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0.0f, 0.1f, 0.0f);
	}
	
	public void SetData(Vector3 prev, Vector3 point1, Vector3 point2, Vector3 next) {
		
		mesh = new Mesh();
		meshFilter = (MeshFilter)GetComponent("MeshFilter");
		mesh.Clear();
		//頂点位置の算出
		bool[] vecValid = new bool[]{false, false, false};
		Vector3[] vecValue = new Vector3[]{	new Vector3(0.0f, 0.0f, 0.0f),
											new Vector3(0.0f, 0.0f, 0.0f),
											new Vector3(0.0f, 0.0f, 0.0f)};
		//3ベクトル分算出
		if(prev.x != float.MaxValue){
			vecValid[0] = true;
			vecValue[0] = point1 - prev;
		}
		vecValid[1] = true;
		vecValue[1] = point2 - point1;
		if(next.x != float.MaxValue){
			vecValid[2] = true;
			vecValue[2] = next - point2;
		}
		//線分の端での方向ベクトルを算出
		if(vecValid[0])	vecValue[0] = vecValue[0] + vecValue[1];
		else 			vecValue[0] = vecValue[1];
		if(vecValid[2])	vecValue[1] += vecValue[2];
		vecValue[0] = Vector3.Normalize(vecValue[0]);
		vecValue[1] = Vector3.Normalize(vecValue[1]);
		float tmp;
		for(int i=0; i<2; i++)
		{
			tmp = vecValue[i].z;
			vecValue[i].z = vecValue[i].x;
			vecValue[i].x = -tmp;
		}
		
		//頂点位置の設定
		vertices = new Vector3[4];
		vertices[0] = point1 + (vecValue[0]* 1.0f);
		vertices[1] = point2 + (vecValue[1]* 1.0f);
		vertices[2] = point1 - (vecValue[0]* 1.0f);
		vertices[3] = point2 - (vecValue[1]* 1.0f);
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
	
	// Update is called once per frame
	void Update () {
		
		//mesh.Clear();
		
		//vertices = new Vector3[4];
		//vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
		//vertices[1] = new Vector3( 0, 0.0f, 1 );
		//vertices[2] = new Vector3( 1, 0.0f, 0 );
		//vertices[3] = new Vector3( 1, 0.0f, 1 );
		//
		//triangles = new int[6];
		//triangles[0] = 0;
		//triangles[1] = 1;
		//triangles[2] = 2;
		//triangles[3] = 2;
		//triangles[4] = 1;
		//triangles[5] = 3;
		//
		//uvs = new Vector2[4];
		//uvs[0] = Vector2.zero;
		//uvs[1] = Vector2.up;
		//uvs[2] = Vector2.right;
		//uvs[3] = new Vector2(1.0f, 1.0f);
		//
		//mesh.vertices = vertices;
		//mesh.triangles = triangles;
		//mesh.uv = uvs;
		//
		//mesh.RecalculateNormals();
		//mesh.RecalculateBounds();
		////mesh.Optimize();
		//
		//meshFilter.sharedMesh = mesh;
		//meshFilter.sharedMesh.name = "SimpleMesh";
	}
	
}
