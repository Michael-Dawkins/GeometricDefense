using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    public ScrollRect upgradeMenuScrollRect;
    public RectTransform buttonPrefab;
    bool isMenuDisplayed = false;
    GameObject upgradeListPanel;

    void Start() {
        //Like an angular promise
        if (PlayerUpgrades.instance.loaded) {
            Init();
        } else {
            PlayerUpgrades.instance.PlayerUpgradesLoaded += Init;
        }
    }

    void Init() {
        upgradeListPanel = upgradeMenuScrollRect.transform.Find("UpgradeListPanel").gameObject;
        ClickReceptor.instance.AddOnClickListener(OnClickReceptorClick);
        ConstructButtonsFromUpgradeslist(PlayerUpgrades.instance.Upgrades);
    }

    public void ShowMenu() {
        upgradeMenuScrollRect.gameObject.SetActive(true);
        isMenuDisplayed = true;
    }

    public void HideMenu() {
        upgradeMenuScrollRect.gameObject.SetActive(false);
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
            lastButton = buttonTrans;
        }
    }

    void AddButtonListener(Button button, Upgrade upgrade) {
        button.onClick.AddListener(() => PlayerUpgrades.instance.BuyUpgrade(upgrade));
    }
}