using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	private List<Cell> openList = new List<Cell>();
	private List<Cell> closedList = new List<Cell>();

	// Use this for initialization
	void Start () {
		Map.CreateMap();
		FindPath(new Cell(2,2,false), new Cell(6,7,false));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FindPath(Cell originCell, Cell destinationCell){
		//bool pathFound = false;
		Cell currentCell;
		List<Cell> adjacentCells = new List<Cell>();

		openList.Add(originCell);

		do {
			currentCell = GetCellWithLowestFScore();

			closedList.Add(currentCell);
			openList.Remove(currentCell);

			//Yeay, reconstruct the found path
			if (closedList.Contains(destinationCell)){
				//pathFound = true;
				Cell tmpCell = currentCell;
				Debug.Log("Path found!");
				do {
					Debug.Log(tmpCell);
					tmpCell = tmpCell.parent;
				} while (tmpCell != null);
				//release memory
				openList = new List<Cell>();
				closedList = new List<Cell>();
				break;
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
	
}
