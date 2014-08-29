using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	public List<Cell> cells = new List<Cell>();
	public int mapWidth = 14;
	public int mapHeight = 10;
	public float cellSize = 0.4f;
	public int xGoal = 7;
	public int yGoal = 7;
	public GameObject goalSprite;

	void Start () {
		for (int x = 0; x < mapWidth; x++){
			for (int y = 0; y < mapHeight; y++){
				cells.Add(new Cell(x, y, false));
			}
		}

		Instantiate(goalSprite, GetCellPos(CellAt(xGoal, yGoal)), Quaternion.identity);
	}

	void Update(){

	}

	//TODO Optimize this, too many foreach to find cells
	public List<Cell> GetWalkableAdjacentSquares(Cell cell){
		List<Cell> tmp = new List<Cell>();
		int x, y;

		//Top
		x = cell.x;
		y = cell.y + 1;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(CellAt(x, y));
		}

		//Right
		x = cell.x + 1;
		y = cell.y;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(CellAt(x, y));
		}
		//Bottom
		x = cell.x;
		y = cell.y - 1;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(CellAt(x, y));
		}

		//Left
		x = cell.x -1;
		y = cell.y;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(CellAt(x, y));
		}
		return tmp;
		
	}

	bool IsCoordValid(int x, int y){
		return (0 <= x && x < mapWidth) && (0 <= y && y < mapHeight);
	}

	bool IsCoordWalkable(int x, int y){
		foreach (Cell cell in cells){
			if (cell.x == x && cell.y == y){
				return !cell.isObstacle;
			}
		}
		return false;
	}

	public Cell CellAt(int x, int y){
		foreach (Cell cell in cells){
			if (cell.x == x && cell.y == y){
				return cell;
			}
		}
		return null;
	}
	
	public Cell GetCellAtPos(float xPos, float yPos){
		int x = (int) (xPos / cellSize);
		int y = (int) (yPos / cellSize);
		return CellAt(x, y);
	}

	public Vector3 GetCellPos(Cell cell){
		return new Vector3(cell.x * cellSize, cell.y * cellSize, 0f);
	}
}