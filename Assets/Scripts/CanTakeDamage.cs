using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class CanTakeDamage : MonoBehaviour {

	public float hp = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		Bullet bullet = other.gameObject.GetComponent<Bullet>();
		if(bullet){
			Destroy(other.gameObject);
			hp -= 20f;
			if (hp <=0){
				bullet.ShootingTower.removeEnemyFromTargets(gameObject.GetComponent<Enemy>());
				Destroy(gameObject);
			}
		}
	}
}