using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;
	public int towerCost = 50;
	public GameObject ghost;
	public bool applyButtonColorToTowers = true;
	public float xViewportPos = 0.1f;
	public float yViewportPos = 0.1f;

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
		GDUtils.PlaceTransformOnViewport(transform, xViewportPos, yViewportPos);

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
			Vector3 tmpPos = lastTowerCreated.transform.position;
			Cell cellAtPos = map.GetCellAtPos(tmpPos.x, tmpPos.y) ;
			if (cellAtPos == null || cellAtPos.isObstacle){
				Destroy(lastTowerCreated);
				return;
			}
			PathFinder pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
			cellAtPos.isObstacle = true;

			if (pathFinder.requestNewGlobalPath(map.CellAt(map.xStart,map.yStart), map.CellAt(map.xGoal, map.yGoal))){
				BuyTower();
				RecalculatePathForCurrentEnemies();
				
				tmpPos.x = tmpPos.x - tmpPos.x % map.cellSize;
				tmpPos.y = tmpPos.y - tmpPos.y % map.cellSize;
				tmpPos.z = 0;
				lastTowerCreated.transform.position = tmpPos;
			} else {
				//TODO This seems to mess up future path findings, maybe because of messed up score ?
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
