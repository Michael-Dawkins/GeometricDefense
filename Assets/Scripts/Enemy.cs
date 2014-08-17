using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour {

	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveRight();
	}

	void moveRight(){
		transform.position = Vector2.Lerp(
			transform.position,
			transform.position + Vector3.right, 
			Time.deltaTime * speed);
	}
}
