using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public abstract class CanShoot : MonoBehaviour {

	public float Damage;
	//ex: "Plasma" > 40 for à 40% boost from a plasma tower
	public Dictionary<string,float> damageMultipliers = new Dictionary<string, float>();
	public float cellRange{
        get {return _cellRange;}
        set {
            _cellRange = value;
            UpdateColliderRadius();
        }
    }
    private float _cellRange;
	public TowerTypeManager.TowerType towerType;
	public DamageTypeManager.DamageType damageType;
	public Color towerColor;

    public string DpsLabel = "DPS";
    public virtual float DPS {
        get { return GetDamage(); }
    }

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
	public Color bulletColor;

	IonCharger ionCharger;

	// Use this for initialization
	protected virtual void Start () {
		towerSpriteCenter = transform.Find("TowerSpriteCenter").gameObject;
		towerSpriteGlow = transform.Find("TowerSpriteGlow").gameObject;
		map = Map.instance;
		Damage = Values.instance.Towers[towerType].Levels[1].Damage;
		cellRange = Values.instance.Towers[towerType].Levels[1].CellRange;
		towerSpriteCenterAnimator = towerSpriteCenter.GetComponent<Animator> ();
		towerSpriteGlowAnimator = towerSpriteGlow.GetComponent<Animator> ();
		bulletColor = towerSpriteCenter.GetComponent<SpriteRenderer>().color;
        SetUpDamageTypeDamageMultiplier();
		UpdateColliderRadius();
		if (damageType == DamageTypeManager.DamageType.IonCharge){
			ionCharger = gameObject.AddComponent<IonCharger>();
			ionCharger.canShoot = this;
		}
	}

    void SetUpDamageTypeDamageMultiplier() {
        string damageTypeKey = "Damage type "
            + DamageTypeManager.instance.GetDamageTypeLabel(
                DamageTypeManager.instance.currentDamageType);
        switch(damageType){
            case DamageTypeManager.DamageType.Antimatter:
                damageMultipliers.Add(damageTypeKey, -25f);
                break;
            case DamageTypeManager.DamageType.IonCharge:
                damageMultipliers.Add(damageTypeKey, 0f);
                break;
            case DamageTypeManager.DamageType.Plasma:
                damageMultipliers.Add(damageTypeKey, 10f);
                break;
        }
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
		}
		if (deadTargets.Count > 0){
			deadTargets = new List<CanTakeDamage>();
		}
	}

	protected virtual void Shoot(CanTakeDamage target){
		if (ionCharger != null){
			ionCharger.Charge(GetDamage());
		}
	}

	public virtual float GetDamage(){
		float calculatedDamage = Damage;
		float totalPercentageToApply = 0f;
		foreach(KeyValuePair<string, float> damagePercentage in damageMultipliers){
			totalPercentageToApply += damagePercentage.Value;
		}
		calculatedDamage += (totalPercentageToApply / 100f) * calculatedDamage;
		return calculatedDamage;
	}

}
