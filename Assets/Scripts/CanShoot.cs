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

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (shooting){
			shoot(currentTarget);
			shooting = false;
		}
	}

	void shoot(Enemy targetEnemy){
		anim.SetTrigger("shooting");
		Bullet currentBullet = Instantiate(bullet, transform.position, transform.rotation) as Bullet;
		currentBullet.TargetEnemy = targetEnemy;
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
