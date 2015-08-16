using UnityEngine;
using System.Collections;

public class GreyOutWhenNoDamageBooster : GreyOutWhenNoBooster {
    
    void Start() {
        base.Start();
        playerBoostertiles.AddDamageBoosterAmountListener(UpdateColor);
        UpdateColor(playerBoostertiles.CurrentDamageBoosterAmount);
    }

}
