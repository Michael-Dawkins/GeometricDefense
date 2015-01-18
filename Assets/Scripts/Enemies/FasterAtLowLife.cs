using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanMove))]
[RequireComponent(typeof(CanTakeDamage))]
public class FasterAtLowLife : MonoBehaviour {

	float healthThreshold = 0.25f;//percentage expressed 0 to 1
	CanMove canMoove;
	CanTakeDamage canTakeDamage;

	void Start () {
		canMoove = GetComponent<CanMove>();
		canTakeDamage = GetComponent<CanTakeDamage>();
		canTakeDamage.healthChanged += GoFasterIfHealthIsLow;
	}

	void GoFasterIfHealthIsLow(){
		float lifePercentage = canTakeDamage.currentHp / canTakeDamage.InitialHp;
		canMoove.speedUp = (lifePercentage < healthThreshold);
	}
}