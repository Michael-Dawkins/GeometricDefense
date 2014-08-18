using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Enemy TargetEnemy {get; set;}
	public float speed = 4f;

	public CanShoot ShootingTower {get; set;}

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
		} else {

			transform.position = 
				Vector3.MoveTowards(transform.position, TargetEnemy.transform.position, Time.deltaTime * speed);
		}
	}
}
