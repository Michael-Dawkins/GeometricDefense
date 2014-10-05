using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public class CanShootBullets : CanShoot {

	public TargetedBullet bulletPrefab;
	private float nextShootingTime = 0f;
	public float shootingSpeed;
	public AudioClip shootingSound;

	// Use this for initialization
	protected override void Start() {
		base.Start();
	}
	
	// Update is called once per frame
	void Update() {
		if(nextShootingTime < Time.time) {
			if(targets.Count > 0) {
				shoot(targets [0]);
			}
		}
	}

	protected override void LateUpdate(){
		base.LateUpdate();
	}
	
	void shoot(CanTakeDamage target) {
		if(target != null) {
			towerSpriteCenterAnimator.SetTrigger("shooting");
			towerSpriteGlowAnimator.SetTrigger("shooting");
			TargetedBullet targetedBullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as TargetedBullet;
			targetedBullet.Target = target;
			targetedBullet.damage = Damage;
			targetedBullet.GetComponent<SpriteRenderer>().color = bulletColor;
			
			nextShootingTime = Time.time + (1f / shootingSpeed);
			audio.PlayOneShot(shootingSound, 0.5f);
		}
	}
}
