using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ComposableVisibility))]
public class ChangeVisibilityWhenPlacingTiles : MonoBehaviour {

    private string VISIBILITY_KEY = "ChangeVisibilityWhenPlacingTiles";
    public bool VisibleWhenPlacingTiles;
    ComposableVisibility composableVisibility;

    void Start () {
        composableVisibility = GetComponent<ComposableVisibility>();
        TilePlacer.instance.AddStartPositioningTilesListener(SetAppropriateVisibility);
        TilePlacer.instance.AddStopPositioningTilesListener(SetAppropriateVisibility);
        SetAppropriateVisibility();
	}

    public void SetAppropriateVisibility() {
        if (VisibleWhenPlacingTiles == true) {
            composableVisibility.setVisibilityKey(VISIBILITY_KEY, TilePlacer.instance.IsPositioningTiles);
        } else {
            composableVisibility.setVisibilityKey(VISIBILITY_KEY, !TilePlacer.instance.IsPositioningTiles);
        }
    }

    void OnDestroy() {
        if (TilePlacer.instance != null) {
            TilePlacer.instance.RemoveStartPositioningTilesListener(SetAppropriateVisibility);
            TilePlacer.instance.RemoveStopPositioningTilesListener(SetAppropriateVisibility);
        }
    }

}
