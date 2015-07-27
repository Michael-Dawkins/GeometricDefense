using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RangeBoosterAmountLabel : MonoBehaviour {

    Text rangeBoosterAmountLabel;
    PlayerBoosterTiles playerBoosterTiles;

    // Use this for initialization
    void Start() {
        playerBoosterTiles = PlayerBoosterTiles.instance;
        playerBoosterTiles.AddRangeBoosterAmountListener(UpdateLabel);
        rangeBoosterAmountLabel = GetComponent<Text>();
        UpdateLabel(playerBoosterTiles.CurrentRangeBoosterAmount);
    }

    void UpdateLabel(int amount) {
        rangeBoosterAmountLabel.text = amount.ToString();
    }

    void Destroy() {
        playerBoosterTiles.RemoveRangeBoosterAmountListener(UpdateLabel);
    }
}
