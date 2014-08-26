using UnityEngine;
using System.Collections;

public class PlayerMoney : MonoBehaviour {

	private int money = 1000;
	public int Money {
		get {
			return money;
		}
		set {
			money = value;
			setMoneyLabel(money);
		}
	}

	private GUIText moneyGUIText;
	// Use this for initialization
	void Start () {
		GameObject moneyLabel = new GameObject();
		moneyLabel.AddComponent<GUIText>();
		moneyLabel.transform.position = new Vector3(0.9f,0.9f,0.0f);
		moneyGUIText = moneyLabel.guiText;
		setMoneyLabel(money);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void setMoneyLabel(int amount){
		moneyGUIText.text = "gold: " + money.ToString();
	}
}
