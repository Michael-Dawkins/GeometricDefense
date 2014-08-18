using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class CanTakeDamage : MonoBehaviour {

	public float hp = 100f;

	private List<CanShoot> TargetedBy = new List<CanShoot>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addTargetingTower(CanShoot tower){
		TargetedBy.Add(tower);
	}

	public void removeTargetingTower(CanShoot tower){
		TargetedBy.Remove(tower);
	}

	void OnTriggerEnter2D(Collider2D other){
		Bullet bullet = other.gameObject.GetComponent<Bullet>();
		if(bullet){
			Destroy(other.gameObject);
			hp -= 20f;
			if (hp <=0){
				foreach (CanShoot tower in TargetedBy){
					tower.removeEnemyFromTargets(gameObject.GetComponent<Enemy>());
				}
				Destroy(gameObject);
			}
		}
	}
}