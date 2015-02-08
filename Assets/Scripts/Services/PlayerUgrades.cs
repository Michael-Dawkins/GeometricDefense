using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUgrades : MonoBehaviour {

    public static PlayerUgrades instance;
    PlayerUpgradeMoney playerUpgradeMoney;
    public event OnPlayerUpgradesLoaded PlayerUpgradesLoaded;
    public delegate void OnPlayerUpgradesLoaded();
    public bool loaded = false;

    public List<Upgrade> Upgrades {
        get { return _Upgrades; }
    }

    List<Upgrade> _Upgrades;

    void Awake() {
        instance = this;
    }

	void Start () {
        playerUpgradeMoney = PlayerUpgradeMoney.instance;
        _Upgrades = new List<Upgrade>();
        TextAsset upgradesTextAsset = (TextAsset)Resources.Load("upgrades", typeof(TextAsset));
        JSONObject upgradesJsonRoot = new JSONObject(upgradesTextAsset.text);
        JSONObject upgradesJson = upgradesJsonRoot.list[upgradesJsonRoot.keys.IndexOf("upgrades")];
        upgradesJson.list.ForEach(u => _Upgrades.Add(new Upgrade(u)));
        _Upgrades.ForEach(u => Debug.Log(u.name));
        if (PlayerUpgradesLoaded != null) {
            PlayerUpgradesLoaded();
        }
        loaded = true;
	}

    public void BuyUpgrade(Upgrade upgrade) {
        if (upgrade.cost <= playerUpgradeMoney.Money) {
            Debug.Log("Bought " + upgrade.name + " for " 
                + upgrade.cost + " player upgrade money");
            playerUpgradeMoney.Money -= upgrade.cost;
            upgrade.bought = true;
            //TODO save in PlayerPrefs
        } else {
            Debug.Log("Couldn't buy " + upgrade.name + " for "
                + upgrade.cost + "player upgrade money, player only has"
                + playerUpgradeMoney.Money);
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
