using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public CanTakeDamage Target {get; set;}
	public float speed = 4f;

	public CanShoot ShootingTower {get; set;}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveTowardsTarget();
	}

	void moveTowardsTarget(){
		if (Target == null){
			Destroy (gameObject);
		} else {
			transform.position = 
				Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * speed);
		}
	}
}
