using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]

public class CutEdgeManagerScript : MonoBehaviour {
	
	public struct EDGEPOINT
	{
		public int[] vertexID;
		public bool opened;
		public float width;
		public Vector3 dir;
		public Vector3 basePos;
	};
	//メッシュデータ
	Mesh mesh;
	MeshFilter meshFilter;
	Mesh meshLeft;
	MeshFilter meshFilterLeft;
	//メッシュを構成するデータ
	ArrayList vertices;		//Vector3
	ArrayList triangles;	//int
	ArrayList uvs;			//Vector2
//	Vector3[]   vertices;
//	int[]		triangles;
//	Vector2[]	uvs;
//	Vector3[]   verticesBak;
//	int[]		trianglesBak;
//	Vector2[]	uvsBak;
	
	float     uv;
	//制御点
	ArrayList edgePointArray;
	bool initiarized = false;
	int  openedPointNum = 0;
	//外部
	LineManagerScript SLineManager = null;
	
	public float MaxWidth;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
	}
	void Init()
	{
		//パラメータの初期化
		openedPointNum = 0;
		//メッシュ構成データの準備
		vertices 	= new ArrayList();
		triangles 	= new ArrayList();
		uvs 		= new ArrayList();
		uv = 0.0f;
		//メッシュの準備
		mesh 			= new Mesh();
		meshFilter 		= (MeshFilter)GetComponent("MeshFilter");
		mesh.Clear();
		//制御点の準備
		edgePointArray = new ArrayList();
		PlayerScript SPlayer = ((GameObject)GameObject.Find("ナイフ5")).GetComponent<PlayerScript>();
		Vector3 pos = SLineManager.CalcPos( (int)SPlayer.m_Timer, SPlayer.m_Offset);
		AddPoint(pos);
		initiarized = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!initiarized)
		{
			if(SLineManager == null)
				SLineManager = ((GameObject)GameObject.Find("LineManager")).GetComponent<LineManagerScript>();
			if(SLineManager.isExist())
				Init();
		}
		else
			UpdateMesh();
	}
	private void UpdateMesh()
	{
		Vector3 tmpVertex;
		EDGEPOINT tmpPoint;
		if(mesh != null)
		{	
			for(int i=openedPointNum; i<edgePointArray.Count-1; i++)
			{
				tmpPoint = ((EDGEPOINT)edgePointArray[i]);
				
				if(((EDGEPOINT)edgePointArray[i]).width < (MaxWidth - 0.0001f))
				{
					tmpPoint.width += (MaxWidth - tmpPoint.width)*0.1f;
					(edgePointArray[i]) = tmpPoint;
					//頂点を広げる
					tmpVertex = ((Vector3)vertices[tmpPoint.vertexID[0]]);
					tmpVertex = tmpPoint.basePos - tmpPoint.dir*tmpPoint.width;
					(vertices[tmpPoint.vertexID[0]]) = tmpVertex;
					tmpVertex = ((Vector3)vertices[tmpPoint.vertexID[1]]);
					tmpVertex = tmpPoint.basePos + tmpPoint.dir*tmpPoint.width;
					(vertices[tmpPoint.vertexID[1]]) = tmpVertex;
				}
				else
				{
					tmpPoint.opened = true;
					(edgePointArray[i]) = tmpPoint;
					openedPointNum++;
				}
			}
			
			//頂点・インデックス・UV値の設定
			Vector3[] 	vtx = (Vector3[])vertices.ToArray( typeof( Vector3 ));
			int[]		idx = (int[])triangles.ToArray( typeof( int ));
			Vector2[]	UV  = (Vector2[])uvs.ToArray( typeof( Vector2 ));
			mesh.vertices = vtx;
			mesh.triangles = idx;
			mesh.uv = UV;
			//法線・当たり判定の再計算
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();
			//メッシュの設定
			meshFilter.sharedMesh = mesh;
			meshFilter.sharedMesh.name = "SimpleMesh";
		}
	}
	//制御点の追加
	public void AddPoint(Vector3 point)
	{
		Vector3 prevPoint;
		if(edgePointArray != null)
		{
			int pointNo = edgePointArray.Count;
			
			//新しい点を作成
			EDGEPOINT tmp = new EDGEPOINT();
			
			tmp.vertexID = new int[2];
			tmp.vertexID[0] = (pointNo*2);
			tmp.vertexID[1] = (pointNo*2)+1;
			tmp.width = 0.0f;
			tmp.opened = false;
			tmp.basePos = point;
			if(edgePointArray.Count > 0)
			{
				prevPoint = ((EDGEPOINT)edgePointArray[pointNo-1]).basePos;
				tmp.dir = Vector3.Normalize(tmp.basePos - prevPoint);
				float x = tmp.dir.x;
				tmp.dir.x = tmp.dir.z;
				tmp.dir.z = -x;
				tmp.dir.y = 0.0f;
			}
			else
				tmp.dir = SLineManager.GetLineDirection(SLineManager.GetPlyaerLineID());
			
			edgePointArray.Add(tmp);
			//ひとつ前の点の方向を再計算
			RecalcDirection(pointNo-1);
			
			//メッシュへの頂点の追加
			AddMeshData(point, (pointNo*2), uv);
			if(uv == 0.0f) 	uv = 1.0f;
			else			uv = 0.0f;
		}
	}
	private void AddMeshData(Vector3 v, int idx, float texUV)
	{
		//頂点を2点追加
		vertices.Add(v);
		vertices.Add(v);
		//UV追加
		uvs.Add(new Vector2(0.0f, texUV));
		uvs.Add(new Vector2(1.0f, texUV));
		//インデックスを追加
		if(idx > 0)
		{
			triangles.Add(idx-2);
			triangles.Add(idx);
			triangles.Add(idx-1);
			
			triangles.Add(idx);
			triangles.Add(idx+1);
			triangles.Add(idx-1);
		}
	}
	
	//制御点の方向（進行方向に対して右向き）を計算
	private void RecalcDirection(int id)
	{
		EDGEPOINT tmpPoint;
		if( id > 0 &&
			id != edgePointArray.Count-1)
		{
			tmpPoint = ((EDGEPOINT)edgePointArray[id]);
			Vector3 dir1 = ((EDGEPOINT)edgePointArray[id]).basePos - ((EDGEPOINT)edgePointArray[id-1]).basePos;
			Vector3 dir2 = ((EDGEPOINT)edgePointArray[id+1]).basePos - ((EDGEPOINT)edgePointArray[id]).basePos;
			dir1 = Vector3.Normalize(dir1);
			dir2 = Vector3.Normalize(dir2);
			tmpPoint.dir = Vector3.Normalize(dir1 + dir2);
			float tmp = tmpPoint.dir.z;
			tmpPoint.dir.z = -tmpPoint.dir.x;
			tmpPoint.dir.x = tmp;
			tmpPoint.dir.y = 0.0f;
			
			(edgePointArray[id]) = tmpPoint;
		}
	}
	
	//更新をストップさせる
	public void StopEdgeOpen()
	{
		EDGEPOINT tmpPoint;
		for(int i=openedPointNum; i<edgePointArray.Count; i++)
		{
			tmpPoint = ((EDGEPOINT)edgePointArray[i]);
			tmpPoint.opened = true;
			(edgePointArray[i]) = tmpPoint;
		}
		openedPointNum = edgePointArray.Count;
	}
	
