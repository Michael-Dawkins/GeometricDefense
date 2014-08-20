using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {
	public static List<Cell> cells = new List<Cell>();
	public int mapWidth = 14;
	public int mapHeight = 8;

	public static void CreateMap () {
		bool isObstacle = false;
		for (int x = 0; x < mapWidth; x++){
			for (int y = 0; y < mapHeight; y++){
				cells.Add(new Cell(x, y, isObstacle));
			}
		}
	}

	//TODO Optimize this, too many foreach to find cells
	public static List<Cell> GetWalkableAdjacentSquares(Cell cell){
		List<Cell> tmp = new List<Cell>();
		int x, y;

		//Top
		x = cell.x;
		y = cell.y + 1;
		if (isCoordValid(x, y) && isCoordWalkable(x,y)){
			tmp.Add(cellAt(x, y));
		}

		//Right
		x = cell.x + 1;
		y = cell.y;
		if (isCoordValid(x, y) && isCoordWalkable(x,y)){
			tmp.Add(cellAt(x, y));
		}
		//Bottom
		x = cell.x;
		y = cell.y - 1;
		if (isCoordValid(x, y) && isCoordWalkable(x,y)){
			tmp.Add(cellAt(x, y));
		}

		//Left
		x = cell.x -1;
		y = cell.y;
		if (isCoordValid(x, y) && isCoordWalkable(x,y)){
			tmp.Add(cellAt(x, y));
		}
		return tmp;
		
	}

	bool isCoordValid(int x, int y){
		return (0 <= x < mapHeight) && (0 <= y < mapWidth);
	}

	bool isCoordWalkable(int x, int y){
		foreach (Cell cell in cells){
			if (cell.x == x && cell.y == y){
				return !cell.isObstacle;
			}
		}
		Debug.LogError("cell not found in map");
		return false;
	}

	Cell cellAt(int x, int y){
		foreach (Cell cell in cells){
			if (cell.x == x && cell.y == y){
				return cell;
			}
		}
	}

}