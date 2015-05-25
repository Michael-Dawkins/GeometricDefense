using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaBoostable : MonoBehaviour {

    public bool beingSold = false;//used because when we have 2 adjacents boosters, selling one
    //will not inform the other corretly that it is disapearing, we need a way to know that
    //a currently being sold tower cannot be taken into account when updating boosted tower list
	public static string DAMAGE_BONUS_STRING= "PlasmaBoost";
    public static string CELL_DAMAGE_BONUS_STRING = "cellPlasmaBoost";
	CanShoot canShoot;
	List<PlasmaBooster> activePlasmaBoosters;

    void Start() {
        AddDamageMultiplierIfOnDamageBooster();
    }

    public void AddDamageMultiplierIfOnDamageBooster(){
        if (GetComponent<LocalizableOnMap>().cell.tile.tileType == Tile.TileType.DAMAGE_BOOSTER) {
            canShoot = GetComponent<CanShoot>();
            canShoot.damageMultipliers.Add(CELL_DAMAGE_BONUS_STRING, 50f);
        }
    }

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
        if (activePlasmaBoosters.Count == 0) {
            StopTilePulsations();
        }
        UpdateBoostBonus();
	}

	public void UpdateBoostBonus(){
		if (!canShoot.damageMultipliers.ContainsKey(DAMAGE_BONUS_STRING)){
			canShoot.damageMultipliers.Add(DAMAGE_BONUS_STRING, 0);
		}
		canShoot.damageMultipliers[DAMAGE_BONUS_STRING] = 0;
		foreach(PlasmaBooster plasmaBooster in activePlasmaBoosters){
			canShoot.damageMultipliers[DAMAGE_BONUS_STRING] += plasmaBooster.GetBoostAmount();
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

    void OnDestroy() {
        StopTilePulsations();
    }

    void StopTilePulsations() {
        LocalizableOnMap localizableOnMap = GetComponent<LocalizableOnMap>();
        if (localizableOnMap.cell != null) {
            localizableOnMap.cell.tile.StopPulsating();
        }
    }
}
