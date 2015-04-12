using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaBooster : MonoBehaviour {

	public float boostAmount = 10f; // 10f means 10% boost
    float addedBonusPerLevel = 3f;

	List<PlasmaBoostable> boostedTowers;
	TowerTypeManager.TowerType towerType;
	LocalizableOnMap localizableOnMap;
	Map map;

	void Start () {
		boostedTowers = new List<PlasmaBoostable>();
		towerType = GetComponent<CanShoot>().towerType;
		localizableOnMap = GetComponent<LocalizableOnMap>();
		map = Map.instance;

		UpdateBoost();
		SubscribeToDirectlyAdjacentTowerEvents();
		SubscribeToOwnTowerUpgrade();
		SubscribeToOwnTowerSell();
	}

    public float GetBoostAmount() {
        UpgradableTower upgradableTower = GetComponent<UpgradableTower>();
        return boostAmount + (upgradableTower.towerLevel - 1) * addedBonusPerLevel;
    }

	//When a new tower is added or sold next to the booster, update the bonuses
    void SubscribeToDirectlyAdjacentTowerEvents() {
		List<Cell> directlyAdjacentCells = map.FindDirectlyAdjacentCells(localizableOnMap.cell);
		foreach(Cell cell in directlyAdjacentCells){
			map.AddOnTowerAddCallback(cell, UpdateBoost);
            map.AddOnTowerSellCallback(cell, UpdateBoost);
		}
	}

	//If the booster tower get upgraded, we want to know it to tell that to al the boostables so they update their bonuses
	void SubscribeToOwnTowerUpgrade(){
		map.AddOnTowerUpgradeCallback(localizableOnMap.cell, UpdateBoost);
	}

	void SubscribeToOwnTowerSell(){
		map.AddOnTowerSellCallback(localizableOnMap.cell, RemoveBoosterFromBoostedTowers);
	}

	void RemoveBoosterFromBoostedTowers(){
		foreach(PlasmaBoostable boostedTower in boostedTowers){
			boostedTower.RemoveBooster(this);
		}
	}

	void UpdateBoost(){
		UpdateBoostedTowerList();
		ApplyEffectOnToPlasmaBoostables();
	}

	void ApplyEffectOnToPlasmaBoostables(){
		foreach(PlasmaBoostable boostedTower in boostedTowers){
			boostedTower.AddBooster(this);
			boostedTower.UpdateBoostBonus();
		}
	}

	void UpdateBoostedTowerList(){
		boostedTowers = new List<PlasmaBoostable>();
		List<Cell> directlyAdjacentCells = map.FindDirectlyAdjacentCells(localizableOnMap.cell);
		foreach(Cell cell in directlyAdjacentCells){
			if (cell.localizableOnMap == null){
				continue;
			}
			CanShoot canShoot = cell.localizableOnMap.GetComponent<CanShoot>();
			if (canShoot.towerType == towerType){
                PlasmaBoostable plasmaBoostable = cell.localizableOnMap.GetComponent<PlasmaBoostable>();
                //during the selling / notify sell process, we might encounter the 
                //beingSold but still present and thus applicable for boosters
                //we need to filter that
                if (!plasmaBoostable.beingSold) {
                    boostedTowers.Add(plasmaBoostable);
                }
			}
		}
	}

    void OnDestroy() {
        List<Cell> directlyAdjacentCells = map.FindDirectlyAdjacentCells(localizableOnMap.cell);
        foreach (Cell cell in directlyAdjacentCells) {
            map.RemoveOnTowerAddCallback(cell, UpdateBoost);
            map.RemoveOnTowerSellCallback(cell, UpdateBoost);
        }
        map.RemoveOnTowerUpgradeCallback(localizableOnMap.cell, UpdateBoost);
        map.RemoveOnTowerSellCallback(localizableOnMap.cell, RemoveBoosterFromBoostedTowers);
    }
}
