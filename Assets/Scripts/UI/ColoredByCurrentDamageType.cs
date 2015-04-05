using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Can color a sprite or a text based upon current damage type*
//It switches color automatically when damage type changes
//SO that the interface matches the damage type, giving the playing a visual cue 
//of what is currently in use
public class ColoredByCurrentDamageType : MonoBehaviour {

    Text text;
    SpriteRenderer spriteRenderer;
    Image image;
    Colorable colorableType;
    enum Colorable {
        Text, Sprite, Image
    }

	void Start () {
        DetectColorableType();
        UpdateColor();
        DamageTypeManager.instance.AddDamageTypeSelectionChangeListener(UpdateColor);
	}

    void UpdateColor() {
        Color damageColor = DamageTypeManager.GetDamageTypeColor(
            DamageTypeManager.instance.currentDamageType);

        float h, s, v;
		GDUtils.ColorToHSV(damageColor, out h, out s, out v);
		Color lightenDamageColor = GDUtils.ColorFromHSV(h,0.25f,v);

        switch (colorableType){
            case Colorable.Sprite:
                spriteRenderer.color = lightenDamageColor;
                break;
            case Colorable.Text:
                text.color = lightenDamageColor;
                break;
            case Colorable.Image:
                image.color = lightenDamageColor;
                break;
        }
    }

    void DetectColorableType() {
        text = GetComponent<Text>();
        if (text != null) {
            colorableType = Colorable.Text;
        } else {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) {
                colorableType = Colorable.Sprite;
            } else {
                image = GetComponent<Image>();
                if (image != null) {
                    colorableType = Colorable.Image;
                } else {
                    Debug.LogError("Must have a Text or Sprite to be colored dynamically");
                }
            }
        }
    }
	
}
