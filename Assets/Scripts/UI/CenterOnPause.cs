using UnityEngine;
using UnityEngine.UI;

public class CenterOnPause : MonoBehaviour {

    public float transitionTime;
    public CanvasScaler canvasScaler;
    bool centered = true;
    bool isAnimating = false;
    Vector2 initialPosition;
    Vector3 initialScale;
    float animationStartTime;
    RectTransform rectTransform;
    Vector2 centeredPosition;
    float centerScaleFactor;
    float timer;

	void Start () {
        PlayPause.instance.AddPauseListener(CenterIfPaused);
        rectTransform = GetComponent<RectTransform>();
        centeredPosition = GetCenterPosition();
        initialPosition = rectTransform.rect.position;
        initialScale = rectTransform.localScale;
        Debug.Log("Initial position " + initialPosition);
        CenterIfPaused();
    }
	
	void Update () {
        if (!isAnimating)
            return;
        timer = (Time.realtimeSinceStartup - animationStartTime) / transitionTime;
        if (centered) {
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, centeredPosition, timer);
            rectTransform.localScale = Vector3.Lerp(initialScale, initialScale * 2f, timer);
        } else {
            rectTransform.anchoredPosition = Vector2.Lerp(centeredPosition, initialPosition, timer);
            rectTransform.localScale = Vector3.Lerp(initialScale * 2f, initialScale, timer);
        }
        if (Time.realtimeSinceStartup > (animationStartTime + transitionTime))
            isAnimating = false;
	}



    Vector3 GetCenterPosition() {
        return new Vector3(
                initialPosition.x - canvasScaler.referenceResolution.x / 2f,
                initialPosition.y - canvasScaler.referenceResolution.y / 2f);
    }

    void CenterIfPaused() {
        centered = Time.timeScale == 0;
        Debug.Log("Should be centered " + centered);
        isAnimating = true;
        animationStartTime = Time.realtimeSinceStartup;
    }
}
