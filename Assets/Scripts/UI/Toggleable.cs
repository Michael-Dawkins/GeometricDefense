using UnityEngine;
using System.Collections;

public class Toggleable : MonoBehaviour {

    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
