using UnityEngine;

public class Projectile : MonoBehaviour {

	public float damage;
	public DamageTypeManager.DamageType damageType;
	public float speed = 4f;

	public virtual void OnEnemyHit(){}
}
