using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour {

    public static MapLoader instance;
    Map map;

	void Awake() {
        instance = this;
	}

    void Start() {
        map = Map.instance;
        TextAsset mapTextAsset = (TextAsset)Resources.Load("map_1", typeof(TextAsset));
        JSONObject mapJson = new JSONObject(mapTextAsset.text);
        LoadMap(mapJson);
    }

    void LoadMap(JSONObject mapJson) {
        int cellsIndex = mapJson.keys.IndexOf("cells");
        LoadCells(mapJson.list[cellsIndex]);
        PathFinder.instance.FindGlobalPath(map.GetCellAt(map.xStart, map.yStart), map.GetCellAt(map.xGoal, map.yGoal));
    }

    void LoadCells(JSONObject cellsJson) {
        int wallsIndex = cellsJson.keys.IndexOf("walls");
        LoadWalls(cellsJson.list[wallsIndex]);
    }

    void LoadWalls(JSONObject wallsJson) {
        foreach (JSONObject obj in wallsJson.list) {
            int x = (int) obj.list[0].n;
            int y = (int) obj.list[1].n;
            Cell cell = map.GetCellAt(x, y);
            cell.isObstacle = true;
            cell.tile.targetColor = Color.red;
            cell.tile.isAnimatable = false;
        }
    }
}
