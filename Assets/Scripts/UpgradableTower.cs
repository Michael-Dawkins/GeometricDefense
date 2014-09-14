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

	// Use this for initialization
	void Start() {
		map = GameObject.Find("Map").GetComponent<Map>();
		GameObject playerState = GameObject.Find("PlayerState");
		playerMoney = playerState.GetComponent<PlayerMoney>();
		clickableObject = new GameObject ("ClickableZone");
		clickableObject.transform.parent = transform;
		clickableObject.transform.localPosition = Vector3.zero;
		clickableCollider = clickableObject.AddComponent<BoxCollider2D>();
		clickableCollider.size = new Vector2(map.cellSize,map.cellSize);
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2 (wp.x, wp.y);
			if(clickableCollider == Physics2D.OverlapPoint(touchPos)) {
				Debug.Log("Click on tower");
				DisplayUpgradeButton();
			}
		}
	}

	void DisplayUpgradeButton() {
		upgradeCanvas =  GetComponentsInChildren<Canvas>(true)[0];
		upgradeCanvas.transform.localPosition = new Vector3(-map.cellSize * 1.5f, 0, 0);
		upgradeCanvas.enabled = true;
		upgradeCanvas.gameObject.transform.parent = transform;
		Button button = upgradeCanvas.gameObject.GetComponentInChildren<Button>();
		button.onClick.AddListener(UpGradeTower);
		EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		eventSystem.SetSelectedGameObject(button.gameObject, new BaseEventData(eventSystem));
	}

	void UpGradeTower(){
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
		upgradeCanvas.enabled = false;
	}
}
