using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageBoosterAmountLabel : MonoBehaviour {

    Text damageBoosterAmountLabel;
    PlayerBoosterTiles playerBoosterTiles;

	// Use this for initialization
	void Start () {
        playerBoosterTiles = PlayerBoosterTiles.instance;
        playerBoosterTiles.AddDamageBoosterAmountListener(UpdateLabel);
        damageBoosterAmountLabel = GetComponent<Text>();
        UpdateLabel(playerBoosterTiles.CurrentDamageBoosterAmount);
	}

    void UpdateLabel(int amount) {
        damageBoosterAmountLabel.text = amount.ToString();
    }

    void Destroy() {
        playerBoosterTiles.RemoveDamageBoosterAmountListener(UpdateLabel);
    }
	
}
