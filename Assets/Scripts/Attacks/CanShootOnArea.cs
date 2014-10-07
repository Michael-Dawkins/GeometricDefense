using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class CanShootOnArea : CanShoot {

	bool isAttacking = false;
	Animator puslingWaveAnimator;
	SpriteRenderer puslingWaveRenderer;

	protected virtual float ColliderRadius {
		get {
			return map.cellSize * cellRange * 2f + map.cellSize;
		}
	}

	protected override void Start() {
		base.Start();
		Transform pulsingWaveTransform = transform.Find("SquareWave");
		puslingWaveAnimator = pulsingWaveTransform.GetComponent<Animator>();
		puslingWaveRenderer = pulsingWaveTransform.GetComponent<SpriteRenderer>();
		puslingWaveRenderer.color = new Color(puslingWaveRenderer.color.r, puslingWaveRenderer.color.g, puslingWaveRenderer.color.b, 0f);
	}
	
	void Update() {
		if(targets.Count > 0) {
			if (isAttacking == false){
				isAttacking = true;
				StartPuslingWaveAnimation();
			}
			foreach(CanTakeDamage enemy in targets) {
				shoot(enemy);
			}
		} else {
			if (isAttacking == true){
				isAttacking = false;
				StopPulsingWaveAnimation();
			}
		}
	}

	void StartPuslingWaveAnimation(){
		Debug.Log("Attack !");
		puslingWaveAnimator.SetBool("isAttacking", true);
	}

	void StopPulsingWaveAnimation(){
		Debug.Log("Stop attack !");
		puslingWaveAnimator.SetBool("isAttacking", false);
		puslingWaveRenderer.color = new Color(puslingWaveRenderer.color.r, puslingWaveRenderer.color.g, puslingWaveRenderer.color.b, 0f);
	}

	protected override void LateUpdate(){
		base.LateUpdate();
	}

	void shoot(CanTakeDamage target) {
		if(target != null) {
			target.takeDamage(Damage * Time.deltaTime, damageType);
		}
	}

	public override void UpdateColliderRadius(){
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		float radius = ColliderRadius;
		collider.size = new Vector2(radius, radius);
	}
}
