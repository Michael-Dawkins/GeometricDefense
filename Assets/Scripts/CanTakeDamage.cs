using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class CanTakeDamage : MonoBehaviour {
	
	public float InitialHp {
		get {
			return initialHp;
		}
		set {
			initialHp = value;
			currentHp = value;
		}
	}
	public GameObject healthBarSPrite;

	private float initialHp = 100f;
	private float currentHp;
	private GameObject healthBar;
	private List<CanShoot> TargetedBy = new List<CanShoot>();

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		healthBar = Instantiate(healthBarSPrite,new Vector3(pos.x - 0.15f, pos.y + 0.15f, 0f), transform.rotation) as GameObject;
		healthBar.transform.parent = gameObject.transform;
		healthBar.gameObject.transform.localScale = new Vector3(0,1,1);
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
			currentHp -= 20f;
			updateHealthBar();
			if (currentHp <=0){
				foreach (CanShoot tower in TargetedBy){
					tower.removeTargetFromList(gameObject.GetComponent<CanTakeDamage>());
				}
				Destroy(gameObject);
			}
		}
	}

	void updateHealthBar(){
		healthBar.gameObject.transform.localScale = new Vector3(currentHp / initialHp, 1f, 1f);
	}
}