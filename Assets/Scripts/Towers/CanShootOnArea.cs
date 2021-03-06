﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class CanShootOnArea : CanShoot {

	bool isAttacking = false;
	Animator pulsingWaveAnimator;
	SpriteRenderer puslingWaveRenderer;
	Transform squareWaverWrapperTrans;
    public override float DPS {
        get { return base.GetDamage(); }
    }

	override protected float ColliderRadius {
		get {
			return map.cellSize * cellRange * 2f + map.cellSize;
		}
	}

	protected override void Start() {
		base.Start();
		squareWaverWrapperTrans = transform.Find("SquareWaveWrapper");
		Transform pulsingWaveTransform = squareWaverWrapperTrans.Find("SquareWave");
		pulsingWaveAnimator = pulsingWaveTransform.GetComponent<Animator>();
		puslingWaveRenderer = pulsingWaveTransform.GetComponent<SpriteRenderer>();
		pulsingWaveAnimator.enabled = false;
		Color towerColor = transform.Find("TowerSpriteGlow").GetComponent<SpriteRenderer>().color;
		puslingWaveRenderer.color = new Color(towerColor.r, towerColor.g, towerColor.b, 0f);
	}
	
	void Update() {
		if(targets.Count > 0) {
            if (TimeScaleManager.instance.IsPaused) {
                return;
            }
			if (isAttacking == false){
				isAttacking = true;
				StartPuslingWaveAnimation();
                SoundManager.instance.PlaySound(SoundManager.SQUARE_ATTACK_START);
			}

			foreach(CanTakeDamage enemy in targets) {
				Shoot(enemy);
			}
		} else {
			if (isAttacking == true){
				isAttacking = false;
				StopPulsingWaveAnimation();
			}
		}
	}

	public void UpdateWaveSize(){
		squareWaverWrapperTrans.localScale = new Vector3(cellRange, cellRange, 0f);
	}

	void StartPuslingWaveAnimation(){
		pulsingWaveAnimator.enabled = true;
		pulsingWaveAnimator.SetBool("isAttacking", true);

        //Swirl
        towerSpriteCenterAnimator.SetBool("squareRotation", true);
        towerSpriteGlowAnimator.SetBool("squareRotation", true);
	}

	void StopPulsingWaveAnimation(){
		pulsingWaveAnimator.SetBool("isAttacking", false);
		pulsingWaveAnimator.enabled = false;
		puslingWaveRenderer.color = new Color(puslingWaveRenderer.color.r, puslingWaveRenderer.color.g, puslingWaveRenderer.color.b, 0f);

        //Swirl
        towerSpriteCenterAnimator.SetBool("squareRotation", false);
        towerSpriteGlowAnimator.SetBool("squareRotation", false);
	}

	protected override void LateUpdate(){
		base.LateUpdate();
	}

	override protected void Shoot(CanTakeDamage target) {
		base.Shoot(target);
		if(target != null) {
            target.takeDamage(GetDamage(), damageType);
		}
	}

    public override float GetDamage() {
        return base.GetDamage() * Time.deltaTime;
    }

	public override void UpdateColliderRadius(){
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		float radius = ColliderRadius;
		collider.size = new Vector2(radius, radius);
	}
}
