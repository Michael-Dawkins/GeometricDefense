using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanMove : MonoBehaviour {

	public float speed = 2f;
	PathFinder pathFinder;
	int indexInPath = 0;
	List<Cell> path;
	Vector3 currentTargetPos;
	private Map map;

	// Use this for initialization
	void Start () {
		map = GameObject.Find("Map").GetComponent<Map>();
		pathFinder = GameObject.Find ("PathFinder").GetComponent<PathFinder> ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveAlongPath ();
	}

	void moveRight(){
		transform.position = Vector2.Lerp(
			transform.position,
			transform.position + Vector3.right, 
			Time.deltaTime * speed);
	}

	void MoveAlongPath(){
		if (path == null){
			path = pathFinder.pathFound;
		}
		currentTargetPos = map.GetCellPos (path [indexInPath]);
		if (currentTargetPos == transform.position){
			indexInPath++;
			if (indexInPath == path.Count){
				ReachGoal();
				return;
			}
			currentTargetPos = map.GetCellPos (path [indexInPath]);
		}
		transform.position = Vector2.MoveTowards(
			transform.position,
			currentTargetPos, 
			Time.deltaTime * speed);
	}

	void ReachGoal(){
		Debug.Log("Enemy has reached its goal");
		Destroy(gameObject);
	}
}
