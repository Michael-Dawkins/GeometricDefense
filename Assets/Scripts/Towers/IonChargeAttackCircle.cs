using UnityEngine;
using System.Collections;

public class IonChargeAttackCircle : IonChargeAttack {

	CanTakeDamage targetedEnemy;
	GameObject bulletObj;

	protected override void Start() {
		base.Start();
		DamageMultiplier = 2f;
		AcquireTarget();
		SetUpGuidedBullet();
        SoundManager.instance.PlaySound(SoundManager.CIRCLE_ION_CHARGE);
	}

	void SetUpGuidedBullet() {
		bulletObj = Instantiate(Resources.Load("IonChargeAttackCircleBullet")) as GameObject;
		bulletObj.transform.localPosition = transform.localPosition;
		GuidedBullet guidedBullet = bulletObj.GetComponent<GuidedBullet>();
		guidedBullet.Target = targetedEnemy;
		guidedBullet.damage = BaseDamage * DamageMultiplier;
		guidedBullet.speed = 3f;
		guidedBullet.GetComponent<SpriteRenderer>().color = canShoot.bulletColor;
	}

	void AcquireTarget() {
		CanTakeDamage[] enemies = Transform.FindObjectsOfType(typeof (CanTakeDamage)) as CanTakeDamage[];
		CanTakeDamage closestEnemy = null;
		foreach(CanTakeDamage enemy in enemies){
			if (closestEnemy == null){
				closestEnemy = enemy;
			} else {
				if (DistanceToEnemy(enemy) < DistanceToEnemy(closestEnemy)){
					closestEnemy = enemy;
				}
			}
		}
		targetedEnemy = closestEnemy;
	}

	float DistanceToEnemy(CanTakeDamage enemy){
		return Vector3.Distance(enemy.transform.position, transform.position);
	}
	
	// Update is called once per frame
	void Update () {

		//TODO move towards target
		//TODO acquire new target is enemy dies
	}
}
