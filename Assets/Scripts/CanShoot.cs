using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public class CanShoot : MonoBehaviour {

	public float Damage;
	public Bullet bulletPrefab;
	public float shootingSpeed;
	public AudioClip shootingSound;
	public float cellRange;

	private Animator anim;
	private float ColliderRadius {
		get {
			return map.cellSize * cellRange + map.cellSize / 2f;
		}
	}
	private float nextShootingTime = 0f;
	private List<CanTakeDamage> targets = new List<CanTakeDamage>();
	private Color bulletColor;
	private Map map;

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
		anim = GetComponent<Animator> ();
		bulletColor = gameObject.GetComponent<SpriteRenderer>().color;
		CircleCollider2D collider = GetComponent<CircleCollider2D>();
		collider.radius = ColliderRadius;
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
			bullet.Damage = Damage;
			bullet.GetComponent<SpriteRenderer>().color = bulletColor;

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
