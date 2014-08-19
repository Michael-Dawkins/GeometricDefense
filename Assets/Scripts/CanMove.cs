using UnityEngine;
using System.Collections;

public class CanMove : MonoBehaviour {

	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveRight ();
	}

	void moveRight(){
		transform.position = Vector2.Lerp(
			transform.position,
			transform.position + Vector3.right, 
			Time.deltaTime * speed);
	}
}
