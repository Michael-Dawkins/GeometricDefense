using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Enemy TargetEnemy {get; set;}
	public float speed = 4f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveTowardsEnemy();
	}

	void moveTowardsEnemy(){
		if (TargetEnemy == null){
			Destroy (gameObject);
		}

		transform.position = Vector2.Lerp(
			transform.position,
			TargetEnemy.transform.position, 
			Time.deltaTime * speed);
	}
}
