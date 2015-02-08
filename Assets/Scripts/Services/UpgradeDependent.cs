using UnityEngine;
using System.Collections;

public class UpgradeDependent : MonoBehaviour {

    public string upgradeName;
    PlayerUpgrades playerUpgrades;

	void Start () {
        playerUpgrades = PlayerUpgrades.instance;
        playerUpgrades.PlayerUgradeBought += UpdateGameObjectState;
        //My promise pattern
        if (playerUpgrades.loaded) {
            UpdateGameObjectState();
        } else {
            playerUpgrades.PlayerUpgradesLoaded += UpdateGameObjectState;
        }
	}

    void UpdateGameObjectState() {
        gameObject.SetActive(PlayerUpgrades.instance.HasPLayerGotUpgrade(upgradeName));
    }
	
}
