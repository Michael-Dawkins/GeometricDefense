using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ExtensionMethods {

    public static void SetAlpha(this SpriteRenderer renderer, float value) {
        Color color = renderer.color;
        color.a = value;
        renderer.color = color;
    }

    public static void SetAlpha (this Material material, float value) { 
		Color color = material.color; 
		color.a = value; 
		material.color = color; 
	}

	public static void SetAlpha (this Image image, float value) { 
		Color color = image.color; 
		color.a = value; 
		image.color = color; 
	}

	public static void SetAlpha (this Text text, float value) { 
		Color color = text.color; 
		color.a = value; 
		text.color = color; 
	}
	
}