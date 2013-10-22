using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	float oldMouseX;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0.0f,0.0f,0.0f);
		oldMouseX = Input.mousePosition.x;
	}
	
	// Update is called once per frame
	void Update () {
		float transX = Input.mousePosition.x - oldMouseX;
		
		transform.position = new Vector3(	Input.mousePosition.x,
											transform.position.y,
											transform.position.z + 0.1f);
		
		if(transform.position.z >= 15.0f)
		{
			transform.position = new Vector3(	transform.position.x,
												transform.position.y,
												-15.0f);
		}
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name != "Floor")
			Destroy(collision.gameObject);
	}
}
