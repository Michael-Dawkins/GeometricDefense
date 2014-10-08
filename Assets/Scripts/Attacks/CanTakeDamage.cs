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
	
	float slowTime = 0.2f;
	float slowStartTime;
	float initialHp = 100f;
	float currentHp;
	GameObject healthBar;
	List<CanShoot> TargetedBy = new List<CanShoot>();
	PlayerMoney playerMoney;
	CanMove canMove;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		healthBar = Instantiate(healthBarSPrite,new Vector3(pos.x - 0.15f, pos.y + 0.15f, 0f), transform.rotation) as GameObject;
		healthBar.transform.parent = gameObject.transform;
		healthBar.gameObject.transform.localScale = new Vector3(0,1,1);

		if (playerMoney == null){
			GameObject playerState = GameObject.Find("PlayerState");
			if (playerState == null){
				throw new UnityException("PlayerState cannot be found in scene");
			}
			playerMoney = playerState.GetComponent<PlayerMoney>();
		}
		canMove = GetComponent<CanMove>();
	}
	
	// Update is called once per frame
	void Update () {
		canMove.slowed = Time.time < (slowStartTime + slowTime);
	}

	public void addTargetingTower(CanShoot tower){
		TargetedBy.Add(tower);
	}

	public void removeTargetingTower(CanShoot tower){
		TargetedBy.Remove(tower);
	}

	void OnTriggerEnter2D(Collider2D other){
		Projectile projectile = other.gameObject.GetComponent<Projectile>();
		if(projectile){
			takeDamage(projectile.damage, projectile.damageType);
			projectile.OnEnemyHit();
		}
	}

	public void takeDamage(float damage, DamageTypeManager.DamageType damageType){
		if (damageType == DamageTypeManager.DamageType.Antimatter){
			slowStartTime = Time.time;
		}
		currentHp -= damage;
		updateHealthBar();
		if (currentHp <=0){
			Die();
		}
	}

	void updateHealthBar(){
		healthBar.gameObject.transform.localScale = new Vector3((currentHp / initialHp) * 0.4f, 0.3f, 1f);
	}

	void Die(){
		foreach (CanShoot tower in TargetedBy){
			tower.removeTargetFromList(gameObject.GetComponent<CanTakeDamage>());
		}
		Destroy(gameObject);
		playerMoney.Money += 6 + (int) initialHp / 16;
	}
}