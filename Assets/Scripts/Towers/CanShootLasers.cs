using UnityEngine;
using System.Collections;

public class CanShootLasers : CanShoot {

	public GameObject laserPrefab;
	public float coolDownTime;
	float lastShootingTime;

	protected override void Start() {
		base.Start();
        DpsLabel = "Damage"; //For tower info on select, a triangle has no "DPS" per say, but a damage per laser
		GameObject topColliderObj = new GameObject ("TopCollider");
		GameObject leftColliderObj = new GameObject ("LeftCollider");
		GameObject rightColliderObj = new GameObject ("RightCollider");

		topColliderObj.transform.parent = transform;
		leftColliderObj.transform.parent = transform;
		rightColliderObj.transform.parent = transform;

		topColliderObj.transform.localPosition = new Vector3 (0f, 0.4f, 0f);
		leftColliderObj.transform.localPosition = new Vector3 (-0.4f, -0.4f, 0f);
		rightColliderObj.transform.localPosition = new Vector3 (0.4f, -0.4f, 0f);

		topColliderObj.AddComponent<BoxCollider2D>();
		leftColliderObj.AddComponent<BoxCollider2D>();
		rightColliderObj.AddComponent<BoxCollider2D>();

		topColliderObj.AddComponent<CanShootLasersCollider>().OnCollision(ShootCallback);
		leftColliderObj.AddComponent<CanShootLasersCollider>().OnCollision(ShootCallback);
		rightColliderObj.AddComponent<CanShootLasersCollider>().OnCollision(ShootCallback);
	}

	protected override void LateUpdate() {
		base.LateUpdate();
		coolDownTime = Values.instance.Towers[towerType].Levels[1].CoolDown;
	}

	void ShootCallback(){
		Shoot(null);
	}

	override protected void Shoot(CanTakeDamage target) {
		base.Shoot(target);
        towerSpriteCenterAnimator.SetTrigger("shooting");
        towerSpriteGlowAnimator.SetTrigger("shooting");
        SoundManager.instance.PlaySound(SoundManager.TRIANGLE_SHOOT);

		GameObject topLaserObj = Instantiate(laserPrefab) as GameObject;
		GameObject leftLaserObj = Instantiate(laserPrefab) as GameObject;
		GameObject rightLaserObj = Instantiate(laserPrefab) as GameObject;

		topLaserObj.transform.parent = transform;
		leftLaserObj.transform.parent = transform;
		rightLaserObj.transform.parent = transform;

		topLaserObj.transform.localPosition = new Vector3 (0f, 0.2f, 0f);
		leftLaserObj.transform.localPosition = new Vector3 (-0.2f, -0.2f, 0f);
		rightLaserObj.transform.localPosition = new Vector3 (0.2f, -0.2f, 0f);

		topLaserObj.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.forward);
		leftLaserObj.transform.localRotation = Quaternion.AngleAxis(-135f, Vector3.forward);
		rightLaserObj.transform.localRotation = Quaternion.AngleAxis(-45f, Vector3.forward);

		Laser topLaser = topLaserObj.transform.Find("Laser").GetComponent<Laser>();
		Laser leftLaser = leftLaserObj.transform.Find("Laser").GetComponent<Laser>();
		Laser rightLaser = rightLaserObj.transform.Find("Laser").GetComponent<Laser>();

		Color colorToApply = transform.Find("TowerSpriteGlow").GetComponent<SpriteRenderer>().color;
		topLaser.damage = Damage;
		topLaser.damageType = damageType;
		topLaser.SetColor(colorToApply);
		leftLaser.damage = Damage;
		leftLaser.damageType = damageType;
		leftLaser.SetColor(colorToApply);
		rightLaser.damage = Damage;
		rightLaser.damageType = damageType;
		rightLaser.SetColor(colorToApply);
	}
}