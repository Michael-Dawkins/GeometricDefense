using UnityEngine;
using System.Collections;

public class IonChargeAttackTriangle : IonChargeAttack {

	protected override void Start() {
		base.Start();
		DamageMultiplier = 1.2f;
		Shoot();
        SoundManager.instance.PlaySound(SoundManager.TRIANGLE_ION_CHARGE);
	}

	void Shoot() {
		GameObject topLaserObj = Instantiate(Resources.Load("IonChargeAttackTriangle")) as GameObject;
		GameObject leftLaserObj = Instantiate(Resources.Load("IonChargeAttackTriangle")) as GameObject;
		GameObject rightLaserObj = Instantiate(Resources.Load("IonChargeAttackTriangle")) as GameObject;
		
		topLaserObj.transform.parent = canShoot.transform;
		leftLaserObj.transform.parent = canShoot.transform;
		rightLaserObj.transform.parent = canShoot.transform;
		
		topLaserObj.transform.localPosition = new Vector3 (0f, 0.2f, 0f);
		leftLaserObj.transform.localPosition = new Vector3 (-0.2f, -0.2f, 0f);
		rightLaserObj.transform.localPosition = new Vector3 (0.2f, -0.2f, 0f);
		
		topLaserObj.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.forward);
		leftLaserObj.transform.localRotation = Quaternion.AngleAxis(-135f, Vector3.forward);
		rightLaserObj.transform.localRotation = Quaternion.AngleAxis(-45f, Vector3.forward);
		
		Laser topLaser = topLaserObj.transform.Find("Laser").GetComponent<Laser>();
		Laser leftLaser = leftLaserObj.transform.Find("Laser").GetComponent<Laser>();
		Laser rightLaser = rightLaserObj.transform.Find("Laser").GetComponent<Laser>();
		
		Color colorToApply = canShoot.transform.Find("TowerSpriteGlow").GetComponent<SpriteRenderer>().color;
		topLaser.damage = BaseDamage * DamageMultiplier;
		topLaser.speed = 2.5f;
		topLaser.SetColor(colorToApply);
		leftLaser.damage = BaseDamage * DamageMultiplier;
		leftLaser.SetColor(colorToApply);
		leftLaser.speed = 2.5f;
		rightLaser.damage = BaseDamage * DamageMultiplier;
		rightLaser.SetColor(colorToApply);
		rightLaser.speed = 2.5f;
	}
}
