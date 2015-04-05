using UnityEngine;
using System.Collections;

public class ShowWhenPaused : MonoBehaviour {

    void Start() {
        UpdateVisibility();
        PlayPause.instance.AddPauseListener(UpdateVisibility);
    }

    void UpdateVisibility() {
        gameObject.SetActive(Time.timeScale == 0f);
    }

    void OnDestroy() {
        PlayPause.instance.RemovePauseListener(UpdateVisibility);
    }
	
}
