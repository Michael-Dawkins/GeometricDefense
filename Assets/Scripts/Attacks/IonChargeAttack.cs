using UnityEngine;
using System.Collections;

public abstract class IonChargeAttack : MonoBehaviour {

	public CanShoot canShoot;
	public float BaseDamage;
	public float DamageMultiplier;

	protected virtual void Start () {
		gameObject.AddComponent<AudioSource>();
	}
}
