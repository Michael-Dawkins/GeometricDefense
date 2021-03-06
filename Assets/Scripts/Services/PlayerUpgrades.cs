﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[Serializable]
public struct PlayerUpgradeImage {
    public string upgradeName;
    public Sprite sprite;
}

public class PlayerUpgrades: MonoBehaviour {

    //assign tutorial images in inspector to automaticall get them displayed to
    //the user when the associated upgrade name is bought by the player
    public PlayerUpgradeImage[] tutorialImages;
    static string PLAYER_PREFS_KEY = "UPGRADES";
    public static PlayerUpgrades instance;
    PlayerUpgradeMoney playerUpgradeMoney;
    float moneyPerWin = 15f;
    public float lastAmountEarned = 0f;
    
    public delegate void OnPlayerUpgradesLoaded();
    List<OnPlayerUpgradesLoaded> loadCallbacks = new List<OnPlayerUpgradesLoaded>();
    public event OnPlayerUpgradeBought PlayerUgradeBought = delegate { };
    public delegate void OnPlayerUpgradeBought();
    [HideInInspector]
    public bool loaded = false;

    public List<Upgrade> Upgrades {
        get { return _Upgrades; }
    }

    List<Upgrade> _Upgrades;

    void Awake() {
        instance = this;
    }

	void Start () {
        LoadUpgradesFromPrefsAndJson();
        loadCallbacks.ForEach(c => c());
        loaded = true;
	}

    void LoadUpgradesFromPrefsAndJson() {
        playerUpgradeMoney = PlayerUpgradeMoney.instance;
        List<Upgrade> upgradesLoaded = null;
        if (PlayerPrefs.HasKey(PLAYER_PREFS_KEY)) {
            upgradesLoaded = SaveTools.LoadFromPlayerPrefs(PLAYER_PREFS_KEY) as List<Upgrade>;
        } else {
            Debug.Log("First load, no upgrades found in player prefs");
        }
        _Upgrades = new List<Upgrade>();
        TextAsset upgradesTextAsset = (TextAsset)Resources.Load("upgrades", typeof(TextAsset));
        JSONObject upgradesJsonRoot = new JSONObject(upgradesTextAsset.text);
        JSONObject upgradesJson = upgradesJsonRoot.list[upgradesJsonRoot.keys.IndexOf("upgrades")];
        upgradesJson.list.ForEach(u => _Upgrades.Add(new Upgrade(u)));
        if (upgradesLoaded != null) {
            _Upgrades.ForEach(u => {
                Upgrade upgradeFromPlayerPrefs = upgradesLoaded.Find(e => e.name == u.name);
                if (upgradeFromPlayerPrefs != null) {
                    u.bought = upgradeFromPlayerPrefs.bought;
                }
            });
        }
    }

    public void OnLoad(OnPlayerUpgradesLoaded callback) {
        if (!loaded) {
            loadCallbacks.Add(callback);
        } else {
            callback();
        }
    }

    //Win the default upgrade points amount specified in moneyPerwin field
    public void WinUpgradePoints(){
        playerUpgradeMoney.Money += moneyPerWin;
        lastAmountEarned = moneyPerWin;
    }

    public void ResetLastAmountEarned() {
        lastAmountEarned = 0f;
    }

    public void ClearPlayerPrefs() {
        Debug.Log("PlayerPref " + PLAYER_PREFS_KEY + " was deleted from PlayPrefs");
        PlayerPrefs.DeleteKey(PLAYER_PREFS_KEY);
    }

    public void BuyUpgrade(Upgrade upgrade) {
        if (upgrade.bought) {
            Debug.Log("Upgrade " + upgrade.name + " is already own");
            return;
        }
        if (upgrade.cost <= playerUpgradeMoney.Money) {
            Debug.Log("Bought " + upgrade.name + " for " 
                + upgrade.cost + " player upgrade money");
            playerUpgradeMoney.Money -= upgrade.cost;
            upgrade.bought = true;
            PlayerUgradeBought();
            SaveTools.SaveInPlayerPrefs(PLAYER_PREFS_KEY, Upgrades);
            DisplayTutorial(upgrade.name);
        } else {
            Debug.Log("Couldn't buy " + upgrade.name + " for "
                + upgrade.cost + "player upgrade money, player only has "
                + playerUpgradeMoney.Money + " playerUpgradeMoney");
        }
    }

    void DisplayTutorial(string upgradeName) {
        foreach(PlayerUpgradeImage playerUpgradeImage in tutorialImages) {
            if (playerUpgradeImage.upgradeName == upgradeName)
                ImagePopupManager.instance.DisplayPopup(playerUpgradeImage.sprite);
        }
    }

    public bool HasPLayerGotUpgrade(string upgradeName) {
        Upgrade upgrade = Upgrades.Find(u => u.name.Equals(upgradeName));
        if (upgrade == null) {
            Debug.LogError("Cannot test if player has " 
                + upgradeName + " because it is not present in upgrades.txt");
            return false;
        }
        return upgrade.bought;
    }
}
