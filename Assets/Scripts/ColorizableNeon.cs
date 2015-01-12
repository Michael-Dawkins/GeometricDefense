using UnityEngine;
using System.Collections;

public class ColorizableNeon : MonoBehaviour {

	[SerializeField][HideInInspector]
	Color aColor;
	SpriteRenderer centerRenderer;
	SpriteRenderer glowRenderer;

	public Color Color {
		set{
			aColor = value;
			ApplyColor(aColor);
		}
		get {
			return aColor;
		}
	}

	void Start () {
		ApplyColor(aColor);
	}

	void ApplyColor(Color colorToApply){
		float h, s, v;
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		colorToApply = GDUtils.ColorFromHSV(h, 1f, v);
		
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		Color lightColor = GDUtils.ColorFromHSV(h,0.25f,v);

		centerRenderer = transform.FindChild("NeonCenter").GetComponent<SpriteRenderer>();
		glowRenderer = transform.FindChild("NeonGlow").GetComponent<SpriteRenderer>();
		centerRenderer.color = lightColor;
		glowRenderer.color = colorToApply;
	}
	
}
