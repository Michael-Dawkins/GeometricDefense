using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanMove : MonoBehaviour {

	public float speed = 2f;
	public bool slowed = false;
	public bool speedUp = false;

	float speedUpRatio = 2f;
	float slowRatio = 0.5f;
	PathFinder pathFinder;
	int indexInPath = 0;
	List<Cell> path;
	Vector3 currentTargetPos;
	Map map;
	Transform neonCenterTransform;
	Transform neonGlowTransform;
	private PlayerLife playerLife;
    AudioClip reachGoalSound;

	// Use this for initialization
	void Start () {
		map = Map.instance;
		pathFinder = PathFinder.instance;
		playerLife = PlayerLife.instance;
        reachGoalSound = Instantiate(Resources.Load("EnemyToBase")) as AudioClip;
        gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		MoveAlongPath ();
	}
	
	public void SetOwnPath(){
		Cell currentCell = map.GetCellAtPos(transform.position.x, transform.position.y);
		Cell destination = map.GetCellAt(map.xGoal, map.yGoal);
		if (currentCell != null){
			if (!pathFinder.IsEnemyOnCurrentPath(currentCell)){
				path = pathFinder.FindPath(currentCell, destination);
				if (path.Count == 1){ //path is too short, enemy is nearly at destination
					indexInPath = 0;
				} else {
					indexInPath = 1;
				}
			} else {
				//enemy is still on the path, but it might not be at the same index if beggining of path has changed
				indexInPath = pathFinder.GetIndexOfEnemyOnCurrentPath(currentCell);
				path = pathFinder.pathFound;
			}
		} //if origin is null, enemy is not yet arrived close to a grid position (it just spawned)

	}

	void MoveAlongPath(){
		if (path == null){
			path = pathFinder.pathFound;
		}
		if (indexInPath >= path.Count){
			Debug.Log("Index path is too big");
			indexInPath = path.Count - 1;
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
			if (neonCenterTransform == null){
				neonCenterTransform = transform.FindChild("NeonCenter");
				neonGlowTransform = transform.FindChild("NeonGlow");

			}
			float angle = GDUtils.SignedAngleBetween(Vector3.right, 
			                                 		 map.GetCellPos (path [indexInPath]) - map.GetCellPos (path [indexInPath - 1]), 
			                                 		 Vector3.forward);
			neonCenterTransform.eulerAngles = new Vector3(0,0,angle);
			neonGlowTransform.eulerAngles = new Vector3(0,0,angle);

		}
		float amountOfMotion = Time.deltaTime * speed;
		if (slowed){
			amountOfMotion *= slowRatio;
		}
		if (speedUp){
			amountOfMotion *= speedUpRatio;
		}
		transform.position = Vector2.MoveTowards(
			transform.position,
			currentTargetPos, amountOfMotion);
	}

	void ReachGoal(){
		playerLife.Lives--;
        SoundManager.instance.PlaySound(SoundManager.ENEMY_TO_BASE);
		Destroy(gameObject);
	}
}
