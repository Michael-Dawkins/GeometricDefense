using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class CanShootOnArea : CanShoot {

	protected virtual float ColliderRadius {
		get {
			return map.cellSize * cellRange * 2f + map.cellSize;
		}
	}

	protected override void Start() {
		base.Start();
	}
	
	void Update() {
		if(targets.Count > 0) {
			foreach(CanTakeDamage enemy in targets) {
				shoot(enemy);
			}
		}
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
