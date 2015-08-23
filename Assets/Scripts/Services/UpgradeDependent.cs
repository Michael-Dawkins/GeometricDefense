using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ComposableVisibility))]
public class UpgradeDependent : MonoBehaviour {

    private string VISIBILITY_KEY = "UpgradeDependent";

    public string upgradeName;
    PlayerUpgrades playerUpgrades;
    ChangeVisibilityWhenPlacingTiles changeVisibilityWhenPlacingTiles;
    ComposableVisibility composableVisibility;

    void Start () {
        composableVisibility = GetComponent<ComposableVisibility>();
        playerUpgrades = PlayerUpgrades.instance;
        playerUpgrades.PlayerUgradeBought += UpdateGameObjectState;
        playerUpgrades.OnLoad(UpdateGameObjectState);
        changeVisibilityWhenPlacingTiles = GetComponent<ChangeVisibilityWhenPlacingTiles>();
	}

    void UpdateGameObjectState() {
        composableVisibility.setVisibilityKey(VISIBILITY_KEY, ShouldBeDisplayed());
    }

    public bool ShouldBeDisplayed() {
        return PlayerUpgrades.instance.HasPLayerGotUpgrade(upgradeName);
    }
	
}
