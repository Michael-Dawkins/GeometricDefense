using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisappearingText : MonoBehaviour {

	public float timeToFadeOut;
	public float timeBeforeFadeOut;
	Color color;
	Text text;
	float initialTime;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		color = text.color;
		initialTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > (initialTime + timeBeforeFadeOut)){
			text.color = new Color(color.r, color.g,color.b, 
			                       Mathf.Lerp(text.color.a, 0f, Time.deltaTime * (1 / timeToFadeOut)));
			if (color.a == 0){
				Destroy(this);
			}
		}
	}
}
