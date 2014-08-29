using UnityEngine;
using System.Collections;

public class MapBackground : MonoBehaviour {

	Map map;

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
		transform.position = new Vector3((map.mapWidth * map.cellSize) / 2f - (map.cellSize / 2f), (map.mapHeight * map.cellSize) / 2f, 1f);
		transform.localScale = new Vector3(map.mapWidth * map.cellSize, map.mapWidth * map.cellSize, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
