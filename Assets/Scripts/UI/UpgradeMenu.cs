using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    public GameObject upgradeMenu;
    public RectTransform buttonPrefab;
    bool isMenuDisplayed = false;
    GameObject upgradeListPanel;
    Dictionary<Upgrade, Text> upgradeTexts;

    void Start() {
        PlayerUpgrades.instance.OnLoad(Init);
    }

    void Init() {
        upgradeTexts = new Dictionary<Upgrade, Text>();
        upgradeListPanel = upgradeMenu.transform.Find("PlayerUpgradesView/UpgradeListPanel").gameObject;
        ClickReceptor.instance.AddOnClickListener(OnClickReceptorClick);
        ConstructButtonsFromUpgradeslist(PlayerUpgrades.instance.Upgrades);
        UpdateButtonStates();
        PlayerUpgrades.instance.PlayerUgradeBought += UpdateButtonStates;
    }

    public void ShowMenu() {
        upgradeMenu.gameObject.SetActive(true);
        isMenuDisplayed = true;
    }

    public void HideMenu() {
        upgradeMenu.gameObject.SetActive(false);
        isMenuDisplayed = false;
    }

    void OnClickReceptorClick() {
        if (isMenuDisplayed) {
            HideMenu();
        }
    }

    void ConstructButtonsFromUpgradeslist(List<Upgrade> upgrades) {
        RectTransform lastButton = null;
        foreach (Upgrade upgrade in upgrades) {
            RectTransform buttonTrans = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as RectTransform;
            buttonTrans.localScale = Vector3.one;
            buttonTrans.SetParent(upgradeListPanel.transform, false);
            if (lastButton != null) {
                buttonTrans.anchoredPosition = new Vector2(0, (lastButton.anchoredPosition.y - buttonTrans.rect.height));
            } else {
                buttonTrans.anchoredPosition = new Vector2(0, -(buttonTrans.rect.height / 2f));
            }
            Text buttonText = buttonTrans.FindChild("Text").gameObject.GetComponent<Text>();
            Button button = buttonTrans.GetComponent<Button>();
            AddButtonListener(button, upgrade);
            buttonText.text = upgrade.name + ": " + upgrade.cost;
            upgradeTexts.Add(upgrade, buttonText);
            lastButton = buttonTrans;
        }
    }

    void AddButtonListener(Button button, Upgrade upgrade) {
        button.onClick.AddListener(() => PlayerUpgrades.instance.BuyUpgrade(upgrade));
    }

    void UpdateButtonStates() {
        foreach(KeyValuePair<Upgrade, Text> entry in upgradeTexts){
            entry.Value.SetAlpha(entry.Key.bought ? 0.3f : 1f);
        }
    }

    void OnDestroy() {
        PlayerUpgrades.instance.PlayerUgradeBought -= UpdateButtonStates;
    }
}