using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider2D))]
public class CanShoot : MonoBehaviour {

	public int Damage = 10;
	public Bullet bullet;
	public float shootingRate = 1f;

	private Animator anim;
	private Enemy currentTarget;
	private bool shooting = false;
	private Bullet currentBullet;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (shooting && currentBullet == null){
			shoot(currentTarget);
		}
	}

	void shoot(Enemy targetEnemy){
		if (targetEnemy != null) {
						anim.SetTrigger ("shooting");
						currentBullet = Instantiate (bullet, transform.position, transform.rotation) as Bullet;
						currentBullet.TargetEnemy = targetEnemy;
				}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		
		if (other.gameObject.GetComponent<Enemy>()){
			shooting = true;
			currentTarget = other.GetComponent<Enemy>();
			Debug.Log ("start shooting");
		} else {
			Debug.LogError("not an ennemy");
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.GetComponent<Enemy>()){
			shooting = false;
			currentTarget = null;
			Debug.Log ("stop shooting");
		}
	}
}
