using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeMoneyLabel : MonoBehaviour {

    Text text;

	void Start () {
        text = GetComponent<Text>();
        UpdateLabel();
        PlayerUpgradeMoney.instance.PlayerUpgradeMoneyChange += UpdateLabel;
	}

    void UpdateLabel() {
        text.text = "Upgrade points: " + PlayerUpgradeMoney.instance.Money;
    }

    void OnDestroy() {
        PlayerUpgradeMoney.instance.PlayerUpgradeMoneyChange -= UpdateLabel;
    }
}
