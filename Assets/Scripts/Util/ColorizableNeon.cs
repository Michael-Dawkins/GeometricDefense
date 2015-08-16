using UnityEngine;
using System.Collections;

public class ColorizableNeon : MonoBehaviour {

	[SerializeField][HideInInspector]
	Color aColor;
	public SpriteRenderer centerRenderer;
	public SpriteRenderer glowRenderer;
    public Color initialColor;
    bool colorInitialized;

	public Color Color {
		set{
			aColor = value;
			ApplyColor(aColor,0.25f);
		}
		get {
			return aColor;
		}
	}

	void Start () {
        centerRenderer = transform.FindChild("NeonCenter").GetComponent<SpriteRenderer>();
        glowRenderer = transform.FindChild("NeonGlow").GetComponent<SpriteRenderer>();
        ApplyColor(aColor, 0.25f);
	}

	public void ApplyColor(Color colorToApply, float centerSaturation){
        if (!colorInitialized) {
            initialColor = colorToApply;
            colorInitialized = true;
        }

        float h, s, v;
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		colorToApply = GDUtils.ColorFromHSV(h, 1f, v);
		
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		Color lightColor = GDUtils.ColorFromHSV(h, centerSaturation, v);

		centerRenderer.color = lightColor;
		glowRenderer.color = colorToApply;

    }
	
}
