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
        LoadCells(mapJson);
        PathFinder.instance.FindGlobalPath(map.GetCellAt(map.xStart, map.yStart), map.GetCellAt(map.xGoal, map.yGoal));
    }

    public void LoadMap(string mapResourceName) {
        Debug.Log("Loading " + mapResourceName);
        TextAsset mapTextAsset = (TextAsset)Resources.Load(mapResourceName, typeof(TextAsset));
        JSONObject mapJson = new JSONObject(mapTextAsset.text);
        int dimensionsIndex = mapJson.keys.IndexOf("dimensions");
        JSONObject dimensionsJson = mapJson.list[dimensionsIndex];
        int mapWidth = (int)  dimensionsJson.list[dimensionsJson.keys.IndexOf("width")].n;
        int mapHeight = (int)dimensionsJson.list[dimensionsJson.keys.IndexOf("height")].n;
        map.InitEmptyMap(mapWidth, mapHeight);
        TileGenerator.instance.ResetTiles();
        DestroyAllEnemies();
        LoadCells(mapJson);
        Spawner.instance.Reset();
        PathFinder.instance.FindGlobalPath(map.GetCellAt(map.xStart, map.yStart), map.GetCellAt(map.xGoal, map.yGoal));
    }

    void DestroyAllEnemies() {
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        foreach (CanTakeDamage enemy in enemySpawner.GetComponentsInChildren<CanTakeDamage>()) {
            Destroy(enemy.gameObject);
        }
    }

    void LoadCells(JSONObject mapJson) {
        int cellsIndex = mapJson.keys.IndexOf("cells");
        LoadWalls(mapJson.list[cellsIndex]);
    }

    void LoadWalls(JSONObject cellsJson) {
        int wallsIndex = cellsJson.keys.IndexOf("walls");
        BuildWalls(cellsJson.list[wallsIndex]);
    }

    void BuildWalls(JSONObject wallsJson) {
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