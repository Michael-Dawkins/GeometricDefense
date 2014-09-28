using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UIState {

	public delegate void OnTowerSelection();

	static List<OnTowerSelection> towerSelectionCallbacks = new List<OnTowerSelection>();

	public static void AddTowerSelectionListener(OnTowerSelection callback){
		towerSelectionCallbacks.Add(callback);
	}

	public static void RemoveTowerSelectionListener(OnTowerSelection callback){
		towerSelectionCallbacks.Remove(callback);
	}

	public static void TowerSelection(){
		foreach( OnTowerSelection callback in towerSelectionCallbacks){
			callback();
		}
	}
}
