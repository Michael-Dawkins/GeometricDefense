using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public class CanShoot : MonoBehaviour {

	public int Damage = 10;
	public Bullet bulletPrefab;
	public float shootingSpeed = 1f;
	public AudioClip shootingSound;

	private Animator anim;
	private float nextShootingTime = 0f;
	private List<CanTakeDamage> targets = new List<CanTakeDamage>();

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

	void shoot (CanTakeDamage target) {
		if (target != null) {
			anim.SetTrigger ("shooting");
			Bullet bullet = Instantiate (bulletPrefab, transform.position, transform.rotation) as Bullet;
			bullet.Target = target;
			nextShootingTime = Time.time + (1f /shootingSpeed);
			audio.PlayOneShot(shootingSound, 1F);
		}
	}

	public void removeTargetFromList(CanTakeDamage target){
		targets.Remove(target);
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<CanTakeDamage> ()) {
			targets.Add(other.GetComponent<CanTakeDamage> ());
			other.gameObject.GetComponent<CanTakeDamage>().addTargetingTower(this);
		} else {
			Debug.LogError ("not an ennemy");
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {
		CanTakeDamage leavingEnemy = other.gameObject.GetComponent<CanTakeDamage> ();
		if (leavingEnemy) {
			targets.Remove(leavingEnemy);
			leavingEnemy.gameObject.GetComponent<CanTakeDamage>().removeTargetingTower(this);
		}
	}
}
