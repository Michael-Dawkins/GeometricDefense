using UnityEngine;
using UnityEngine.UI;

public abstract class GreyOutWhenNoBooster : MonoBehaviour {

    private Color initialColor;
    private Image image;
    private Text text;
    protected PlayerBoosterTiles playerBoostertiles;
    private ColoredByCurrentDamageType coloredByCurrentDamageType;
	protected void Start () {
        playerBoostertiles = PlayerBoosterTiles.instance;
        coloredByCurrentDamageType = GetComponent<ColoredByCurrentDamageType>();
        Image img = GetComponent<Image>();
        if (img != null) {
            image = img;
        }
        else {
            text = GetComponent<Text>();
        }
        SaveInitialColor();
	}

    private void SaveInitialColor(){
        if (image != null) {
            initialColor = image.color;
        } else {
            initialColor = text.color;
        }
    }

    protected void UpdateColor(int amount) {
        if (amount > 0)
            RestoreColor();
        else
            GreyOut();
    }

    private void GreyOut(){
        if (image != null) {
            image.color = Color.gray;
        } else {
            text.color = Color.grey;
        }
    }

    private void RestoreColor(){
        if (image != null) {
            image.color = initialColor;
        } else {
            text.color = coloredByCurrentDamageType.GetDamageTypeColor();
        }
    }
	
}
