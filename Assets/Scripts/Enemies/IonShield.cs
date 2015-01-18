using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanTakeDamage))]
public class IonShield : MonoBehaviour {

	public GameObject shieldSprite;
	float lifeToShieldRatio = 0.25f;
	float timeToRechargeShield = 3f;
	float initialShieldPower;
	float currentShieldPower{
		get{return _currentShieldPower;}
		set{
			if(!shieldDestroyed){
				_currentShieldPower = value;
				UpdateShieldRadius();
				if (_currentShieldPower <= 0f){
					shieldDestroyed = true;
				}
			}
		}
	}
	float _currentShieldPower;
	bool shieldDestroyed = false;
	CanTakeDamage canTakeDamage;
	void Start () {
		canTakeDamage = GetComponent<CanTakeDamage>();
		initialShieldPower = lifeToShieldRatio * canTakeDamage.InitialHp;
		currentShieldPower = initialShieldPower;
	}

	void Update(){
		if (!shieldDestroyed){
			RechargeShield();
		}
	}

	//Return the potential remainaing damage
	public float TakeDamage(float damage){
		if (shieldDestroyed){
			return damage;
		}
		float remainingDamage = 0f;
		if (damage > currentShieldPower){
			remainingDamage = damage - currentShieldPower;
		}
		currentShieldPower -= damage;
		return remainingDamage;
	}

	void RechargeShield(){
		if (currentShieldPower != initialShieldPower){
			float newShieldPower = currentShieldPower + (Time.deltaTime * initialShieldPower) / timeToRechargeShield;
			currentShieldPower = Mathf.Clamp(newShieldPower, 0f, initialShieldPower);
			UpdateShieldRadius();
		}
	}

	void UpdateShieldRadius(){
		float scaleRatio = Mathf.Clamp(currentShieldPower / initialShieldPower, 0f, 1f);
		shieldSprite.transform.localScale = Vector3.one * scaleRatio;
	}

	void ApplyScaleToShieldSprite(float scale){

	}

}
