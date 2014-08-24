using System;

public class Cell {
	public int x;
	public int y;
	public int gScore;
	public int hScore;
	public Cell parent;
	public bool isObstacle;

	public int FScore {
		get {
			return gScore + hScore;
		}
	}

	public Cell (int x, int y, bool obstacle) {
		this.x = x;
		this.y = y;
		this.isObstacle = obstacle;
	}

	public override string ToString()
	{
		return "Cell x: " + x + ", y: " + y + "  --- G score: " + gScore + ", H score: " + hScore;
	}

	public bool isEqual(Cell otherCell){
		return x == otherCell.x && y == otherCell.y;
	}
}
