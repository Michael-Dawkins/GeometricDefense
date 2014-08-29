using System;
using UnityEngine;

public class Cell {
	public int x;
	public int y;
	public int gScore;
	public int hScore;
	public Cell parent;
	public bool isObstacle;
	public Vector3 position{
		get {
			return new Vector3(x * map.cellSize, y * map.cellSize, 0);
		}
	}

	Map map;

	public int FScore {
		get {
			return gScore + hScore;
		}
	}

	public Cell (int x, int y, bool obstacle) {
		this.x = x;
		this.y = y;
		this.isObstacle = obstacle;
		map = GameObject.Find("Map").GetComponent<Map>();
	}

	public override string ToString() {
		return SimpleToString() + "  --- G score: " + gScore + ", H score: " + hScore;
	}

	public String SimpleToString(){
		return "Cell x: " + x + ", y: " + y;
	}

	public bool isEqual(Cell otherCell){
		return x == otherCell.x && y == otherCell.y;
	}
}
