using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	private List<Cell> openList = new List<Cell>();
	private List<Cell> closedList = new List<Cell>();

	public List<Cell> pathFound = new List<Cell>();

	// Use this for initialization
	void Start () {
		Map.CreateMap();
		//FindPath(Map.CellAt(0,Map.mapHeight / 2), Map.CellAt(Map.mapWidth -1, Map.mapHeight / 2));
		FindPath(Map.CellAt(0,0), Map.CellAt(7, 7));
	}
	
	// Update is called once per frame
	void Update () {
		debugPath();
	}

	void debugPath(){
		Vector3 start = new Vector3();
		Vector3 end = new Vector3();
		for (int i = 0; i < pathFound.Count - 1; i++){
			start[0] = pathFound[i].x * Map.cellSize;
			start[1] = pathFound[i].y * Map.cellSize;
			end[0] = pathFound[i + 1].x * Map.cellSize;
			end[1] = pathFound[i + 1].y * Map.cellSize;
			Debug.DrawLine(start, end);
		}
	}

	public List<Cell> FindPath(Cell originCell, Cell destinationCell){
		//bool pathFound = false;
		Cell currentCell;
		List<Cell> adjacentCells = new List<Cell>();
		pathFound = new List<Cell>();

		openList.Add(originCell);
		float timeBeforePathFinding = Time.realtimeSinceStartup;
		do {
			currentCell = GetCellWithLowestFScore();

			closedList.Add(currentCell);
			openList.Remove(currentCell);

			//Yeay, reconstruct the found path
			if (closedList.Contains(destinationCell)){
				Debug.Log("Time to find path : " 
				          + ((Time.realtimeSinceStartup - timeBeforePathFinding)*1000).ToString()
				          + " ms");
				//pathFound = true;
				Cell tmpCell = currentCell;
				do {
					pathFound.Add(tmpCell);
					tmpCell = tmpCell.parent;
				} while (tmpCell != null);
				//release memory
				openList = new List<Cell>();
				closedList = new List<Cell>();
				pathFound.Reverse();
				LogPath(pathFound);
				return pathFound;
			}

			adjacentCells = Map.GetWalkableAdjacentSquares(currentCell);

			foreach (Cell adjacentCell in adjacentCells) {
				if (closedList.Contains(adjacentCell)) {
					continue;
				}
				int moveCost = CostToMoveToCell(currentCell, adjacentCell);
				if (!openList.Contains(adjacentCell)) {
					adjacentCell.parent = currentCell;
					adjacentCell.gScore = currentCell.gScore + moveCost;
					adjacentCell.hScore = ComputeHScoreFromCoord(adjacentCell,destinationCell);
					InsertInOpenList(adjacentCell);
				} else { // if its already in the open list
					//tests if using the current G score make the aSquare F score lower, 
					//if yes update the parent because it means its a better path
					if ((currentCell.gScore + moveCost) < adjacentCell.gScore){
						adjacentCell.gScore = currentCell.gScore + moveCost;
						//scroll has changed, we remove and add it back to keep the list in order
						openList.Remove(adjacentCell);
						InsertInOpenList(adjacentCell);
					}
				}
			}
		} while (openList.Count > 0);
		Debug.LogError("No path found from " + originCell + " to " + destinationCell);
		return null;
	}

	int ComputeHScoreFromCoord(Cell fromCell, Cell destCell){
		//Manhattan approach
		return Mathf.Abs(destCell.x - fromCell.x) + Mathf.Abs(destCell.y - fromCell.y);
	}

	int CostToMoveToCell(Cell fromCell, Cell toCell){
		return 1;
	}

	Cell GetCellWithLowestFScore(){
		//the list is ordered by InsertInOpenList
		return openList[0];
	}

	//Inserts in open list, ordered by the cell's F score
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

	public static void LogPath(List<Cell> path){
		string message = "Path : ";
		foreach(Cell cell in path){
			message += "x: " + cell.x + ", y: " + cell.y + "  → ";
		}
		Debug.Log(message);
	}
	
}
