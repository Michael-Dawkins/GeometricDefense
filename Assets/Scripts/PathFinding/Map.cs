using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {
	public static List<Cell> cells = new List<Cell>();
	public static int mapWidth = 14;
	public static int mapHeight = 10;
	public static float cellSize = 0.4f;

	public static void CreateMap () {
		for (int x = 0; x < mapWidth; x++){
			for (int y = 0; y < mapHeight; y++){
				cells.Add(new Cell(x, y, false));
				//Debug.Log("adding x : " + x + "    y : " + y);
			}
		}
		setUpMapBorderDisplay();
	}

	static void setUpMapBorderDisplay(){
		List<Vector3> vertices = new List<Vector3>();
		GameObject obj = new GameObject("Line renderer");
		obj.AddComponent<LineRenderer>();
		LineRenderer lr = obj.GetComponent<LineRenderer>();
		vertices.Add(new Vector3(0,0,0));
		vertices.Add(new Vector3(mapWidth * cellSize,0,0));
		vertices.Add(new Vector3(mapWidth * cellSize,mapHeight * cellSize,0));
		vertices.Add(new Vector3(0,mapHeight * cellSize,0));
		vertices.Add(new Vector3(0,0,0));
		lr.SetVertexCount(vertices.Count);
		for (int i =0; i < vertices.Count; i++){
			lr.SetPosition(i, vertices[i]);
		}
		lr.SetWidth(0.1f,0.1f);
	}

	//TODO Optimize this, too many foreach to find cells
	public static List<Cell> GetWalkableAdjacentSquares(Cell cell){
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

	static bool IsCoordValid(int x, int y){
		return (0 <= x && x < mapHeight) && (0 <= y && y < mapWidth);
	}

	static bool IsCoordWalkable(int x, int y){
		foreach (Cell cell in cells){
			if (cell.x == x && cell.y == y){
				return !cell.isObstacle;
			}
		}
		return false;
	}

	public static Cell CellAt(int x, int y){
		foreach (Cell cell in cells){
			if (cell.x == x && cell.y == y){
				return cell;
			}
		}
		return null;
	}
	
	public static Cell GetCellAtPos(float xPos, float yPos){
		int x = (int) (xPos / cellSize);
		int y = (int) (yPos / cellSize);
		return Map.CellAt(x, y);
	}

	public static Vector3 GetCellPos(Cell cell){
		return new Vector3(cell.x * cellSize, cell.y * cellSize, 0f);
	}
}