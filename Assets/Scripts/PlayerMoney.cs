using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMoney : MonoBehaviour {

	public int money = 1000;
	public int Money {
		get {
			return money;
		}
		set {
			money = value;
			UpdateMoneyLabel();
		}
	}

	public Text goldLabel;

	void Start () {
		UpdateMoneyLabel();
	}

	void UpdateMoneyLabel(){
		goldLabel.text = "gold: " + money.ToString();
	}
}
