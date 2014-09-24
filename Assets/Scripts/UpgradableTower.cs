﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradableTower : MonoBehaviour {

	public float upgradeCost;
	public float towerCost;
	public GameObject towerRangePrefab;

	int towerLevel = 1;
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
	ClickReceptor clickReceptor;
	GameObject upgradeButtonBackground;
	GameObject sellButtonBackground;

	// Use this for initialization
	void Start() {
		map = GameObject.Find("Map").GetComponent<Map>();
		towerSpriteObj = transform.Find("TowerSprite").gameObject;
		GameObject playerState = GameObject.Find("PlayerState");
		playerMoney = playerState.GetComponent<PlayerMoney>();
		upgradeCanvas =  GetComponentsInChildren<Canvas>(true)[0];
		upgradeButtonObject = upgradeCanvas.transform.Find("UpgradeButton").gameObject;
		upgradeButtonBackground = upgradeCanvas.transform.Find("UpgradeButtonBackground").gameObject;
		sellButtonBackground = upgradeCanvas.transform.Find("SellButtonBackground").gameObject;
		sellButtonObject = upgradeCanvas.transform.Find("SellButton").gameObject;
		Image upgradeImage = upgradeButtonObject.GetComponent<Image>();
		upgradeImage.color = towerSpriteObj.GetComponent<SpriteRenderer>().color;
		clickReceptor = GameObject.Find("ClickReceptorCanvas").GetComponentInChildren<ClickReceptor>();
		clickReceptor.AddOnClickListener(OnDeselect);
	}
	
	public void DisplayUpgradeButton() {
		upgradeButtonObject.SetActive(true);
		upgradeButtonBackground.SetActive(true);
		upgradeCostLabel = upgradeButtonObject.GetComponentInChildren<Text>();
		upgradeCostLabel.text = "$" + upgradeCost;
		DisplayTowerRange();
	}

	public void DisplaySellButton() {
		sellButtonObject.SetActive(true);
		sellButtonBackground.SetActive(true);
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
			towerCost += upgradeCost;
			towerLevel += 1;
			SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.sprite = Resources.Load("tower-circle-" + towerLevel) as Sprite;
		}
		OnDeselect();
	}

	public void SellTower(){
		Debug.Log("Tower sold for " + (int)(towerCost / 2f));
		playerMoney.Money += (int)(towerCost / 2f);
		Destroy(gameObject);
		Destroy(currentTowerRangeObject);
		Cell cell = map.GetCellAtPos(transform.position.x, transform.position.y);
		cell.isObstacle = false;
		clickReceptor.RemoveOnClickListener(OnDeselect);
		PathFinder pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
		pathFinder.requestNewGlobalPath(map.GetCellAt(map.xStart,map.yStart), map.GetCellAt(map.xGoal, map.yGoal));
		pathFinder.RecalculatePathForCurrentEnemies();
	}

	public void OnDeselect(){
		upgradeButtonObject.SetActive(false);
		sellButtonObject.SetActive(false);
		upgradeButtonBackground.SetActive(false);
		sellButtonBackground.SetActive(false);
		HideTowerRange();
	}
}
