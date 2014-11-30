using UnityEngine;
using System.Collections;

public class IonChargeAttackSquare : IonChargeAttack {

	protected override void Start() {
		base.Start();
		DamageMultiplier = 1.2f;
		AudioClip clip = Instantiate(Resources.Load("laser11")) as AudioClip;
		audio.PlayOneShot(clip, 1f);
	}
	
	void Update () {
	
	}
}
