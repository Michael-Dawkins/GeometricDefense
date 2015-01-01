using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class Tile : MonoBehaviour {

	public float timeToFadeOut;

	SpriteRenderer spriteRenderer;
	bool animationStarted = false;
	Color targetColor = new Color(1f,1f,1f,0.15f);

	//pulsation
	float pulsateTimer;
	bool isPulsating = false;
	float pulsationDuration;
	float pulsationRepetitions;
	Color pulsationColor;
	float pulsationStartTime;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = targetColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (animationStarted){
			if(!isPulsating){
				//This type of lerp is smooth, it never actually gets to the destination
				spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * (1f / timeToFadeOut));
				if (spriteRenderer.color.a == 0f){//This is probably dead code
					animationStarted = false;
				}
			} else {
				//modulo enable the pulusing, the repeating, because timer will start over when higher than 0
				pulsateTimer = Mathf.Clamp((pulsateTimer + Time.deltaTime / pulsationDuration) % 1f,0f, 1f);
				spriteRenderer.color = Color.Lerp(pulsationColor, targetColor, pulsateTimer);

				if (pulsationStartTime + pulsationDuration * pulsationRepetitions < Time.time){
					animationStarted = false;
					isPulsating = false;
					spriteRenderer.color = targetColor;
				}
			}
		}
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		animationStarted = true;
		spriteRenderer.color = Color.white;
	}

	public void Pulsate(Color color, float numberOfTimes, float durationOfOnePulsation){
		pulsateTimer = 0f;
		animationStarted = true;
		pulsationColor = color;
		isPulsating = true;
		pulsationDuration = durationOfOnePulsation;
		pulsationRepetitions = numberOfTimes;
		pulsationStartTime = Time.time;
	}
}
