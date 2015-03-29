using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (Image))]
public class PlayPause : MonoBehaviour {

    public Sprite playSprite;
    public Sprite pauseSprite;
    private Image image;

	void Start () {
        image = GetComponent<Image>();
	}

    public void TogglePlayPause() {
        TimeScaleManager.instance.isPlaying = !TimeScaleManager.instance.isPlaying;
        if (TimeScaleManager.instance.isPlaying) {
            switch(TimeScaleManager.instance.timeScale){
                case TimeScaleManager.TimeScale.ONE:
                    Time.timeScale = 1f;
                    break;
                case TimeScaleManager.TimeScale.TWO:
                    Time.timeScale = 2f;
                    break;
                case TimeScaleManager.TimeScale.THREE:
                    Time.timeScale = 3f;
                    break;
            }
            image.sprite = pauseSprite;
        } else {
            Time.timeScale = 0f;
            image.sprite = playSprite;
        }
    }
}
