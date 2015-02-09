using UnityEngine;
using System.Collections;

public class DeactivateOnClickOutside : MonoBehaviour {

	void Start () {
        ClickReceptor.instance.AddOnClickListener(SetInactive);
	}

    void SetInactive() {
        gameObject.SetActive(false);
    }

    void OnDestroy() {
        ClickReceptor.instance.RemoveOnClickListener(SetInactive);
    }
	
}
