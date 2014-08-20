using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	private List<Cell> openList = new List<Cell>();
	private List<Cell> closedList = new List<Cell>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FindPath(Cell originCell, Cell destinationCell){
		Cell currentCell;
		List<Cell> adjacentCells = new List<Cell>();

		openList.Add(originCell);

		do {
			currentCell = GetCellWithLowestFScore(currentCell);

			closedList.Add(currentCell);
			openList.Remove(currentCell);

			if (closedList.Contains(destinationCell)){
				break;
			}

			adjacentCells = Map.GetWalkableAdjacentSquares();

			foreach (Cell cell in adjacentCells) {
				if (closedList.Contains(cell)) {
					continue;
				}
				if (!openList.Contains(cell)) {
					
					//TODO compute its score, set the parent

					openList.Add(cell);
				} else { // if its already in the open list
					//TODO test if using the current G score make the aSquare F score lower, if yes update the parent because it means its a better path
				}
			}
		} while (openList.Count > 0);

	}

	int computeHScoreFromCoord(int xFrom, int yFrom, int xTo, int yTo){
		//Manhattan approach
		return Mathf.Abs(xTo - xFrom) + Mathf.Abs(yTo - yFrom);
	}

	int costToMoveToCell(Cell fromCell, Cell toCell){
		return 1;
	}

	Cell GetCellWithLowestFScore(Cell arroundCell){

	}

	void InsertInOpenList(Cell cell){
		int f = cell.FScore;
		int i = 0;
		for (; i < openList.Count; i++){
			if (f <= openList[i].FScore){
				break;
			}
		}
		openList.Insert(i, cell);
	}
	
}
