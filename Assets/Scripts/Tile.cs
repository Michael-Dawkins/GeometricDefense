using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class Tile : MonoBehaviour {

	public float timeToFadeOut;
	SpriteRenderer spriteRenderer;
	bool animationStarted = false;
	Color targetColor = new Color(1f,1f,1f,0.2f);

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = targetColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (animationStarted){
			spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * (1f / timeToFadeOut));
			if (spriteRenderer.color.a == 0f){
				animationStarted = false;
			}
		}
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		animationStarted = true;
		spriteRenderer.color = Color.white;
	}
}
