using UnityEngine;
using System.Collections;

public class Laser : Projectile {

	float shootingTime;
	float timeBeforeDeletion = 1f;

	void Start () {
		shootingTime = Time.time;
	}
	
	void Update () {
		if ((shootingTime + timeBeforeDeletion) < Time.time){
			Destroy(this);
		} else {
			transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, transform.localPosition.y, 0);
		}
	}

	public override void OnEnemyHit(){}
}
