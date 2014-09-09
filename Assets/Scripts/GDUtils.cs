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
}
