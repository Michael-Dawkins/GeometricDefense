using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class CanTakeDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.GetComponent<Bullet>()){
			Debug.Log ("Taking damage");
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
