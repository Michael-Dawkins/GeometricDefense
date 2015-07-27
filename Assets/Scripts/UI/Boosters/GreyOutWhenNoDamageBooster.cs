using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GreyOutWhenNoDamageBooster : GreyOutWhenNoBooster {
    
    void Start() {
        base.Start();
        playerBoostertiles.AddDamageBoosterAmountListener(UpdateColor);
    }

}
