using UnityEngine;
using System.Collections;

public class ChangeVisibilityWhenPlacingTiles : MonoBehaviour {

    private TilePlacer tilePlacer;

    public bool VisibleWhenPlacingTiles;

	void Start () {
        tilePlacer = TilePlacer.instance;
        tilePlacer.AddStartPositioningTilesListener(SetAppropriateVisibility);
        tilePlacer.AddStopPositioningTilesListener(SetAppropriateVisibility);
        SetAppropriateVisibility();
	}

    void SetAppropriateVisibility() {
        if (VisibleWhenPlacingTiles == true) {
            gameObject.SetActive(tilePlacer.IsPositioningTiles);
        } else {
            gameObject.SetActive(!tilePlacer.IsPositioningTiles);
        }
    }

    void OnDestroy() {
        if (tilePlacer != null) {
            tilePlacer.RemoveStartPositioningTilesListener(SetAppropriateVisibility);
            tilePlacer.RemoveStopPositioningTilesListener(SetAppropriateVisibility);
        }
    }

}
