using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradableTower : MonoBehaviour {

	public float upgradeCost;
	public float towerCost;
	public GameObject towerRangePrefab;
	public int maxLevel;

	int towerLevel = 1;
	Canvas upgradeCanvas;
	BoxCollider2D clickableCollider;
	GameObject clickableObject;
	PlayerMoney playerMoney;
	GameObject upgradeButtonObject;
    GameObject dpsLabel;
	GameObject sellButtonObject;
//	GameObject towerSpriteCenterObj;
	GameObject towerSpriteGlowObj;
	Text upgradeCostLabel;
	Map map;
	GameObject currentTowerRangeObject;
	ClickReceptor clickReceptor;
	GameObject upgradeButtonBackground;
	GameObject sellButtonBackground;
	bool isContextualMenuOpen = false;
	LocalizableOnMap localizableOnMap;

	// Use this for initialization
	void Start() {
		map = Map.instance;
//		towerSpriteCenterObj = transform.Find("TowerSpriteCenter").gameObject;
		towerSpriteGlowObj = transform.Find("TowerSpriteGlow").gameObject;
		localizableOnMap = GetComponent<LocalizableOnMap>();
		playerMoney = PlayerMoney.instance;
		upgradeCanvas =  GetComponentsInChildren<Canvas>(true)[0];
		upgradeButtonObject = upgradeCanvas.transform.Find("UpgradeButton").gameObject;
        dpsLabel = upgradeCanvas.transform.Find("DpsLabel").gameObject;
		upgradeButtonBackground = upgradeCanvas.transform.Find("UpgradeButtonBackground").gameObject;
		sellButtonBackground = upgradeCanvas.transform.Find("SellButtonBackground").gameObject;
		sellButtonObject = upgradeCanvas.transform.Find("SellButton").gameObject;
		Image upgradeImage = upgradeButtonObject.GetComponent<Image>();
		upgradeImage.color = towerSpriteGlowObj.GetComponent<SpriteRenderer>().color;
		clickReceptor = GameObject.Find("ClickReceptorCanvas").GetComponentInChildren<ClickReceptor>();
		clickReceptor.AddOnClickListener(OnDeselect);
		UIState.AddTowerSelectionListener(OnDeselect);
		GameObject towerButtonObject =  transform.Find("TowerButtonCanvas/TowerButton").gameObject;
		towerButtonObject.GetComponent<TowerButton>().OnClick += new TowerButton.OnClickHandler(TowerSelection);
		playerMoney.AddOnMoneyChangeListener(UpdateUpgradeButtonAvailability);
	}

	void OnDestroy(){
		UIState.RemoveTowerSelectionListener(OnDeselect);
        if (clickReceptor == null) {
            if (GameObject.Find("ClickReceptorCanvas") == null) {
                return;
            }
            clickReceptor = GameObject.Find("ClickReceptorCanvas").GetComponentInChildren<ClickReceptor>();
        }
        clickReceptor.RemoveOnClickListener(OnDeselect);
        PlayerMoney.instance.RemoveOnMoneyChangeListener(UpdateUpgradeButtonAvailability);
	}

	public void TowerSelection(){
		if(!isContextualMenuOpen){
			UIState.TowerSelection();
			DisplaySellButton();
			DisplayUpgradeButton();
            DisplayDPSLabel();
			isContextualMenuOpen = true;
		} else {
			OnDeselect();
		}
	}

    public void DisplayDPSLabel() {
        CanShoot canShoot = GetComponent<CanShoot>();
        float dps = canShoot.DPS;
        dpsLabel.SetActive(true);
        dpsLabel.GetComponent<Text>().text = dps + " " + canShoot.DpsLabel;
    }
	
	public void DisplayUpgradeButton() {
		if (towerLevel < maxLevel){
			upgradeButtonObject.SetActive(true);
			upgradeButtonBackground.SetActive(true);
			upgradeCostLabel = upgradeButtonObject.GetComponentInChildren<Text>();
			upgradeCostLabel.text = "$" + upgradeCost;
			UpdateUpgradeButtonAvailability(playerMoney.Money);
		}
		DisplayTowerRange();
	}

	void UpdateUpgradeButtonAvailability(float amountOfMoney){
		if(!isContextualMenuOpen){
			return;
		}
		Image upgradeButtonImage = upgradeButtonObject.GetComponent<Image>();
		//Grey out button when not enough gold to upgrade
		if (upgradeCost < amountOfMoney){
			upgradeButtonImage.SetAlpha(1f);
			upgradeCostLabel.SetAlpha(1f);
		} else {
			upgradeButtonImage.SetAlpha(0.3f);
			upgradeCostLabel.SetAlpha(0.3f);
		}
	}

	public void DisplaySellButton() {
		sellButtonObject.SetActive(true);
		sellButtonBackground.SetActive(true);
	}

	public void DisplayTowerRange(){
		if (currentTowerRangeObject == null){
			CanShoot canShoot = GetComponent<CanShoot>();
			currentTowerRangeObject = Instantiate (towerRangePrefab, transform.position, Quaternion.identity) as GameObject;
            currentTowerRangeObject.transform.SetParent(transform);
            GDUtils.ScaleTransformToXWorldUnit(
				currentTowerRangeObject.transform, (map.cellSize * canShoot.cellRange + map.cellSize / 2f) * 2f);
			SpriteRenderer towerRangeRenderer = currentTowerRangeObject.GetComponent<SpriteRenderer>();
			Color color = towerSpriteGlowObj.GetComponent<SpriteRenderer>().color;
			towerRangeRenderer.color = new Color(color.r, color.g, color.b);
		}
	}

	public void HideTowerRange(){
		if (currentTowerRangeObject != null){
			Destroy(currentTowerRangeObject);
		}
	}

	public void UpGradeTower(){
		if (playerMoney.Money >= (int) upgradeCost && towerLevel < maxLevel){
			playerMoney.Money -= (int) upgradeCost;
			towerLevel += 1;
			CanShoot canShoot = GetComponent<CanShoot>();
			canShoot.Damage = Values.instance.Towers[canShoot.towerType].Levels[towerLevel].Damage;
			canShoot.cellRange = Values.instance.Towers[canShoot.towerType].Levels[towerLevel].CellRange;
			switch(canShoot.towerType){
			case TowerTypeManager.TowerType.Circle:
				CanShootBullets canShootBullets = GetComponent<CanShootBullets>();
				canShootBullets.shootingRate = Values.instance.Towers[canShoot.towerType].Levels[towerLevel].ShootingRate;
				break;
			case TowerTypeManager.TowerType.Square:
				CanShootOnArea canShootOnArea = GetComponent<CanShootOnArea>();
				canShootOnArea.UpdateWaveSize();
				break;
			case TowerTypeManager.TowerType.Triangle:
				CanShootLasers canShootLasers = GetComponent<CanShootLasers>();
				canShootLasers.coolDownTime = Values.instance.Towers[canShoot.towerType].Levels[towerLevel].CoolDown;
				break;
			}

			towerCost += upgradeCost;

			SpriteRenderer spriteRendererCenter = transform.Find("TowerSpriteCenter").GetComponent<SpriteRenderer>();
			string resourceName = spriteRendererCenter.sprite.name.Split('_')[0];
			spriteRendererCenter.sprite = Resources.Load(resourceName + "_" + towerLevel, typeof(Sprite)) as Sprite;
			SpriteRenderer spriteRendererGlow = transform.Find("TowerSpriteGlow").GetComponent<SpriteRenderer>();
			spriteRendererGlow.sprite = Resources.Load(resourceName + "_" + towerLevel + "-glow", typeof(Sprite)) as Sprite;
			map.NotifyUpgradeTowerObservers(localizableOnMap.cell);
            upgradeCost += upgradeCost / 2f;
		}
		OnDeselect();
	}

	public void SellTower(){
		Debug.Log("Tower sold for " + (int)(towerCost / 2f));
		map.NotifySellTowerObservers(localizableOnMap.cell);
		playerMoney.Money += (int)(towerCost / 2f);
		Destroy(gameObject);
		Destroy(currentTowerRangeObject);
		Cell cell = map.GetCellAtPos(transform.position.x, transform.position.y);
		cell.isObstacle = false;
		clickReceptor.RemoveOnClickListener(OnDeselect);
		PathFinder pathFinder = PathFinder.instance;
		pathFinder.requestNewGlobalPath(map.GetCellAt(map.xStart,map.yStart), map.GetCellAt(map.xGoal, map.yGoal));
		pathFinder.RecalculatePathForCurrentEnemies();
	}

	public void OnDeselect(){
		upgradeButtonObject.SetActive(false);
		sellButtonObject.SetActive(false);
		upgradeButtonBackground.SetActive(false);
		sellButtonBackground.SetActive(false);
        dpsLabel.SetActive(false);
		HideTowerRange();
		isContextualMenuOpen = false;
	}
}
