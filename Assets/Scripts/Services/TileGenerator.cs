using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour {

	public static TileGenerator instance;

	public GameObject tilePrefab;
    Map map;
    GameObject tilesObj;
	
	void Awake(){
		instance = this;
	}

	void Start () {
		map = Map.instance;
		tilesObj = new GameObject("Tiles");
        GenerateTiles();
	}

    public void ResetTiles() {
        foreach (Transform tile in tilesObj.transform) {
            Destroy(tile.gameObject);
        }
        GenerateTiles();
    }

    void GenerateTiles() {
        for (int x = 0; x < map.mapWidth; x++) {
            for (int y = 0; y < map.mapHeight; y++) {
                GameObject tile = Instantiate(tilePrefab) as GameObject;
                tile.transform.position = new Vector3(x * map.cellSize + map.cellSize / 2f, y * map.cellSize + map.cellSize / 2f, 0);
                tile.transform.parent = tilesObj.transform;
                map.GetCellAt(x, y).tile = tile.GetComponent<Tile>();
            }
        }
    }
}