//	public void SetMesh(Vector3 currentPos, Vector3 prevPos, Vector3 bottomPos)
//	{
//		mesh 			= new Mesh();
//		meshFilter 		= (MeshFilter)GetComponent("MeshFilter");
//		mesh.Clear();
//		//各ポイントでの方向を算出
//		Vector3 bottomdir 	= Vector3.Normalize(prevPos - bottomPos);
//		Vector3 prevDir 	= Vector3.Normalize(currentPos - prevPos);		
//		bottomDir = Vector3.Normalize(bottomdir + prevDir);
//		bottomDirPure = bottomDir;
//		
//		float tmp;
//		tmp = bottomDir.z;
//		bottomDir.z = bottomDir.x;
//		bottomDir.x = -tmp;
//		bottomDir.y = 0.0f;
//		
//		//頂点位置の設定
//		vertices = new Vector3[4];
//		vertices[0] = prevPos;
//		vertices[1] = currentPos;
//		vertices[2] = prevPos;
//		vertices[3] = currentPos;
//		//インデックス
//		triangles = new int[6];
//		triangles[0] = 0;
//		triangles[1] = 1;
//		triangles[2] = 2;
//		triangles[3] = 2;
//		triangles[4] = 1;
//		triangles[5] = 3;
//		//UV値
//		uvs = new Vector2[4];
//		uvs[0] = Vector2.zero;
//		uvs[1] = Vector2.up;
//		uvs[2] = Vector2.right;
//		uvs[3] = new Vector2(1.0f, 1.0f);
//		//頂点・インデックス・UV値の設定
//		mesh.vertices = vertices;
//		mesh.triangles = triangles;
//		mesh.uv = uvs;
//		//法線・当たり判定の再計算
//		mesh.RecalculateNormals();
//		mesh.RecalculateBounds();
//		mesh.Optimize();
//		//メッシュの設定
//		meshFilter.sharedMesh = mesh;
//		meshFilter.sharedMesh.name = "SimpleMesh";
//		
//		adjustBottom = true;
//		adjustTop    = false;
//	}
//	public void AdjustMesh(Vector3 topPos, Vector3 currentPos)
//	{
//		topDir 	= Vector3.Normalize(topPos - currentPos);
//		topDir  = Vector3.Normalize(topDir + bottomDirPure);
//		
//		float tmp;
//		tmp = topDir.z;
//		topDir.z = topDir.x;
//		topDir.x = -tmp;
//		topDir.y = 0.0f;
//		
//		//adjustBottom = true;
//		adjustTop    = true;
//	}
}
