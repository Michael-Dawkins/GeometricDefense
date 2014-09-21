using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradableTower : MonoBehaviour {

	public float upgradeCost;
	public GameObject towerRangePrefab;

	Canvas upgradeCanvas;
	BoxCollider2D clickableCollider;
	GameObject clickableObject;
	PlayerMoney playerMoney;
	GameObject upgradeButtonObject;
	GameObject sellButtonObject;
	GameObject towerSpriteObj;
	Text upgradeCostLabel;
	Map map;
	GameObject currentTowerRangeObject;

	// Use this for initialization
	void Start() {
		map = GameObject.Find("Map").GetComponent<Map>();
		towerSpriteObj = transform.Find("TowerSprite").gameObject;
		GameObject playerState = GameObject.Find("PlayerState");
		playerMoney = playerState.GetComponent<PlayerMoney>();
		upgradeCanvas =  GetComponentsInChildren<Canvas>(true)[0];
		upgradeButtonObject = upgradeCanvas.transform.Find("UpgradeButton").gameObject;
		sellButtonObject = upgradeCanvas.transform.Find("SellButton").gameObject;
		Image upgradeImage = upgradeButtonObject.GetComponent<Image>();
		upgradeImage.color = towerSpriteObj.GetComponent<SpriteRenderer>().color;
	}
	
	public void DisplayUpgradeButton() {
		upgradeButtonObject.SetActive(true);
		EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		eventSystem.SetSelectedGameObject(upgradeButtonObject, new BaseEventData(eventSystem));
		upgradeCostLabel = upgradeButtonObject.GetComponentInChildren<Text>();
		upgradeCostLabel.text = "$" + upgradeCost;
		DisplayTowerRange();
	}

	public void DisplaySellButton() {
		sellButtonObject.SetActive(true);
		DisplayTowerRange();
	}

	public void DisplayTowerRange(){
		if (currentTowerRangeObject == null){
			CanShoot canShoot = GetComponent<CanShoot>();
			currentTowerRangeObject = Instantiate (towerRangePrefab, transform.position, Quaternion.identity) as GameObject;
			GDUtils.ScaleTransformToXWorldUnit(
				currentTowerRangeObject.transform, (map.cellSize * canShoot.cellRange + map.cellSize / 2f) * 2f);
			SpriteRenderer towerRangeRenderer = currentTowerRangeObject.GetComponent<SpriteRenderer>();
			Color color = towerSpriteObj.GetComponent<SpriteRenderer>().color;
			towerRangeRenderer.color = new Color(color.r, color.g, color.b);
		}
	}

	public void HideTowerRange(){
		Destroy(currentTowerRangeObject);
	}

	public void UpGradeTower(){
		if (playerMoney.Money >= (int) upgradeCost){
			playerMoney.Money -= (int) upgradeCost;
			CanShoot canShoot = GetComponent<CanShoot>();
			canShoot.shootingSpeed += 1f;
		}
		OnDeselect();
	}

	public void SellTower(){
		Debug.Log("Tower sold");
		Destroy(gameObject);
		Destroy(currentTowerRangeObject);
		Cell cell = map.GetCellAtPos(transform.position.x, transform.position.y);
		cell.isObstacle = false;
	}

	public void OnDeselect(){
		upgradeButtonObject.SetActive(false);
		sellButtonObject.SetActive(false);
		HideTowerRange();
	}
}
