using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;
	public int towerCost = 50;
	public GameObject ghost;
	public bool applyButtonColorToTowers = true;
	public int xOffset = 100;

	private bool dragging = false;
	private PlayerMoney playerMoney;
	private Vector3 mousePosition;
	private Vector3 ghostPosition;
	private GameObject currentGhost;
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
		GDUtils.PlaceTransformOnScreen(transform, xOffset, 80);

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);

			if (collider2D == Physics2D.OverlapPoint(touchPos)) {
				CreateTower();
				dragging = true;
			}
		} else if (Input.GetMouseButton(0) && dragging){
			DragTower();
			DrawGhostAtClosestInputPos();
		} else if (Input.GetMouseButtonUp(0) && dragging){
			PlaceTower();
			Destroy(currentGhost);
			dragging = false;
		}
	}

	void CreateTower(){
		lastTowerCreated = Instantiate(towerToCreate, transform.position, transform.rotation) as GameObject;
		if (applyButtonColorToTowers){
			SpriteRenderer renderer = lastTowerCreated.GetComponent<SpriteRenderer>();
			renderer.color = gameObject.GetComponent<SpriteRenderer>().color;
		}
	}
	
	void DragTower(){
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		lastTowerCreated.transform.position = mousePosition;
	}

	void PlaceTower(){
		if (HasEnoughMoneyToBuyTower()){
			BuyTower();
			Vector3 tmpPos = lastTowerCreated.transform.position;
			map.GetCellAtPos(tmpPos.x, tmpPos.y).isObstacle = true;
			PathFinder pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
			//pathFinder.FindPath(map.CellAt(0,map.mapHeight / 2), map.CellAt(map.mapWidth -1, map.mapHeight / 2));
			pathFinder.FindGlobalPath(map.CellAt(0,0), map.CellAt(map.xGoal, map.yGoal));
			RecalculatePathForCurrentEnemies();

			tmpPos.x = tmpPos.x - tmpPos.x % map.cellSize;
			tmpPos.y = tmpPos.y - tmpPos.y % map.cellSize;
			tmpPos.z = 0;
			lastTowerCreated.transform.position = tmpPos;
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
		foreach (GameObject obj in rootObjects){
			CanMove enemy;
			if (enemy = obj.GetComponent<CanMove>()){
				enemy.SetOwnPath();
			}
		}

	}

	void DrawGhostAtClosestInputPos(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		if (map.CellAt((int)(mousePos.x / map.cellSize), (int)(mousePos.y / map.cellSize)) != null){

			Vector3 closestPos = new Vector3(mousePos.x - mousePos.x % map.cellSize,
			                                 mousePos.y - mousePos.y % map.cellSize,
			                                 0);
			if (currentGhost == null){
				currentGhost = Instantiate(ghost, closestPos, Quaternion.identity) as GameObject;
				SpriteRenderer renderer = currentGhost.GetComponent<SpriteRenderer>();

				if (applyButtonColorToTowers){
					Color color = gameObject.GetComponent<SpriteRenderer>().color;
					renderer.color = new Color(color.r,color.g,color.b,0.5f);
				} else {
					renderer.color = new Color(1f,1f,1f,0.5f);
				}

			}
			currentGhost.transform.position = closestPos;
		}
	}

	bool HasEnoughMoneyToBuyTower(){
		return playerMoney.Money >= towerCost;
	}

	void BuyTower(){
		playerMoney.Money -= towerCost;
	}
}
