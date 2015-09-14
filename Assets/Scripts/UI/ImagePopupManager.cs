using UnityEngine;
using UnityEngine.UI;

public class ImagePopupManager : MonoBehaviour {

    public GameObject popupPanel;
    Image image;

    public static ImagePopupManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        image = popupPanel.GetComponentsInChildren<Image>(true)[1];
        image.gameObject.AddComponent<Button>().onClick.AddListener(HidePopup);
        popupPanel.AddComponent<Button>().onClick.AddListener(HidePopup);
    }

    public void DisplayPopup(Sprite sprite) {
        popupPanel.SetActive(true);
        image.sprite = sprite;
    }

    public void HidePopup() {
        popupPanel.SetActive(false);
    }
	
}
