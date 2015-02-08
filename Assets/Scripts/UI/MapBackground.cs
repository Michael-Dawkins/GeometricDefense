using UnityEngine;
using System.Collections;

public class MapBackground : MonoBehaviour {

    public static MapBackground instance;
	public GameObject lineSpritePrefab;
	public float lineWidth;

	Map map;
	GameObject left;
	GameObject top;
	GameObject right;
	GameObject bottom;

    void Awake() {
        instance = this;
    }

	void Start () {
		map = Map.instance;
		left = Instantiate(lineSpritePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		top = Instantiate(lineSpritePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		right = Instantiate(lineSpritePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		bottom = Instantiate(lineSpritePrefab, Vector3.zero, Quaternion.identity) as GameObject;
	}
	
    public void UpdateNeonBorderPosition(){
        left.transform.rotation = Quaternion.Euler(0, 0, 90f);
		left.transform.localScale = new Vector3(map.mapHeight * map.cellSize * 100f, lineWidth, 0);
		top.transform.localPosition = new Vector3(0, map.mapHeight * map.cellSize,0);
		top.transform.localScale = new Vector3(map.mapWidth * map.cellSize * 100f, lineWidth, 0);
		right.transform.rotation = Quaternion.Euler(0, 0, 90f);
		right.transform.localScale = new Vector3(map.mapHeight * map.cellSize * 100f, lineWidth, 0);
		right.transform.localPosition = new Vector3(map.mapWidth * map.cellSize, 0, 0);
		bottom.transform.localScale = new Vector3(map.mapWidth * map.cellSize* 100f, lineWidth, 0);

    }
}
