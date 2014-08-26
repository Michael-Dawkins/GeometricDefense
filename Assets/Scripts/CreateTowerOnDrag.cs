using UnityEngine;
using System.Collections;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;
	public int towerCost = 50;
	public GameObject ghost;

	private bool dragging = false;
	private PlayerMoney playerMoney;
	private Vector3 mousePosition;
	private Vector3 ghostPosition;
	private GameObject currentGhost;

	// Use this for initialization
	void Start () {
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
		GDUtils.PlaceTransformOnScreen(transform, 150, 40);

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
			Map.GetCellAtPos(tmpPos.x, tmpPos.y).isObstacle = true;
			PathFinder pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
			//pathFinder.FindPath(Map.CellAt(0,Map.mapHeight / 2), Map.CellAt(Map.mapWidth -1, Map.mapHeight / 2));
			pathFinder.FindPath(Map.CellAt(0,0), Map.CellAt(7, 7));

			tmpPos.x = tmpPos.x - tmpPos.x % Map.cellSize;
			tmpPos.y = tmpPos.y - tmpPos.y % Map.cellSize;
			tmpPos.z = 0;
			lastTowerCreated.transform.position = tmpPos;
		} else {
			Destroy(lastTowerCreated);
		}
	}

	void DrawGhostAtClosestInputPos(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		Vector3 closestPos = new Vector3(mousePos.x - mousePos.x % Map.cellSize,
		                                 mousePos.y - mousePos.y % Map.cellSize,
		                                 0);
		if (currentGhost == null){
			currentGhost = Instantiate(ghost, closestPos, Quaternion.identity) as GameObject;
			SpriteRenderer renderer = currentGhost.GetComponent<SpriteRenderer>();
			renderer.color = new Color(1f,1f,1f,0.5f);
		}
		currentGhost.transform.position = closestPos;
	}

	bool HasEnoughMoneyToBuyTower(){
		return playerMoney.Money >= towerCost;
	}

	void BuyTower(){
		playerMoney.Money -= towerCost;
	}
}
