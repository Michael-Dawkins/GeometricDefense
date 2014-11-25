using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IonCharger : MonoBehaviour {
	BoxCollider2D boxCollider2D;
	float chargeLevel;
	float maxCharge = 500f;
	GameObject chargeBarObj;
	float width;
	float parentSize;
	bool mouseDown = false;
	Vector2 bottomLeft;
	float lastMouseDownTime;
	float longPressDelay = 0.15f;
	GameObject chargingSpriteObj;
	float timeToShrink = 1f;

	void Start () {
		width = Map.instance.cellSize;

		//instantiate neon line
		chargeBarObj = Instantiate(Resources.Load("NeonLine")) as GameObject;
		chargeBarObj.transform.parent = transform;
		chargeBarObj.transform.localPosition = new Vector3(-(width/2f),-(width/2f), 0);
		chargeBarObj.transform.localScale = new Vector3(chargeBarObj.transform.localScale.x, 2f, 0);
		UpdateChargeBarWidth();
		parentSize = transform.localScale.x;
		chargeBarObj.GetComponent<SpriteRenderer>().color = Color.white;
		chargeBarObj.transform.Find("Glow").GetComponent<SpriteRenderer>().color = Color.white;

		//storing position for quick check during Update
		bottomLeft = new Vector2(chargeBarObj.transform.position.x,chargeBarObj.transform.position.y);
		UIState.AddTowerSelectionListener(StopShrinking);
		TowerButton towerButton =  transform.Find("TowerButtonCanvas/TowerButton")
										.gameObject.GetComponent<TowerButton>();
		towerButton.OnMouseDown += new TowerButton.OnMouseDownHandler(StartShrinking);
		towerButton.OnMouseUp += new TowerButton.OnMouseUpHandler(StopShrinking);
	}

	void Update(){
		if (mouseDown){
			if (lastMouseDownTime + longPressDelay < Time.time){
				ShrinkChargingSpriteDown();
				if (lastMouseDownTime + longPressDelay + timeToShrink < Time.time){
					StopShrinking();
				}
			}
		}
	}

	void StartShrinking(){
		mouseDown = true;
		lastMouseDownTime = Time.time;
	}

	void StopShrinking(){
		mouseDown = false;
		Destroy(chargingSpriteObj);
		chargingSpriteObj = null;
	}

	bool IsPosInsideIonCharger(Vector2 mousePos){
		return (mousePos.x > bottomLeft.x && mousePos.x < bottomLeft.x + width
		        && mousePos.y > bottomLeft.y && mousePos.y < bottomLeft.y + width);
	}

	void ShrinkChargingSpriteDown(){
		if (chargingSpriteObj == null){
			chargingSpriteObj = Instantiate(Resources.Load("TowerRangeCircle")) as GameObject;
			chargingSpriteObj.transform.parent = transform;
			chargingSpriteObj.transform.localPosition = Vector3.zero;
			chargingSpriteObj.transform.localScale = Vector3.one;
		}
		float currentSize = chargingSpriteObj.transform.localScale.x;
		float newSize = Mathf.Lerp(currentSize, 0, Time.deltaTime * (1f / timeToShrink));
		chargingSpriteObj.transform.localScale = new Vector3(newSize, newSize, 0);
	}

	public void Charge(float damage){
		chargeLevel += damage / 10f;;
		UpdateChargeBarWidth();
		if (chargeLevel > maxCharge){
			chargeLevel = maxCharge;
			DisplayAsFullyCharged();
		}
	}

	void UpdateChargeBarWidth(){
		float worldUnits = (chargeLevel/maxCharge) * width * parentSize;
		if (worldUnits < 0.02f){
			worldUnits = 0.02f;
		}
		GDUtils.ScaleTransformToXWorldUnitHorinzontally(chargeBarObj.transform, worldUnits);
	}

	void DisplayAsFullyCharged() {
		
	}
}
