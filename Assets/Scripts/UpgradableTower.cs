using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradableTower : MonoBehaviour {

	Canvas upgradeCanvas;
	BoxCollider2D clickableCollider;
	GameObject clickableObject;
	Map map;
	PlayerMoney playerMoney;
	GameObject upgradeButtonObject;

	// Use this for initialization
	void Start() {
		map = GameObject.Find("Map").GetComponent<Map>();
		GameObject playerState = GameObject.Find("PlayerState");
		playerMoney = playerState.GetComponent<PlayerMoney>();
		upgradeCanvas =  GetComponentsInChildren<Canvas>(true)[0];
		upgradeButtonObject = upgradeCanvas.transform.Find("UpgradeButton").gameObject;
	}
	
	// Update is called once per frame
	void Update() {}

	public void DisplayUpgradeButton() {
		Debug.Log("display upgrade button");
		upgradeButtonObject.SetActive(true);
		Button button = upgradeButtonObject.GetComponent<Button>();
		EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		eventSystem.SetSelectedGameObject(button.gameObject, new BaseEventData(eventSystem));
	}

	public void UpGradeTower(){
		Debug.Log("Upgrading tower");
		if (playerMoney.Money >= 100){
			playerMoney.Money -= 100;
			CanShoot canShoot = GetComponent<CanShoot>();
			canShoot.shootingSpeed += 1f;
		}
		OnDeselect();
	}

	public void OnDeselect(){
		Debug.Log("Deselecting tower");
		upgradeButtonObject.SetActive(false);
	}
}
