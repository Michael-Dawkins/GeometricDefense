using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

    bool isAnimating;
    ColorizableNeon colorizableNeon;
    float flashDuration = 1f;
    //added to duration, stays at full color for x time before fading
    float fullColorDuration = 0.3f;
    float flashStartTime;
    Color flashColor;

	void Start () {
        colorizableNeon = GetComponent<ColorizableNeon>();
	}
	
	void Update () {
        if (isAnimating) {
            if (flashStartTime + (flashDuration + fullColorDuration) < Time.time) {
                isAnimating = false;
                colorizableNeon.ApplyColor(colorizableNeon.initialColor, 0.25f);
            } else {
                colorizableNeon.ApplyColor(Color.Lerp(
                        flashColor,
                        colorizableNeon.initialColor,
                        GetAnimationProgress()
                    ),
                    //saturation from 0.75 to 0.25
                    0.75f - GetAnimationProgress() / 2f
                );
            }
        }
    }

    //between 0 and 1
    private float GetAnimationProgress() {
        return (Time.time - (flashStartTime - fullColorDuration)) / flashDuration;
    }

    //visual flash to indicate that the enemy has been hit by a projectile
    public void TriggerFlash(Color color) {
        flashColor = color;
        isAnimating = true;
        flashStartTime = Time.time;
    }
    
}
