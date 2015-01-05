using UnityEngine;
using System.Collections;

public class TimeScaleManager : MonoBehaviour {

	public static TimeScaleManager instance;
	public delegate void OnTimeScaleChangeCallback();
	public event OnTimeScaleChangeCallback TimeScaleChange;

	public enum TimeScale{
		ONE, TWO, THREE
	}
	public TimeScale timeScale;
	void Awake(){
		instance = this;
		timeScale = TimeScale.ONE;
	}
	
	public void SelectNextTimeScale(){
		switch(timeScale){
		case TimeScale.ONE:
			timeScale = TimeScale.TWO;
			Time.timeScale = 2f;
			if (TimeScaleChange != null){
				TimeScaleChange();
			}
			break;
		case TimeScale.TWO:
			timeScale = TimeScale.THREE;
			Time.timeScale = 3f;
			if (TimeScaleChange != null){
				TimeScaleChange();
			}
			break;
		case TimeScale.THREE:
			timeScale = TimeScale.ONE;
			Time.timeScale = 1f;
			if (TimeScaleChange != null){
				TimeScaleChange();
			}
			break;
		}
	}

	public string GetCurrentTimeScaleLabel(){
		switch(timeScale){
			case TimeScale.ONE:
				return "1";
			case TimeScale.TWO:
				return "2";
			case TimeScale.THREE:
				return "3";
			default:
				return "error";
		}
	}
}
