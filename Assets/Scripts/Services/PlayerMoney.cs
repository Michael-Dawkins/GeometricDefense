using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMoney : MonoBehaviour {

	public static PlayerMoney instance;

	public Text goldLabel;
	public int money = 1000;
	public int Money {
		get {
			return money;
		}
		set {
			if ((value - money)<0){
				DisplayMoneyDelta(value - money);
			}
			money = value;
			UpdateMoneyLabel();
			foreach(OnMoneyChange callback in callbacks){
				callback(value);
			}
		}
	}
	public delegate void OnMoneyChange(float amount);
	List<OnMoneyChange> callbacks = new List<OnMoneyChange>();
	
	void Awake(){
		instance = this;
	}

	void Start () {
		UpdateMoneyLabel();
	}

	void UpdateMoneyLabel(){
		goldLabel.text = "gold: " + money.ToString();
	}

	public void AddOnMoneyChangeListener(OnMoneyChange callback){
		callbacks.Add(callback);
	}

	public void RemoveOnMoneyChangeListener(OnMoneyChange callback){
		callbacks.Remove(callback);
	}

	void DisplayMoneyDelta(float delta){
		GameObject deltaObj = new GameObject("Delta");
		Text deltaText = deltaObj.AddComponent<Text>();
		deltaText.text = delta.ToString();
		deltaText.font = goldLabel.font;
		deltaText.fontSize = goldLabel.fontSize - 6;
		deltaObj.transform.parent = goldLabel.transform;
		deltaObj.transform.localPosition = new Vector3(40f,10f,0);
		deltaObj.transform.localScale = Vector3.one;
		DisappearingText disappearingText = deltaObj.AddComponent<DisappearingText>();
		disappearingText.timeToFadeOut = 0.2f;
		disappearingText.timeBeforeFadeOut = 0.5f;
	}
}
