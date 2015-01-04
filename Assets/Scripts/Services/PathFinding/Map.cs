using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public static Map instance;

	public List<Cell> cellsList = new List<Cell>();
	public int mapWidth = 15;
	public int mapHeight = 10;
	public float cellSize = 0.4f;
	public int xGoal = 14;
	public int yGoal = 4;
	public int xStart = 0;
	public int yStart = 4;
	public GameObject goalSprite;
	Dictionary<Cell, List<OnTowerAdd>> onTowerAddCallbacks;
	Dictionary<Cell, List<OnTowerUpgrade>> onTowerUpgradeCallbacks;
	Dictionary<Cell, List<OnTowerSell>> onTowerSellCallbacks;

	public delegate void OnTowerAdd();
	public delegate void OnTowerUpgrade();
	public delegate void OnTowerSell();
	public Cell[,] cellsArray;

	void Awake () {
		instance = this;
		cellsArray = new Cell[mapWidth, mapHeight];
		
		onTowerAddCallbacks = new Dictionary<Cell, List<OnTowerAdd>>();
		onTowerUpgradeCallbacks = new Dictionary<Cell, List<OnTowerUpgrade>>();
		onTowerSellCallbacks = new Dictionary<Cell, List<OnTowerSell>>();

		for (int x = 0; x < mapWidth; x++){
			for (int y = 0; y < mapHeight; y++){
				Cell cell = new Cell(x, y, false);
				cellsList.Add(cell);
				cellsArray[x, y] = cell;
				onTowerAddCallbacks[cell] = new List<OnTowerAdd>();
				onTowerSellCallbacks[cell] = new List<OnTowerSell>();
				onTowerUpgradeCallbacks[cell] = new List<OnTowerUpgrade>();

			}
		}

		Instantiate(goalSprite, GetCellPos(GetCellAt(xGoal, yGoal)), Quaternion.identity);
	}

	//Up to 4 cells are found (these are up, left, right, bottom)
	public List<Cell> FindDirectlyAdjacentCells(Cell cell){
		List<Cell> directlyAdjacentCells = new List<Cell>();
		int x = cell.x;
		int y = cell.y;
		Cell cellToAdd;

		//Left
		cellToAdd = GetCellAt(x - 1, y);
		if (cellToAdd != null){
			directlyAdjacentCells.Add(cellToAdd);
		}
		//Up
		cellToAdd = GetCellAt(x, y + 1);
		if (cellToAdd != null){
			directlyAdjacentCells.Add(cellToAdd);
		}
		//Right
		cellToAdd = GetCellAt(x + 1, y);
		if (cellToAdd != null){
			directlyAdjacentCells.Add(cellToAdd);
		}
		//Bottom
		cellToAdd = GetCellAt(x, y - 1);
		if (cellToAdd != null){
			directlyAdjacentCells.Add(cellToAdd);
		}
		return directlyAdjacentCells;
	}

	//Up to 8 cells can be found (these are up, left, right, bottom and the four equivalent diagonals)
	public List<Cell> FindAdjacentCells(Cell cell){
		List<Cell> adjacentCells = new List<Cell>();
		int x = cell.x;
		int y = cell.y;
		Cell cellToAdd;
		adjacentCells = FindDirectlyAdjacentCells(cell);
		//Top left
		cellToAdd = GetCellAt(x - 1, y + 1);
		if (cellToAdd != null){
			adjacentCells.Add(cellToAdd);
		}
		//Top right
		cellToAdd = GetCellAt(x + 1, y + 1);
		if (cellToAdd != null){
			adjacentCells.Add(cellToAdd);
		}
		//Bottom Left
		cellToAdd = GetCellAt(x - 1, y - 1);
		if (cellToAdd != null){
			adjacentCells.Add(cellToAdd);
		}
		//Bottom right
		cellToAdd = GetCellAt(x + 1, y - 1);
		if (cellToAdd != null){
			adjacentCells.Add(cellToAdd);
		}
		return adjacentCells;
	}

	public void NotifyAddTowerObservers(Cell cell){
		if (onTowerAddCallbacks.ContainsKey(cell)){
			foreach(OnTowerAdd callback in onTowerAddCallbacks[cell]){
				callback();
			}
		}
	}

	public void NotifyUpgradeTowerObservers(Cell cell){
		if (onTowerUpgradeCallbacks.ContainsKey(cell)){
			foreach(OnTowerUpgrade callback in onTowerUpgradeCallbacks[cell]){
				callback();
			}
		}
	}

	public void NotifySellTowerObservers(Cell cell){
		if (onTowerSellCallbacks.ContainsKey(cell)){
			foreach(OnTowerSell callback in onTowerSellCallbacks[cell]){
				callback();
			}
		}
	}

	public void AddOnTowerAddCallback(Cell cell, OnTowerAdd callback){
		onTowerAddCallbacks[cell].Add(callback);
	}

	public void RemoveOnTowerAddCallback(Cell cell, OnTowerAdd callback){
		onTowerAddCallbacks[cell].Remove(callback);
	}

	public void AddOnTowerUpgradeCallback(Cell cell, OnTowerUpgrade callback){
		onTowerUpgradeCallbacks[cell].Add(callback);
	}

	public void RemoveOnTowerUpgradeCallback(Cell cell, OnTowerUpgrade callback){
		onTowerUpgradeCallbacks[cell].Remove(callback);
	}

	public void AddOnTowerSellCallback(Cell cell, OnTowerSell callback){
		onTowerSellCallbacks[cell].Add(callback);
	}
	
	public void RemoveOnTowerSellCallback(Cell cell, OnTowerSell callback){
		onTowerSellCallbacks[cell].Remove(callback);
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
		foreach (Cell cell in cellsList){
			if (cell.x == x && cell.y == y){
				return !cell.isObstacle;
			}
		}
		return false;
	}

	public Cell GetCellAt(int x, int y){
		if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight){
			return null;
		}
		return cellsArray[x,y];
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