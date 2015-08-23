using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class PulsingText : MonoBehaviour {

    public float Speed;

    Text text;

    void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
        text.SetAlpha(Mathf.Abs(Mathf.Cos(Time.realtimeSinceStartup * Speed)));
    }
}
