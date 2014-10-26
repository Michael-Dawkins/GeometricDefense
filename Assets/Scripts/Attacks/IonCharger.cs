using UnityEngine;
using System.Collections;

public class IonCharger : MonoBehaviour {
	BoxCollider2D boxCollider2D;
	float chargeLevel;
	float maxCharge = 500f;
	GameObject chargeBarObj;
	float width;
	float parentSize;

	void Start () {
		width = Map.instance.cellSize;
		//Add a child holding a box collider to detect user long press
		GameObject childObj = new GameObject("IonChargerCollider");
		childObj.transform.parent = transform;
		childObj.transform.localPosition = Vector3.zero;
		boxCollider2D = childObj.AddComponent<BoxCollider2D>();
		boxCollider2D.size = new Vector2(width,width);
		//instantiate neon line
		chargeBarObj = Instantiate(Resources.Load("NeonLine")) as GameObject;
		chargeBarObj.transform.parent = transform;
		chargeBarObj.transform.localPosition = new Vector3(-(width/2f),-(width/2f), 0);
		chargeBarObj.transform.localScale = new Vector3(chargeBarObj.transform.localScale.x, 2f, 0);
		UpdateChargeBarWidth();
		parentSize = transform.localScale.x;
		chargeBarObj.GetComponent<SpriteRenderer>().color = Color.white;
		chargeBarObj.transform.Find("Glow").GetComponent<SpriteRenderer>().color = Color.white;
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
