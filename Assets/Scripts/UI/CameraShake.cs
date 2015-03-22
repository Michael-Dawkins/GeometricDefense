using UnityEngine;
using System.Collections;

/**
 * Camera shake works in two phases :
 * First it moves the camera's position with PerlinNoise,
 * Then it stabilizes : it smoothly comes back to its original position
 **/
public class CameraShake : MonoBehaviour {

    bool animStarted;
    float animEndTime;
    float startingPerlinNoiseOffset;
    float perlinNoiseScale;
    Vector3 initialCameraPosition;
    Vector3 posBeforeStabilization;
    Transform trans;
    float stabilizationDuration;
    bool stabilizing;

	void Start () {
        animStarted = false;
        trans = transform;
        perlinNoiseScale = 4f;
	}
	
	void Update () {
        if (animStarted) {
            if (Time.time > animEndTime + stabilizationDuration) {
                //anim finished
                animStarted = false;
                stabilizing = false;
                startingPerlinNoiseOffset = 0;
                trans.position = initialCameraPosition;
            } else if (Time.time > animEndTime) {
                //anim in stabilization state
                if (!stabilizing) {
                    stabilizing = true;
                    posBeforeStabilization = trans.position;
                }
                float percentage = (Time.time - animEndTime) / stabilizationDuration;
                trans.position = new Vector3(
                    Mathf.Lerp(posBeforeStabilization.x, initialCameraPosition.x, percentage),
                    initialCameraPosition.y,
                    initialCameraPosition.z
                    );
            } else {
                float perlinValue = Mathf.PerlinNoise(0f, Time.time * perlinNoiseScale);
                if (startingPerlinNoiseOffset == 0) {
                    //perlin noise has to start animation at zero, 
                    //so when anim start,  we calculate the first perlin noise
                    //SO that this value is substracted to avoid jumps in the animation
                    startingPerlinNoiseOffset = perlinValue;
                }
                //anim in perlin noise state (natural shake)
                trans.position = new Vector3(
                    initialCameraPosition.x + perlinValue - startingPerlinNoiseOffset,
                    initialCameraPosition.y,
                    initialCameraPosition.z);
            }
        }
	}

    public void Shake(float duration) {
        if (animStarted){
            return;
        }
        stabilizationDuration = duration / 3f;
        animStarted = true;
        animEndTime = Time.time + duration;
        initialCameraPosition = trans.position;
    }
}
