using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour {

	public GameObject tilePrefab;

	void Start () {
		Map map = GameObject.Find("Map").GetComponent<Map>();
		GameObject tiles = new GameObject("Tiles");
		for( int x = 0; x < map.mapWidth; x++){
			for(int y = 0; y < map.mapHeight; y++){
				GameObject tile = Instantiate(tilePrefab) as GameObject;
				tile.transform.position = new Vector3(x * map.cellSize + map.cellSize / 2f,y * map.cellSize + map.cellSize / 2f, 0);
				tile.transform.parent = tiles.transform;
			}
		}
	}
}
