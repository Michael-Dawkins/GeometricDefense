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
        ResetGameState();
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
        ResetGameState();
    }

    void ResetGameState() {
        PathFinder.instance.FindGlobalPath(map.GetCellAt(map.xStart, map.yStart), map.GetCellAt(map.xGoal, map.yGoal));
        PlayerLife.instance.Reset();
        PlayerMoney.instance.ResetAmount();
        MapBackground.instance.UpdateNeonBorderPosition();
        AdjustCameraToMapSize();
        Spawners.instance.Reset();
        Towers.instance.DestroyAllTowers();//TODO problem on onDeselect afterwards, check removal after tower selection
    }

    void AdjustCameraToMapSize() {
        float mapHeightInWorldUnits = map.mapHeight * map.cellSize;
        float mapWidthInWorldUnits = map.mapWidth * map.cellSize;
        float bottomOffset = 2.2f * map.cellSize;
        Camera camera = Camera.main;
        camera.orthographicSize = (mapHeightInWorldUnits + bottomOffset) / 2f;
        camera.transform.position = new Vector3(
            mapWidthInWorldUnits / 2f,
            (mapHeightInWorldUnits + bottomOffset) / 2f - bottomOffset + bottomOffset / 10f,
            -1f);
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