using UnityEngine;
using System.Collections;

public class Upgrade {

    public string name;
    public float cost;
    public bool bought = false;

    public Upgrade(JSONObject upgradeJson) {
        int nameIndex = upgradeJson.keys.IndexOf("upgradeName");
        this.name = upgradeJson.list[nameIndex].str;
        int costIndex = upgradeJson.keys.IndexOf("upgradeCost");
        this.cost = upgradeJson.list[costIndex].n;
    }
}
