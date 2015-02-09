using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;

[Serializable()]
public class Upgrade : System.Runtime.Serialization.ISerializable {

    public string name;
    public float cost;
    public bool bought = false;

    public Upgrade(JSONObject upgradeJson) {
        int nameIndex = upgradeJson.keys.IndexOf("upgradeName");
        this.name = upgradeJson.list[nameIndex].str;
        int costIndex = upgradeJson.keys.IndexOf("upgradeCost");
        this.cost = upgradeJson.list[costIndex].n;
    }

    public Upgrade(SerializationInfo info, StreamingContext ctxt) {
        name = (string) info.GetValue("name", typeof (string));
        cost = (float) info.GetValue("cost", typeof (float));
        bought = (bool) info.GetValue("bought", typeof (bool));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("name", name);
        info.AddValue("cost", cost);
        info.AddValue("bought", bought);
    }

}
