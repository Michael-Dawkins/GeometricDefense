using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaBoostable : MonoBehaviour {

	public static string DAMAGE_BONUS_STRING= "PlasmaBoost";
	CanShoot canShoot;
	List<PlasmaBooster> activePlasmaBoosters;

	public void AddBooster(PlasmaBooster plasmaBooster){
		//Start executes after AddBooster happens so we eed to initialize here for now
		if (activePlasmaBoosters == null){
			canShoot = GetComponent<CanShoot>();
			activePlasmaBoosters = new List<PlasmaBooster>();
		}
		if (!activePlasmaBoosters.Contains(plasmaBooster)){
			activePlasmaBoosters.Add(plasmaBooster);
		}
	}

	public void RemoveBooster(PlasmaBooster plasmaBooster){
		activePlasmaBoosters.Remove(plasmaBooster);
	}

	public void UpdateBoostBonus(){
		if (!canShoot.damageMultipliers.ContainsKey(DAMAGE_BONUS_STRING)){
			canShoot.damageMultipliers.Add(DAMAGE_BONUS_STRING, 0);
		}
		canShoot.damageMultipliers[DAMAGE_BONUS_STRING] = 0;
		foreach(PlasmaBooster plasmaBooster in activePlasmaBoosters){
			canShoot.damageMultipliers[DAMAGE_BONUS_STRING] += plasmaBooster.boostAmount;
		}
		if (canShoot.damageMultipliers[DAMAGE_BONUS_STRING] != 0){
			Color plasmaColor = DamageTypeManager.GetDamageTypeColor(DamageTypeManager.DamageType.Plasma);
			plasmaColor.a = 0.7f;
			GetComponent<LocalizableOnMap>().cell.tile.Pulsate(
				plasmaColor,
				999999f, 
				2f);
		}
	}

}
