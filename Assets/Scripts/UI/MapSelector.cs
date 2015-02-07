using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour {

    public ScrollRect mapSelectorScrollRect;
    public RectTransform buttonPrefab;

    GameObject mapListPanel;

	void Start () {
        mapListPanel = mapSelectorScrollRect.transform.Find("MapListPanel").gameObject;
        mapSelectorScrollRect.gameObject.SetActive(true);
        TextAsset mapListTextAsset = (TextAsset)Resources.Load("map_list", typeof(TextAsset));
        JSONObject mapListJson = new JSONObject(mapListTextAsset.text);
        ConstructButtonsForJson(mapListJson);
	}

    void ConstructButtonsForJson(JSONObject mapListJson) {
        JSONObject mapList = mapListJson.list[mapListJson.keys.IndexOf("maps")];
        RectTransform lastButton = null;
        foreach (JSONObject mapObj in mapList.list) {
            string mapName = mapObj.list[mapObj.keys.IndexOf("label")].str;
            string mapResourceName = mapObj.list[mapObj.keys.IndexOf("resourceName")].str;
            RectTransform button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as RectTransform;
            button.localScale = Vector3.one;
            button.SetParent(mapListPanel.transform, false);
            if (lastButton != null){
                button.anchoredPosition = new Vector2(0, (lastButton.anchoredPosition.y - button.rect.height));
            } else {
                button.anchoredPosition = new Vector2(0, -(button.rect.height / 2f));
            }
            Text buttonText = button.FindChild("Text").gameObject.GetComponent<Text>();
            buttonText.text = mapName;
            lastButton = button;
        }
    }
	
}
