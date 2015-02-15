using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour {
    public static EndGameMenu instance;
    public GameObject menuObj;
    public Text endGameLabel;
    public Text upgradePointsEarnedLabel;
    void Awake() {
        instance = this;
    }

	void Start () {
	
	}

    public void Show() {
        menuObj.SetActive(true);
        UpdateView();
    }

    public void Hide() {
        menuObj.SetActive(false);
    }

    void UpdateView() {
        string endGameText = "Mission " + (PlayerLife.instance.playerDied ? "Failed" : "Complete");
        endGameLabel.text = endGameText;
        string pointsEarnedText = "Points earned: ";
        upgradePointsEarnedLabel.text = pointsEarnedText + PlayerUpgrades.instance.lastAmountEarned;
    }
}
