using UnityEngine;
using System.Collections;

public class GuidedBullet : Projectile {

	public CanTakeDamage Target {get; set;}

	void Update () {
		moveTowardsTarget();
	}
	
	void moveTowardsTarget(){
		if (Target == null){
			Destroy (gameObject);
		} else {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * speed);
		}
	}

	public override void OnEnemyHit() {
		Destroy(gameObject);
	}
}
