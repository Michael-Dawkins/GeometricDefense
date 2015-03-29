using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreatePrefab;
	public GameObject lastTowerCreated;
	public GameObject towerRangePrefab;

	//Tower specs
	public int towerCost;
	public GameObject ghost;
	public bool applyButtonColorToTowers = true;
	public float upgradeCost;
	public TowerTypeManager.TowerType towerType;

	public float xViewportPos = 0.1f;
	public float yViewportPos = 0.1f;

	bool dragging = false;
	PlayerMoney playerMoney;
	DamageTypeManager damageTypeManager;
	Vector3 mousePosition;
	Vector3 ghostPosition;
	GameObject currentGhost;
	GameObject currentTowerRangeObject;
	Map map;
	Text towerCostLabel;
	SpriteRenderer buttonCenterRenderer;
	SpriteRenderer buttonGlowRenderer;

	// Use this for initialization
	void Start () {
		map = Map.instance;
		playerMoney = PlayerMoney.instance;
		damageTypeManager = DamageTypeManager.instance;

		towerCostLabel = transform.GetComponentInChildren<Text>();
		towerCostLabel.text = towerCost.ToString();
		playerMoney.AddOnMoneyChangeListener(UpdateTowerCostLabelStatus);
		buttonCenterRenderer = GetComponent<SpriteRenderer>();
		buttonGlowRenderer = transform.Find("Glow").GetComponent<SpriteRenderer>();
		damageTypeManager.AddDamageTypeSelectionChangeListener(ChangeTowerDamageType);
		damageTypeManager.SelectCurrentDamageType(DamageTypeManager.DamageType.Antimatter);
	}
	
	// Update is called once per frame
	void Update () {
		GDUtils.PlaceTransformOnViewport(transform, xViewportPos, yViewportPos);

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);

			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
				dragging = true;
			}
		} else if (Input.GetMouseButton(0) && dragging){
            if (GetClosestCell() != null) {
                DrawGhostAtClosestInputPos();
            } else if(currentGhost != null){
                DestroyGhost();
            }
			
		} else if (Input.GetMouseButtonUp(0) && dragging){
			PlaceTower();
			DestroyGhost();
			dragging = false;
		}
	}

    private void DestroyGhost(){
        Destroy(currentGhost);
		Destroy(currentTowerRangeObject);
    }

	public void ChangeTowerDamageType(){
		ApplyColorToButton(DamageTypeManager.GetDamageTypeColor(damageTypeManager.currentDamageType));
		UpdateTowerCostLabelStatus(playerMoney.Money);
	}

	public void UpdateTowerCostLabelStatus(float playerMoney){
		if (towerCost > playerMoney){
			towerCostLabel.color = Color.gray;
			buttonCenterRenderer.color = new Color(buttonCenterRenderer.color.r, 
			                                       buttonCenterRenderer.color.g, 
			                                       buttonCenterRenderer.color.b, 
			                                       0.5f);
			buttonGlowRenderer.color = new Color(buttonGlowRenderer.color.r, 
			                                       buttonGlowRenderer.color.g, 
			                                       buttonGlowRenderer.color.b, 
			                                       0.5f);
		} else {
			towerCostLabel.color = Color.white;
			buttonCenterRenderer.color = new Color(buttonCenterRenderer.color.r, 
			                                       buttonCenterRenderer.color.g, 
			                                       buttonCenterRenderer.color.b, 
			                                       1f);
			buttonGlowRenderer.color = new Color(buttonGlowRenderer.color.r, 
			                                       buttonGlowRenderer.color.g, 
			                                       buttonGlowRenderer.color.b, 
			                                       1f);
		}
	}

	void CreateTower(){
		lastTowerCreated = Instantiate(towerToCreatePrefab) as GameObject;
		lastTowerCreated.transform.position = transform.position;
		lastTowerCreated.transform.rotation = transform.rotation;
		if (applyButtonColorToTowers){
			Color colorToApply = gameObject.GetComponent<SpriteRenderer>().color;
			ApplyColorToTower(colorToApply);
		}
		UpgradableTower upgradableTower = lastTowerCreated.GetComponent<UpgradableTower>();
		upgradableTower.towerRangePrefab = towerRangePrefab;
		upgradableTower.upgradeCost = upgradeCost;
		upgradableTower.towerCost = towerCost;
		CanShoot canShoot = lastTowerCreated.GetComponent<CanShoot>();
		canShoot.towerType = towerType;
		canShoot.damageType = damageTypeManager.currentDamageType;
		canShoot.towerColor = buttonGlowRenderer.color;
	}

	void ApplyColorToButton(Color colorToApply){
		float h, s, v;
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		colorToApply = GDUtils.ColorFromHSV(h, 1f, v);
		
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		Color lightColor = GDUtils.ColorFromHSV(h,0.25f,v);
		buttonCenterRenderer.color = lightColor;
		buttonGlowRenderer.color = colorToApply;
	}

	Color RoundColor(Color color){
		return new Color(Mathf.Round(color.r), Mathf.Round(color.g), Mathf.Round(color.b));
	}

	void ApplyColorToTower(Color colorToApply){
		float h, s, v;
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		colorToApply = GDUtils.ColorFromHSV(h, 1f, v);
		
		SpriteRenderer centerRenderer = lastTowerCreated.transform.Find("TowerSpriteCenter").gameObject.GetComponent<SpriteRenderer>();
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		Color lightColor = GDUtils.ColorFromHSV(h,0.25f,v);
		centerRenderer.color = lightColor;
		SpriteRenderer glowRenderer = lastTowerCreated.transform.Find("TowerSpriteGlow").gameObject.GetComponent<SpriteRenderer>();
		glowRenderer.color = colorToApply;
	}

	void PlaceTower(){
		if (HasEnoughMoneyToBuyTower()){
			CreateTower();
            Cell closetCell = GetClosestCell();
            if (closetCell == null) {
                //User input is too far from the map to place a tower
                Destroy(lastTowerCreated);
                return;
            }
            Vector3 tmpPos = map.GetCellPos(closetCell);
            
			Cell cellAtPos = map.GetCellAtPos(tmpPos.x, tmpPos.y) ;
			if (cellAtPos == null || cellAtPos.isObstacle || cellAtPos == map.GetStartCell()){
				Destroy(lastTowerCreated);
				return;
			}
			PathFinder pathFinder = PathFinder.instance;
			cellAtPos.isObstacle = true;

			if (pathFinder.requestNewGlobalPath(map.GetCellAt(map.xStart,map.yStart), map.GetCellAt(map.xGoal, map.yGoal))){
				BuyTower();
				pathFinder.RecalculatePathForCurrentEnemies();
				tmpPos = map.GetCellPos(cellAtPos);
				lastTowerCreated.transform.position = tmpPos;
				LocalizableOnMap localizableOnMap = lastTowerCreated.GetComponent<LocalizableOnMap>();
				localizableOnMap.cell = cellAtPos;
				//Here we set up the two way association cell <--> localizable on map
				//This enables us to find tower from their cells and vice versa
				localizableOnMap.cell.localizableOnMap = localizableOnMap; 
				if (lastTowerCreated.GetComponent<CanShoot>().damageType == DamageTypeManager.DamageType.Plasma){
					lastTowerCreated.AddComponent<PlasmaBooster>();
				}

				//Might not be necessary be it could solve a bug where a towerObject disapear on next tower positioning
				lastTowerCreated = null;
				map.NotifyAddTowerObservers(cellAtPos);
			} else {
				cellAtPos.isObstacle = false;
				Debug.Log("Cannot place tower, it is blocking enemies");
				Destroy(lastTowerCreated);
			}
		} else {
			Destroy(lastTowerCreated);
		}
	}

	void DrawGhostAtClosestInputPos(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		if (map.GetCellAt((int)(mousePos.x / map.cellSize), (int)(mousePos.y / map.cellSize)) != null){

            Vector3 closestPos = map.GetCellPos(GetClosestCell());

			if (currentGhost == null){
				currentGhost = Instantiate(ghost, closestPos, Quaternion.identity) as GameObject;
				currentTowerRangeObject = Instantiate(towerRangePrefab, closestPos, Quaternion.identity) as GameObject;
				float range = Values.instance.Towers[towerType].Levels[1].CellRange;
				GDUtils.ScaleTransformToXWorldUnit(
					currentTowerRangeObject.transform, (map.cellSize * range + map.cellSize / 2f) * 2f);
				SpriteRenderer ghostRenderer = currentGhost.GetComponent<SpriteRenderer>();
				SpriteRenderer towerRangeRenderer = currentTowerRangeObject.GetComponent<SpriteRenderer>();

				if (applyButtonColorToTowers){
					Color color = gameObject.GetComponent<SpriteRenderer>().color;
					ghostRenderer.color = new Color(color.r,color.g,color.b,0.5f);
					towerRangeRenderer.color = new Color(color.r, color.g, color.b);
				} else {
					ghostRenderer.color = new Color(1f,1f,1f,0.5f);
				}

			}
			currentGhost.transform.position = closestPos;
			currentTowerRangeObject.transform.position = closestPos;
		}
	}
    
    Cell GetClosestCell() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return map.GetCellClosestToPos(mousePos.x, mousePos.y + map.cellSize);
    }

	bool HasEnoughMoneyToBuyTower(){
		return playerMoney.Money >= towerCost;
	}

	void BuyTower(){
		playerMoney.Money -= towerCost;
	}
}
