using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanMove : MonoBehaviour {

	public float speed = 2f;

	PathFinder pathFinder;
	int indexInPath = 0;
	List<Cell> path;
	Vector3 currentTargetPos;
	Map map;
	Transform transformToRotate;

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
		if (indexInPath > 0){
			if (transformToRotate == null){
				Transform[] t = transform.gameObject.GetComponentsInChildren<Transform>();
				//get the first child
				transformToRotate = t[1];
			}
			float angle = Vector3.Angle(Vector3.right, map.GetCellPos (path [indexInPath]) - map.GetCellPos (path [indexInPath - 1]));
			transformToRotate.eulerAngles = new Vector3(0,0,angle);
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
