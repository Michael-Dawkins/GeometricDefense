using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public static Map instance;

	public List<Cell> cells = new List<Cell>();
	public int mapWidth = 15;
	public int mapHeight = 10;
	public float cellSize = 0.4f;
	public int xGoal = 14;
	public int yGoal = 4;
	public int xStart = 0;
	public int yStart = 4;
	public GameObject goalSprite;

	void Awake () {
		instance = this;

		for (int x = 0; x < mapWidth; x++){
			for (int y = 0; y < mapHeight; y++){
				cells.Add(new Cell(x, y, false));
			}
		}

		Instantiate(goalSprite, GetCellPos(GetCellAt(xGoal, yGoal)), Quaternion.identity);
	}

	//TODO Optimize this, too many foreach to find cells
	public List<Cell> GetWalkableAdjacentSquares(Cell cell){
		List<Cell> tmp = new List<Cell>();
		int x, y;

		//Top
		x = cell.x;
		y = cell.y + 1;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(GetCellAt(x, y));
		}

		//Right
		x = cell.x + 1;
		y = cell.y;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(GetCellAt(x, y));
		}
		//Bottom
		x = cell.x;
		y = cell.y - 1;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(GetCellAt(x, y));
		}

		//Left
		x = cell.x -1;
		y = cell.y;
		if (IsCoordValid(x, y) && IsCoordWalkable(x,y)){
			tmp.Add(GetCellAt(x, y));
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

	public Cell GetCellAt(int x, int y){
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
		return GetCellAt(x, y);
	}

	public Vector3 GetCellPos(Cell cell){
		return new Vector3(cell.x * cellSize + cellSize / 2f, cell.y * cellSize + cellSize / 2f, 0f);
	}

	public Cell GetCellClosestToPos(float xPos, float yPos){
		int x = (int) (xPos / cellSize);
		int y = (int) (yPos / cellSize);
		//Clamping x and y to map bounds
		if (x < 0){
			x = 0;
		}
		if (y < 0){
			y = 0;
		}
		if (x >= mapWidth){
			x = mapWidth - 1;
		}
		if (y >= mapHeight){
			y = mapHeight - 1;
		}
		return GetCellAt(x, y);
	}

	public Cell GetStartCell(){
		return GetCellAt(xStart, yStart);
	}
}