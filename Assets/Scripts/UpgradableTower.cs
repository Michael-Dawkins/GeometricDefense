using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradableTower : MonoBehaviour {

	public float upgradeCost;

	Canvas upgradeCanvas;
	BoxCollider2D clickableCollider;
	GameObject clickableObject;
	PlayerMoney playerMoney;
	GameObject upgradeButtonObject;
	GameObject towerSpriteObj;
	Text upgradeCostLabel;

	// Use this for initialization
	void Start() {
		towerSpriteObj = transform.Find("TowerSprite").gameObject;
		GameObject playerState = GameObject.Find("PlayerState");
		playerMoney = playerState.GetComponent<PlayerMoney>();
		upgradeCanvas =  GetComponentsInChildren<Canvas>(true)[0];
		upgradeButtonObject = upgradeCanvas.transform.Find("UpgradeButton").gameObject;
		Image upgradeImage = upgradeButtonObject.GetComponent<Image>();
		upgradeImage.color = towerSpriteObj.GetComponent<SpriteRenderer>().color;
	}
	
	public void DisplayUpgradeButton() {
		upgradeButtonObject.SetActive(true);
		Button button = upgradeButtonObject.GetComponent<Button>();
		EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		eventSystem.SetSelectedGameObject(button.gameObject, new BaseEventData(eventSystem));
		upgradeCostLabel = upgradeButtonObject.GetComponentInChildren<Text>();
		upgradeCostLabel.text = "$" + upgradeCost;
	}

	public void UpGradeTower(){
		if (playerMoney.Money >= (int) upgradeCost){
			playerMoney.Money -= (int) upgradeCost;
			CanShoot canShoot = GetComponent<CanShoot>();
			canShoot.shootingSpeed += 1f;
		}
		OnDeselect();
	}

	public void OnDeselect(){
		upgradeButtonObject.SetActive(false);
	}
}
