using System;
using UnityEngine;

public class GDUtils {

	public GDUtils () {
	}

	public enum ScreenCorner{
		TOP_LEFT, TOP_RIGHT, BOTTOM_RIGHT, BOTTOM_LEFT
	}

	//Coordinates are in piwels, starting from the bottom left of the screen
	public static void PlaceTransformOnScreen(Transform trans, int x, int y){
		Vector3 screenPoint = new Vector3(x, y, 0);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
		worldPos.z = 0;
		trans.position = worldPos;
	}

	public static void PlaceTransformAtScreenCorner(Transform trans, ScreenCorner corner, int xMargin, int yMargin){
		int width = Screen.width, height = Screen.height;

		switch (corner) {
			case ScreenCorner.TOP_LEFT:
				PlaceTransformOnScreen(trans, xMargin, height - yMargin);
				break;
			case ScreenCorner.TOP_RIGHT:
				PlaceTransformOnScreen(trans, width - xMargin, height - yMargin);
				break;
			case ScreenCorner.BOTTOM_RIGHT:
				PlaceTransformOnScreen(trans, width - xMargin, yMargin);
				break;
			case ScreenCorner.BOTTOM_LEFT:
				PlaceTransformOnScreen(trans, xMargin, yMargin);
				break;
		}
	}

	public static void PlaceTransformOnViewport(Transform trans, float xMargin, float yMargin){
		Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(xMargin, yMargin, 0));
		worldPos.z = 0;
		trans.position = worldPos;
	}

	public static void ScaleTransformToXWorldUnit(Transform trans, float xScale){
		Bounds bounds = trans.gameObject.renderer.bounds;
		float ratio = xScale / bounds.size.x;
		trans.localScale = new Vector3(trans.localScale.x * ratio, trans.localScale.y * ratio, 0);
	}

	public static void ScaleTransformToXWorldUnitHorinzontally(Transform trans, float xScale){
		Bounds bounds = trans.gameObject.renderer.bounds;
		float ratio = xScale / bounds.size.x;
		trans.localScale = new Vector3(trans.localScale.x * ratio, trans.localScale.y, 0);
	}
	
	public static float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n){
		// angle in [0,180]
		float angle = Vector3.Angle(a,b);
		float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));
		
		// angle in [-179,180]
		float signed_angle = angle * sign;
		
		// angle in [0,360] (not used but included here for completeness)
		//float angle360 =  (signed_angle + 180) % 360;
		
		return signed_angle;
	}

	public static Color ColorFromHSV(float h, float s, float v, float a = 1)
	{
		// no saturation, we can return the value across the board (grayscale)
		if (s == 0)
			return new Color(v, v, v, a);
		
		// which chunk of the rainbow are we in?
		float sector = h / 60;
		
		// split across the decimal (ie 3.87 into 3 and 0.87)
		int i = (int)sector;
		float f = sector - i;
		
		float p = v * (1 - s);
		float q = v * (1 - s * f);
		float t = v * (1 - s * (1 - f));
		
		// build our rgb color
		Color color = new Color(0, 0, 0, a);
		
		switch(i)
		{
		case 0:
			color.r = v;
			color.g = t;
			color.b = p;
			break;
			
		case 1:
			color.r = q;
			color.g = v;
			color.b = p;
			break;
			
		case 2:
			color.r  = p;
			color.g  = v;
			color.b  = t;
			break;
			
		case 3:
			color.r  = p;
			color.g  = q;
			color.b  = v;
			break;
			
		case 4:
			color.r  = t;
			color.g  = p;
			color.b  = v;
			break;
			
		default:
			color.r  = v;
			color.g  = p;
			color.b  = q;
			break;
		}

		return color;
	}
	
	public static void ColorToHSV(Color color, out float h, out float s, out float v)
	{
		float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
		float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
		float delta = max - min;
		
		// value is our max color
		v = max;
		
		// saturation is percent of max
		if (!Mathf.Approximately(max, 0))
			s = delta / max;
		else
		{
			// all colors are zero, no saturation and hue is undefined
			s = 0;
			h = -1;
			return;
		}
		
		// grayscale image if min and max are the same
		if (Mathf.Approximately(min, max))
		{
			v = max;
			s = 0;
			h = -1;
			return;
		}
		
		// hue depends which color is max (this creates a rainbow effect)
		if (color.r == max)
			h = (color.g - color.b) / delta;            // between yellow & magenta
		else if (color.g == max)
			h = 2 + (color.b - color.r) / delta;                // between cyan & yellow
		else
			h = 4 + (color.r - color.g) / delta;                // between magenta & cyan
		
		// turn hue into 0-360 degrees
		h *= 60;
		if (h < 0 )
			h += 360;
	}
}
