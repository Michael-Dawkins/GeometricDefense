using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	private List<Cell> openList = new List<Cell>();
	private List<Cell> closedList = new List<Cell>();

	public List<Cell> pathFound = new List<Cell>();
	private Map map;

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
		//FindPath(map.CellAt(0,map.mapHeight / 2), map.CellAt(map.mapWidth -1, map.mapHeight / 2));
		FindGlobalPath(map.CellAt(0,0), map.CellAt(map.xGoal, map.yGoal));
	}
	
	// Update is called once per frame
	void Update () {
		DebugPath();
		DebugMapObstacles();
	}

	void DebugPath(){
		Vector3 start = new Vector3();
		Vector3 end = new Vector3();
		for (int i = 0; i < pathFound.Count - 1; i++){
			start[0] = pathFound[i].x * map.cellSize;
			start[1] = pathFound[i].y * map.cellSize;
			end[0] = pathFound[i + 1].x * map.cellSize;
			end[1] = pathFound[i + 1].y * map.cellSize;
			Debug.DrawLine(start, end, Color.green);
		}
	}

	void DebugMapObstacles(){
		List<Cell> cells = map.cells;
		string message = "";
		foreach(Cell cell in cells){
			message += cell.SimpleToString();
			Color color;
			if (cell.isObstacle){
				color = Color.red;
			} else {
				color = Color.grey;
			}
			float sizeOfCellSize = 0.9f;
			float x = cell.position.x;
			float y = cell.position.y;
			Vector3 bottomLeft = new Vector3(x + (map.cellSize - map.cellSize*sizeOfCellSize) - (map.cellSize / 2f),
			                                 y + (map.cellSize - map.cellSize*sizeOfCellSize) - (map.cellSize / 2f),0);
			Vector3 bottomRight = new Vector3(x + map.cellSize*sizeOfCellSize - (map.cellSize / 2f),
			                                  y + (map.cellSize - map.cellSize*sizeOfCellSize) - (map.cellSize / 2f),0);
			Vector3 topRight = new Vector3(x + map.cellSize*sizeOfCellSize - (map.cellSize / 2f),
			                               y + map.cellSize*sizeOfCellSize - (map.cellSize / 2f),0);
			Vector3 topLeft = new Vector3(x + (map.cellSize - map.cellSize*sizeOfCellSize) - (map.cellSize / 2f),
			                              y + map.cellSize*sizeOfCellSize - (map.cellSize / 2f),0);
			Debug.DrawLine(bottomLeft, bottomRight, color);
			Debug.DrawLine(bottomRight, topRight, color);
			Debug.DrawLine(topRight, topLeft, color);
			Debug.DrawLine(topLeft, bottomLeft, color);
		}
		Debug.Log(message);
	}

	//Try to find a new global path, if not possible, it returns false
	public bool requestNewGlobalPath(Cell originCell, Cell destinationCell){
		List<Cell> path = FindPath(originCell, destinationCell);
		if (path == null){
			return false;
		}
		pathFound = path;
		return true;
	}

	public void FindGlobalPath(Cell originCell, Cell destinationCell){
		pathFound = FindPath(originCell, destinationCell);
	}

	public List<Cell> FindPath(Cell originCell, Cell destinationCell){
		//bool path = false;
		Cell currentCell;
		List<Cell> adjacentCells = new List<Cell>();
		List<Cell> path = new List<Cell>();

		openList.Add(originCell);
//		float timeBeforePathFinding = Time.realtimeSinceStartup;
		do {
			currentCell = GetCellWithLowestFScore();

			closedList.Add(currentCell);
			openList.Remove(currentCell);

			//Yeay, reconstruct the found path
			if (closedList.Contains(destinationCell)){
//				Debug.Log("Time to find path : " 
//				          + ((Time.realtimeSinceStartup - timeBeforePathFinding)*1000).ToString()
//				          + " ms");
				//path = true;
				Cell tmpCell = currentCell;
				do {
					path.Add(tmpCell);
					tmpCell = tmpCell.parent;
				} while (tmpCell != null);
				//release memory
				openList = new List<Cell>();
				closedList = new List<Cell>();
				path.Reverse();
				LogPath(path);
				for (int i = 0; i < map.mapWidth; i++){
					for(int j = 0; j < map.mapHeight; j++){
						map.CellAt(i,j).parent = null;
					}
				}
				return path;
			}

			adjacentCells = map.GetWalkableAdjacentSquares(currentCell);

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
		Debug.Log("No path found from " + originCell + " to " + destinationCell);
		return null;
	}

	public bool IsEnemyOnCurrentPath(Cell enemyCell){
		return (pathFound.Contains(enemyCell));
	}

	public int GetIndexOfEnemyOnCurrentPath(Cell enemyCell){
		int index = pathFound.IndexOf(enemyCell);
		if (pathFound.Count >1){
			index++;
		}
		return index;
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
