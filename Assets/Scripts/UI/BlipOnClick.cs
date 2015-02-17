using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlipOnClick : MonoBehaviour {

	void Start () {
        GetComponent<Button>().onClick.AddListener(PlaySound);
	}

    void PlaySound() {
        SoundManager.instance.PlaySound(SoundManager.BUTTON_CLICK);
    }
	
}
