using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MuteSoundLabel : MonoBehaviour {

    Text text;

	void Start () {
        text = GetComponent<Text>();
        UpdateText(SoundManager.instance.IsSoundEnabled);
        SoundManager.instance.SoundToggle += UpdateText;
	}

    public void UpdateText(bool isSoundEnabled) {
        text.text = isSoundEnabled ? "Mute sounds" : "Enable sounds";
    }
}
