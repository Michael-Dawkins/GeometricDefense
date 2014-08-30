using UnityEngine;
using System.Collections;

public class PlayerMoney : MonoBehaviour {

	public int money = 1000;
	public Font font;
	public Material fontMaterial;
	public int Money {
		get {
			return money;
		}
		set {
			money = value;
			SetMoneyLabel(money);
		}
	}

	private TextMesh moneyTextMesh;

	void Start () {
		GameObject moneyLabel = new GameObject("money label");
		moneyLabel.AddComponent<TextMesh>();
		GDUtils.PlaceTransformOnViewport(moneyLabel.transform, 0.8f,0.08f);
		moneyTextMesh = moneyLabel.GetComponent<TextMesh>();
		moneyLabel.GetComponent<TextMesh>().renderer.material = fontMaterial;
		moneyTextMesh.font = font;
		SetMoneyLabel(money);
		GDUtils.ScaleTextMeshToMatchXWorldUnit(moneyLabel.transform, 0.8f);
	}

	void SetMoneyLabel(int amount){
		moneyTextMesh.text = "gold: " + money.ToString();
	}
}
