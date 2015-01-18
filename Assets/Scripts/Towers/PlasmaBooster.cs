using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaBooster : MonoBehaviour {

	public float boostAmount = 15f; // 15f means 15% boost

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
		SubscribeToDirectlyAdjacentTowerAdds();
		SubscribeToOwnTowerUpgrade();
		SubscribeToOwnTowerSell();
	}

	//When a new tower is added next to the booster, update the bonuses
	void SubscribeToDirectlyAdjacentTowerAdds(){
		List<Cell> directlyAdjacentCells = map.FindDirectlyAdjacentCells(localizableOnMap.cell);
		foreach(Cell cell in directlyAdjacentCells){
			map.AddOnTowerAddCallback(cell, UpdateBoost);
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
			boostedTower.UpdateBoostBonus();
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
				boostedTowers.Add(cell.localizableOnMap.GetComponent<PlasmaBoostable>());
			}
		}
	}
}
