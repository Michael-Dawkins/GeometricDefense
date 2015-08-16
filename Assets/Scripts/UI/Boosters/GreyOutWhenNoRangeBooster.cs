using UnityEngine;
using System.Collections;

public class GreyOutWhenNoRangeBooster : GreyOutWhenNoBooster {

    void Start() {
        base.Start();
        playerBoostertiles.AddRangeBoosterAmountListener(UpdateColor);
        UpdateColor(playerBoostertiles.CurrentRangeBoosterAmount);
    }

}