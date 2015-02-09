﻿using UnityEngine;
using System.Collections;

public class UpgradeDependent : MonoBehaviour {

    public string upgradeName;
    PlayerUpgrades playerUpgrades;

	void Start () {
        playerUpgrades = PlayerUpgrades.instance;
        playerUpgrades.PlayerUgradeBought += UpdateGameObjectState;
        playerUpgrades.OnLoad(UpdateGameObjectState);
	}

    void UpdateGameObjectState() {
        gameObject.SetActive(PlayerUpgrades.instance.HasPLayerGotUpgrade(upgradeName));
    }
	
}
