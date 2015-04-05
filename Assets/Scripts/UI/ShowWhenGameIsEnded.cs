using UnityEngine;
using System.Collections;

public class ShowWhenGameIsEnded : MonoBehaviour {

	void Start () {
        EndGameMenu.instance.AddEndMenuListener(UpdateVisibility);
	}

    void UpdateVisibility() {
        gameObject.SetActive(EndGameMenu.instance.isShown);
    }

    void OnDestroy() {
        EndGameMenu.instance.RemoveEndMenuListener(UpdateVisibility);
    }
}
