using UnityEngine;
using System.Collections;

public class EnemySpwaner : MonoBehaviour {
	
	public Enemy enemyToSpawn;
	public float rate = 3f;

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnEnemy", 0, rate);
	}

	void SpawnEnemy(){
		Instantiate (enemyToSpawn, transform.position, transform.rotation);
	}
}
