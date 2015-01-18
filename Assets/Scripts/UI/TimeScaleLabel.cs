using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeScaleLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TimeScaleManager.instance.TimeScaleChange += UpdateSpeedLabel;
		UpdateSpeedLabel();
	}

	void UpdateSpeedLabel(){
		GetComponent<Text>().text = TimeScaleManager.instance.GetCurrentTimeScaleLabel() + "X";
	}

}
