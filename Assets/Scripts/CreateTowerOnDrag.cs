using UnityEngine;
using System.Collections;

public class CreateTowerOnDrag : MonoBehaviour {

	public GameObject towerToCreate;
	public GameObject lastTowerCreated;

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPoint = new Vector3 (150, 40, 0);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
		worldPos.z = 0;
		transform.position = worldPos;
	}

	void OnMouseDown(){
		Debug.Log ("Creating tower");
		lastTowerCreated = Instantiate(towerToCreate, transform.position, transform.rotation) as GameObject;
	}

	void OnMouseDrag(){
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		lastTowerCreated.transform.position = mousePosition;
	}

}
