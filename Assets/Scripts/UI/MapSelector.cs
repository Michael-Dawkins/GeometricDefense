using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour {

    public ScrollRect mapSelectorScrollRect;
    public RectTransform buttonPrefab;
    bool isMenuDisplayed = false;

    GameObject mapListPanel;

	void Start () {
        mapListPanel = mapSelectorScrollRect.transform.Find("MapListPanel").gameObject;
        TextAsset mapListTextAsset = (TextAsset)Resources.Load("map_list", typeof(TextAsset));
        JSONObject mapListJson = new JSONObject(mapListTextAsset.text);
        ConstructButtonsForJson(mapListJson);
        ClickReceptor.instance.AddOnClickListener(OnClickReceptorClick);
	}

    void OnClickReceptorClick() {
        if (isMenuDisplayed) {
            HideMapSelector();
        }
    }

    public void ShowMapSelector() {
        mapSelectorScrollRect.gameObject.SetActive(true);
        isMenuDisplayed = true;
    }

    public void HideMapSelector() {
        mapSelectorScrollRect.gameObject.SetActive(false);
        isMenuDisplayed = false;
    }

    void ConstructButtonsForJson(JSONObject mapListJson) {
        JSONObject mapList = mapListJson.list[mapListJson.keys.IndexOf("maps")];
        RectTransform lastButton = null;
        foreach (JSONObject mapObj in mapList.list) {
            string mapName = mapObj.list[mapObj.keys.IndexOf("label")].str;
            string mapResourceName = mapObj.list[mapObj.keys.IndexOf("resourceName")].str;
            RectTransform buttonTrans = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as RectTransform;
            buttonTrans.localScale = Vector3.one;
            buttonTrans.SetParent(mapListPanel.transform, false);
            if (lastButton != null){
                buttonTrans.anchoredPosition = new Vector2(0, (lastButton.anchoredPosition.y - buttonTrans.rect.height));
            } else {
                buttonTrans.anchoredPosition = new Vector2(0, -(buttonTrans.rect.height / 2f));
            }
            Text buttonText = buttonTrans.FindChild("Text").gameObject.GetComponent<Text>();
            Button button = buttonTrans.GetComponent<Button>();
            button.onClick.AddListener(() => LoadMap(mapResourceName));
            buttonText.text = mapName;
            lastButton = buttonTrans;
        }
    }

    void LoadMap(string mapResourceName) {
        HideMapSelector();
        MapLoader.instance.LoadMap(mapResourceName);
    }
}