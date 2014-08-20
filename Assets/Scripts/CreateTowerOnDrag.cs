using UnityEngine;
using System.Collections;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;
	public float cellSize = 0.4f;
	public int towerCost = 50;

	private bool dragging = false;
	private PlayerMoney playerMoney;
	private Vector3 mousePosition;

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
		} else if (Input.GetMouseButtonUp(0)){
			PlaceTower();
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
		if (hasEnoughMoneyToBuyTower()){
			buyTower();
			Vector3 tmpPos = lastTowerCreated.transform.position;
			tmpPos.x = tmpPos.x - tmpPos.x % cellSize;
			tmpPos.y = tmpPos.y - tmpPos.y % cellSize;
			tmpPos.z = 0;
			lastTowerCreated.transform.position = tmpPos;
		} else {
			Destroy(lastTowerCreated);
		}

	}

	bool hasEnoughMoneyToBuyTower(){
		return playerMoney.Money >= towerCost;
	}

	void buyTower(){
		playerMoney.Money -= towerCost;
	}
}
