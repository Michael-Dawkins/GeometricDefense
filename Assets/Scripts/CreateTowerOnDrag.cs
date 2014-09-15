using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;
	public GameObject towerRangeObject;

	//Tower specs
	public int towerCost;
	public float cellRange;
	public GameObject ghost;
	public float damage;
	public float shootingSpeed;
	public bool applyButtonColorToTowers = true;

	public float xViewportPos = 0.1f;
	public float yViewportPos = 0.1f;

	private bool dragging = false;
	private PlayerMoney playerMoney;
	private Vector3 mousePosition;
	private Vector3 ghostPosition;
	private GameObject currentGhost;
	private GameObject currentTowerRangeObject;
	private Map map;

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
		if (playerMoney == null){
			GameObject playerState = GameObject.Find("PlayerState");
			if (playerState == null){
				throw new UnityException("PlayerState cannot be found in scene");
			}
			playerMoney = playerState.GetComponent<PlayerMoney>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		GDUtils.PlaceTransformOnViewport(transform, xViewportPos, yViewportPos);

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);

			if (collider2D == Physics2D.OverlapPoint(touchPos)) {
				dragging = true;
			}
		} else if (Input.GetMouseButton(0) && dragging){
			DrawGhostAtClosestInputPos();
		} else if (Input.GetMouseButtonUp(0) && dragging){
			PlaceTower();
			Destroy(currentGhost);
			Destroy(currentTowerRangeObject);
			dragging = false;
		}
	}

	void CreateTower(){
		lastTowerCreated = Instantiate(towerToCreate) as GameObject;
		lastTowerCreated.transform.position = transform.position;
		lastTowerCreated.transform.rotation = transform.rotation;
		if (applyButtonColorToTowers){
			SpriteRenderer renderer = lastTowerCreated.transform.Find("TowerSprite").gameObject.GetComponent<SpriteRenderer>();
			renderer.color = gameObject.GetComponent<SpriteRenderer>().color;
		}
	}

	void PlaceTower(){
		if (HasEnoughMoneyToBuyTower()){
			CreateTower();
			Vector3 tmpPos = GetClosestPos();
			Cell cellAtPos = map.GetCellAtPos(tmpPos.x, tmpPos.y) ;
			Debug.Log(tmpPos);
			if (cellAtPos == null || cellAtPos.isObstacle || cellAtPos == map.GetStartCell()){
				Destroy(lastTowerCreated);
				return;
			}
			PathFinder pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
			cellAtPos.isObstacle = true;

			if (pathFinder.requestNewGlobalPath(map.GetCellAt(map.xStart,map.yStart), map.GetCellAt(map.xGoal, map.yGoal))){
				BuyTower();
				RecalculatePathForCurrentEnemies();
				tmpPos = map.GetCellPos(cellAtPos);
				lastTowerCreated.transform.position = tmpPos;

				CanShoot canShoot = lastTowerCreated.GetComponent<CanShoot>();
				canShoot.Damage = damage;
				canShoot.shootingSpeed = shootingSpeed;
				canShoot.cellRange = cellRange;
				//Might not be necessary be it could solve a bug where a towerObject disapear on next tower positioning
				lastTowerCreated = null;
			} else {
				cellAtPos.isObstacle = false;
				Debug.Log("Cannot place tower, it is blocking enemies");
				Destroy(lastTowerCreated);
			}
		} else {
			Destroy(lastTowerCreated);
		}
	}

	void RecalculatePathForCurrentEnemies(){
		List<GameObject> rootObjects = new List<GameObject>();
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
		{
			if (obj.transform.parent == null)
			{
				rootObjects.Add(obj);
			}
		}
		float timeBeforePathFinding = Time.realtimeSinceStartup;
		int counter = 0;
		foreach (GameObject obj in rootObjects){
			CanMove enemy;
			if (enemy = obj.GetComponent<CanMove>()){
				enemy.SetOwnPath();
				counter++;
			}
		}
		Debug.Log("Time to find paths for " + counter + " enemies : "
          + ((Time.realtimeSinceStartup - timeBeforePathFinding)*1000).ToString()
          + " ms");

	}

	void DrawGhostAtClosestInputPos(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		if (map.GetCellAt((int)(mousePos.x / map.cellSize), (int)(mousePos.y / map.cellSize)) != null){

			Vector3 closestPos = GetClosestPos();

			if (currentGhost == null){
				currentGhost = Instantiate(ghost, closestPos, Quaternion.identity) as GameObject;
				currentTowerRangeObject = Instantiate(towerRangeObject, closestPos, Quaternion.identity) as GameObject;
				GDUtils.ScaleTransformToXWorldUnit(
					currentTowerRangeObject.transform, (map.cellSize * cellRange + map.cellSize / 2f) * 2f);
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

	Vector3 GetClosestPos(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		Vector3 pos = map.GetCellPos(map.GetCellClosestToPos(mousePos.x, mousePos.y + map.cellSize));
		return pos;
	}

	bool HasEnoughMoneyToBuyTower(){
		return playerMoney.Money >= towerCost;
	}

	void BuyTower(){
		playerMoney.Money -= towerCost;
	}
}
