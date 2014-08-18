using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public class CanShoot : MonoBehaviour {

	public int Damage = 10;
	public Bullet bulletPrefab;
	public float shootingSpeed = 1f;

	private Animator anim;
	private bool shooting = false;
	private float nextShootingTime = 0f;
	private List<Enemy> targets = new List<Enemy>();

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (nextShootingTime < Time.time) {
			if (targets.Count > 0){
				shoot (targets[0]);
			}
		}
	}

	void shoot (Enemy targetEnemy) {
		if (targetEnemy != null) {
			Debug.Log("Shooting");
			anim.SetTrigger ("shooting");
			Bullet bullet = Instantiate (bulletPrefab, transform.position, transform.rotation) as Bullet;
			bullet.ShootingTower = this;
			bullet.TargetEnemy = targetEnemy;
			nextShootingTime = Time.time + (1f /shootingSpeed);
		}
	}

	public void removeEnemyFromTargets(Enemy enemy){
		targets.Remove(enemy);
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<Enemy> ()) {
			targets.Add(other.GetComponent<Enemy> ());
		} else {
			Debug.LogError ("not an ennemy");
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {
		Enemy leavingEnemy = other.gameObject.GetComponent<Enemy> ();
		if (leavingEnemy) {
			targets.Remove(leavingEnemy);
		}
	}
}
