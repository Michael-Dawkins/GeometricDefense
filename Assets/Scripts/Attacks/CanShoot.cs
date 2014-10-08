﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public abstract class CanShoot : MonoBehaviour {

	public float Damage;
	public float cellRange;
	public TowerTypeManager.TowerType towerType;
	public DamageTypeManager.DamageType damageType;

	protected Animator towerSpriteCenterAnimator;
	protected Animator towerSpriteGlowAnimator;
	protected virtual float ColliderRadius {
		get {
			return map.cellSize * cellRange + map.cellSize / 2f;
		}
	}
	protected Map map;
	protected GameObject towerSpriteCenter;
	protected GameObject towerSpriteGlow;

	protected List<CanTakeDamage> targets = new List<CanTakeDamage>();
	//dead targets is used to postpone dead targets deletion until LateUpdate
	protected List<CanTakeDamage> deadTargets = new List<CanTakeDamage>();
	protected Color bulletColor;

	// Use this for initialization
	protected virtual void Start () {
		towerSpriteCenter = transform.Find("TowerSpriteCenter").gameObject;
		towerSpriteGlow = transform.Find("TowerSpriteGlow").gameObject;
		map = Singletons.map;
		Damage = Singletons.values.Towers[towerType].Levels[1].Damage;
		cellRange = Singletons.values.Towers[towerType].Levels[1].CellRange;
		towerSpriteCenterAnimator = towerSpriteCenter.GetComponent<Animator> ();
		towerSpriteGlowAnimator = towerSpriteGlow.GetComponent<Animator> ();
		bulletColor = towerSpriteCenter.GetComponent<SpriteRenderer>().color;
		UpdateColliderRadius();
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

	public virtual void UpdateColliderRadius(){
		CircleCollider2D collider = GetComponent<CircleCollider2D>();
		collider.radius = ColliderRadius;
	}
	
	public void removeTargetFromList(CanTakeDamage target){
		deadTargets.Add(target);
	}

	protected virtual void LateUpdate(){
		foreach(CanTakeDamage target in deadTargets){
			targets.Remove(target);
			Debug.Log("Removing target from targets");
		}
		if (deadTargets.Count > 0){
			deadTargets = new List<CanTakeDamage>();
		}
	}

}
