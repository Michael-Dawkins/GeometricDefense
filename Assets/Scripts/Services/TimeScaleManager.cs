using UnityEngine;
using System.Collections;

public class TimeScaleManager : MonoBehaviour {

	public static TimeScaleManager instance;
	public delegate void OnTimeScaleChangeCallback();
	public event OnTimeScaleChangeCallback TimeScaleChange;
    public bool isPlaying;
    public bool IsPaused {
        get { return !isPlaying; }
    }
	public enum TimeScale{
		ONE, TWO, THREE
	}
	public TimeScale timeScale;
	void Awake(){
		instance = this;
		timeScale = TimeScale.ONE;
        Time.timeScale = 0f;
        isPlaying = false;
	}
	
	public void SelectNextTimeScale(){
		switch(timeScale){
		    case TimeScale.ONE:
			    timeScale = TimeScale.TWO;
                if (isPlaying) {
                    Time.timeScale = 2f;
                }
			    if (TimeScaleChange != null){
				    TimeScaleChange();
			    }
			    break;
		    case TimeScale.TWO:
			    timeScale = TimeScale.THREE;
                if (isPlaying) {
                    Time.timeScale = 3f;
                }
			    if (TimeScaleChange != null){
				    TimeScaleChange();
			    }
			    break;
		    case TimeScale.THREE:
			    timeScale = TimeScale.ONE;
                if (isPlaying) {
                    Time.timeScale = 1f;
                }
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
