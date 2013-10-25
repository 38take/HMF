using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	float oldMouseX;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0.0f,2.0f,0.0f);
		oldMouseX = Input.mousePosition.x;
		rigidbody.AddForce(new Vector3(0.0f, 0.0f, 5.0f));
	}
	
	// Update is called once per frame
	void Update () {
		float transX = (Input.mousePosition.x - oldMouseX) * 5.0f;
		
		//transform.position = new Vector3(	transform.position.x + transX,
		//									transform.position.y,
		//									transform.position.z + 0.1f);
		rigidbody.AddForce(new Vector3(transX, 0.0f, 5.0f));
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
		if(collision.gameObject.name != "Floor")
			Destroy(collision.gameObject);
	}
}
