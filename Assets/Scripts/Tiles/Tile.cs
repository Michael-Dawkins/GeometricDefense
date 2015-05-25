using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class Tile : MonoBehaviour {

	public float timeToFadeOut;

	SpriteRenderer spriteRenderer;
	bool animationStarted = false;
	public Color targetColor = new Color(1f,1f,1f,0.15f);
    public bool isAnimatable = true;

    public enum TileType {
        NORMAL, OBSTACLE, DAMAGE_BOOSTER, RANGE_BOOSTER
    }
    public TileType tileType {
        get { return _tileType; }
        set {
            _tileType = value;
            switch (_tileType) {
                case TileType.NORMAL:
                    isAnimatable = true;
                    break;
                case TileType.OBSTACLE:
                case TileType.DAMAGE_BOOSTER:
                case TileType.RANGE_BOOSTER:
                    targetColor = GetTileTypeColor(tileType);
                    if (spriteRenderer != null) {
                        spriteRenderer.color = targetColor;
                    }
                    isAnimatable = false;
                    break;
            }
        }
    }
    TileType _tileType;

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

    public static Color GetTileTypeColor(TileType tileType){
        switch (tileType) {
            case TileType.DAMAGE_BOOSTER:
                return Color.cyan;
            case TileType.NORMAL:
                return Color.white;
            case TileType.OBSTACLE:
                return Color.red;
            case TileType.RANGE_BOOSTER:
                return Color.yellow;
            default:
                Debug.LogError("unknown tile type, cannot get color");
                return Color.white;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAnimatable) {
            return;
        }
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
                    StopPulsating();
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

    public void StopPulsating() {
        animationStarted = false;
        isPulsating = false;
        if (spriteRenderer != null) {
            spriteRenderer.color = targetColor;
        }
    }
}
