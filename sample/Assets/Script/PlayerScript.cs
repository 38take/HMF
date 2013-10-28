using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	ScoreScript				SScore;
	private float			oldMouseX;
	private float			Speed = 7.5f;
	private bool			InitFall = false;
	private	Vector3			oldPos;
	
	// Use this for initialization
	void Start () {
		SScore = (ScoreScript)GameObject.Find("ScoreTextBox").GetComponent("ScoreScript");
			
		transform.position = new Vector3(0.0f,10.0f,0.0f);
		oldMouseX = Input.mousePosition.x;
	}
	
	// Update is called once per frame
	void Update () {
		float transX = (Input.mousePosition.x - oldMouseX) * 5.0f;
		
		//transform.position = new Vector3(	transform.position.x + transX,
		//									transform.position.y,
		//									transform.position.z + 0.1f);
		
//		rigidbody.AddForce(new Vector3(transX, 0.0f, Speed));
		
		if( InitFall )
			rigidbody.velocity = new Vector3(transX, rigidbody.velocity.y, Speed);

		// 落下制限
		if( Physics.Raycast(transform.position,Vector3.down,5) )
		{
			oldPos = transform.position;
			
			if( !InitFall )
				InitFall = true;
		}
		else
		{
			if( InitFall )
			{
				transform.position = oldPos;
			}
		}
		
		if(transform.position.z >= 15.0f)
		{
			transform.position = new Vector3(	transform.position.x,
												transform.position.y,
												-15.0f);
		}
		
		oldMouseX = Input.mousePosition.x;
	
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "Target(Clone)")
		{
			SScore.AddScore(100);
			Destroy(collision.gameObject);
		}
	}
}
