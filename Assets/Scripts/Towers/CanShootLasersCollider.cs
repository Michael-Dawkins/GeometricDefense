using UnityEngine;
using System.Collections;

public class CanShootLasersCollider : MonoBehaviour {

	public delegate void OnCollisionListener();

	OnCollisionListener callback;

	void Start() {
		BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
		boxCollider2D.size = new Vector2(0.1f, 0.2f);
	}

	public void OnCollision(OnCollisionListener collisionCallback){
		callback = collisionCallback;
	}

	void OnTriggerEnter2D(Collider2D other){
		callback();
	}
}