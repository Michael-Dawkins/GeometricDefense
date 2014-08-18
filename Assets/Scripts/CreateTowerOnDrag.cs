using UnityEngine;
using System.Collections;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;

	private bool dragging = false;

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		PlaceObjectOnScreen(new Vector3 (150, 40, 0));

		if (Input.touchCount == 1 || Input.GetMouseButtonDown(0) && !dragging)
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 touchPos = new Vector2(wp.x, wp.y);

			if (collider2D == Physics2D.OverlapPoint(touchPos)) {
				CreateTower();
				dragging = true;
			}
		} else if (Input.touchCount == 1 || Input.GetMouseButton(0) && dragging){
			DragTower();
		} else if (Input.touchCount == 0 && !Input.GetMouseButton(0) && dragging){
			dragging = false;
		}
	}

	void PlaceObjectOnScreen(Vector3 screenPoint){
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
		worldPos.z = 0;
		transform.position = worldPos;
	}

	void CreateTower(){
		lastTowerCreated = Instantiate(towerToCreate, transform.position, transform.rotation) as GameObject;
	}
	
	void DragTower(){
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		lastTowerCreated.transform.position = mousePosition;
	}

}
