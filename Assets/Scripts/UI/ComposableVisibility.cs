using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComposableVisibility : MonoBehaviour {

    private Dictionary<string, bool> visibilityKeys = new Dictionary<string, bool>();
    
    public void setVisibilityKey(string key, bool visible) {
        visibilityKeys[key] = visible;
        UpdateVisibility();
    }

    private void UpdateVisibility() {
        foreach (KeyValuePair<string, bool> entry in visibilityKeys) {
            if (entry.Value == false) {
                gameObject.SetActive(false);
                return;
            }
        }
        gameObject.SetActive(true);
    }
}
