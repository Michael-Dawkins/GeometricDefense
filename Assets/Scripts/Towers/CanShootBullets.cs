using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider2D))]
public class CanShootBullets : CanShoot {

	public GuidedBullet bulletPrefab;
	private float nextShootingTime = 0f;
	public float shootingRate;
	public AudioClip shootingSound;

    public override float DPS {
        get {
            return Damage * shootingRate;
        }
    }

	// Use this for initialization
	protected override void Start() {
		base.Start();
		shootingRate = Values.instance.Towers[towerType].Levels[1].ShootingRate;
	}
	
	// Update is called once per frame
	void Update() {
		if(nextShootingTime < Time.time) {
			if(targets.Count > 0) {
				Shoot(targets [0]);
			}
		}
	}

	protected override void LateUpdate(){
		base.LateUpdate();
	}
	
	override protected void Shoot(CanTakeDamage target) {
		base.Shoot(target);
		if(target != null) {
			towerSpriteCenterAnimator.SetTrigger("shooting");
			towerSpriteGlowAnimator.SetTrigger("shooting");
			GuidedBullet guidedBullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GuidedBullet;
			guidedBullet.Target = target;
			guidedBullet.damage = Damage;
			guidedBullet.damageType = damageType;
			guidedBullet.GetComponent<SpriteRenderer>().color = bulletColor;
			
			nextShootingTime = Time.time + (1f / shootingRate);
			audio.PlayOneShot(shootingSound, 0.4f);
		}
	}
}
