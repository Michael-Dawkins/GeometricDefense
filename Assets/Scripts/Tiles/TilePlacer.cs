using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePlacer : MonoBehaviour {

    public static TilePlacer instance;

    private bool _IsPositioningTiles = false;
    public bool IsPositioningTiles {
        get { return _IsPositioningTiles; }
        set {
            _IsPositioningTiles = value;
            if (value == true) {
                foreach(OnStartPositioningTiles callback in StartPositioningCallbacks){
                    callback();
                }
            }
            if (value == false) {
                foreach (OnStopPositioningTiles callback in StopPositioningCallbacks) {
                    callback();
                }
            }
        }
    }

    public delegate void OnStartPositioningTiles();
    public delegate void OnStopPositioningTiles();

    private List<OnStartPositioningTiles> StartPositioningCallbacks = new List<OnStartPositioningTiles>();
    private List<OnStopPositioningTiles> StopPositioningCallbacks = new List<OnStopPositioningTiles>();

    void Awake() {
        instance = this;
    }

    public void TogglePositioningMode() {
        IsPositioningTiles = !IsPositioningTiles;
    }

    public void AddStartPositioningTilesListener(OnStartPositioningTiles callback) {
        StartPositioningCallbacks.Add(callback);
    }

    public void RemoveStartPositioningTilesListener(OnStartPositioningTiles callback) {
        StartPositioningCallbacks.Remove(callback);
    }

    public void AddStopPositioningTilesListener(OnStopPositioningTiles callback) {
        StopPositioningCallbacks.Add(callback);
    }

    public void RemoveStopPositioningTilesListener(OnStopPositioningTiles callback) {
        StopPositioningCallbacks.Remove(callback);
    }
	
}
