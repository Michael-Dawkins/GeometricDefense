using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour {

    static string LAST_MAP_PLAYED = "LAST_MAP_PLAYED";
    public static MapLoader instance;
    Map map;

	void Awake() {
        instance = this;
	}

    void Start() {
        map = Map.instance;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        string mapToLoad = "map_1";//default map if the game has never been launched before
        if (PlayerPrefs.HasKey(LAST_MAP_PLAYED)) {
            mapToLoad = SaveTools.LoadFromPlayerPrefs(LAST_MAP_PLAYED) as string;
        } else {
            Debug.Log("First load, no LAST_MAP_PLAYED found in PlayerPrefs");
        }

        LoadMap(mapToLoad);
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
        map.UpdateGoalPosition();
        ResetGameState();
        SaveTools.SaveInPlayerPrefs(LAST_MAP_PLAYED, mapResourceName);
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
        LoadGoal(mapJson.list[cellsIndex]);
        LoadSpawner(mapJson.list[cellsIndex]);
        LoadDamageBoosters(mapJson.list[cellsIndex]);
    }

    void LoadGoal(JSONObject cellsJson) {
        int goalsIndex = cellsJson.keys.IndexOf("goals");
        //Only support one goal for now
        map.xGoal = (int)cellsJson.list[goalsIndex].list[0].list[0].n;
        map.yGoal = (int)cellsJson.list[goalsIndex].list[0].list[1].n;
    }

    void LoadSpawner(JSONObject cellsJson) {
        int spawnersIndex = cellsJson.keys.IndexOf("spawners");
        //Only support one spawner for now
        map.xStart = (int)cellsJson.list[spawnersIndex].list[0].list[0].n;
        map.yStart = (int)cellsJson.list[spawnersIndex].list[0].list[1].n;
        Spawners.instance.SpawnerList[0].transform.position = map.GetCellAt(map.xStart, map.yStart).position
            + new Vector3(map.cellSize / 2f, map.cellSize / 2f, 0);
    }

    void LoadDamageBoosters(JSONObject cellsJson) {
        int damageBoostersIndex = cellsJson.keys.IndexOf("damageBoosters");
        foreach (JSONObject damageBooster in cellsJson.list[damageBoostersIndex].list) {
            int x = (int)damageBooster.list[0].n;
            int y = (int)damageBooster.list[1].n;
            Cell cell = map.GetCellAt(x, y);
            cell.tile.tileType = Tile.TileType.DAMAGE_BOOSTER;
        }
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
            cell.tile.tileType = Tile.TileType.OBSTACLE;
        }
    }
}