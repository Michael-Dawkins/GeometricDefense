using UnityEngine;
using System.Collections;

public abstract class IonChargeAttack : MonoBehaviour {

	public CanShoot canShoot;
	public float BaseDamage;//This is set depending on the chargeLevel
	public float DamageMultiplier;//This one is different for each tower type (circle, triangle, square)

	protected virtual void Start () {
		gameObject.AddComponent<AudioSource>();
	}
}
