using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndGameMenu : MonoBehaviour {
    public static EndGameMenu instance;
    public GameObject menuObj;
    public Text endGameLabel;
    public Text rewardText;
    public delegate void OnEndMenu();
    private List<OnEndMenu> callbacks = new List<OnEndMenu>();
    public bool isShown = false;
    void Awake() {
        instance = this;
    }

	void Start () {
	
	}

    public void Show() {
        if (menuObj.activeSelf)
            return;
        menuObj.SetActive(true);
        UpdateView();
        isShown = true;
        NotifyEndMenuListeners();
    }

    void OnDisable() {
        isShown = false;
    }

    public void Hide() {
        menuObj.SetActive(false);
        isShown = false;
        NotifyEndMenuListeners();
    }

    void UpdateView() {
        string endGameText = "Mission " + (PlayerLife.instance.playerDied ? "Failed" : "Complete");
        endGameLabel.text = endGameText;
        if (PlayerLife.instance.playerDied)
            ShowFunnyMessage();
        else
            EarnAReward();
    }

    private void ShowFunnyMessage() {
        string[] messages = new [] {
            "Life can be tough",
            "You'll do better",
            "HAHA! Sorry...",
            "The answer is 42",
            "It's the doctor!",
            "Is it a bird? Is it a plane?",
            "Insert coin",
            "KNOCK OUT",
            "Instructions unclear, got stuck",
            "Come on, try one more time!"
        };
        int index = Random.Range(0, messages.Length - 1);
        rewardText.text = messages[index];
    }

    private void EarnAReward() {
        if (RandomChance(0.5f)) {
            EarnUpgradePoints();
        } else {
            EarnABoosterTile();
        }
    }

    private void EarnUpgradePoints() {
        rewardText.text = 
            "You won " + PlayerUpgrades.instance.lastAmountEarned + " upgrade points!";
    }

    private void EarnABoosterTile() {
        if (RandomChance(0.5f)) {
            PlayerBoosterTiles.instance.EarnOneDamageBooster();
            rewardText.text = "You won 1 damage booster!";
        } else {
            PlayerBoosterTiles.instance.EarnOneRangeBooster();
            rewardText.text = "You won 1 range booster!";
        }
    }

    //a chance of 0.7 means a 70% chance of returning true
    private bool RandomChance(float chance) {
        return Random.Range(0, 1f) > chance;
    }

    public void AddEndMenuListener(OnEndMenu callback) {
        callbacks.Add(callback);
    }

    public void RemoveEndMenuListener(OnEndMenu callback) {
        callbacks.Remove(callback);
    }

    public void NotifyEndMenuListeners() {
        foreach (OnEndMenu callback in callbacks) {
            callback();
        }
    }
}
