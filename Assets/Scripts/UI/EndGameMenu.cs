using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndGameMenu : MonoBehaviour {
    public static EndGameMenu instance;
    public GameObject menuObj;
    public Text endGameLabel;
    public Text upgradePointsEarnedLabel;
    public delegate void OnEndMenu();
    private List<OnEndMenu> callbacks = new List<OnEndMenu>();
    public bool isShown = false;
    void Awake() {
        instance = this;
    }

	void Start () {
	
	}

    public void Show() {
        menuObj.SetActive(true);
        UpdateView();
        isShown = true;
        NotifyEndMenuListeners();
    }

    public void Hide() {
        menuObj.SetActive(false);
        isShown = false;
        NotifyEndMenuListeners();
    }

    void UpdateView() {
        string endGameText = "Mission " + (PlayerLife.instance.playerDied ? "Failed" : "Complete");
        endGameLabel.text = endGameText;
        string pointsEarnedText = "Upgrade points earned: ";
        upgradePointsEarnedLabel.text = pointsEarnedText + PlayerUpgrades.instance.lastAmountEarned;
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
