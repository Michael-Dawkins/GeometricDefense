using UnityEngine;
using System.Collections;

public class EnemySpwaner : MonoBehaviour {
	
	public Enemy enemyToSpawn;
	public float rate = 3f;

	public float hpIncrease = 10f;
	public int increaseHpEvery = 5;
	private int spawnCounter = 0;

	private float currentIncrease = 0f;

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnEnemy", 0, rate);
	}

	void SpawnEnemy(){
		spawnCounter ++;
		if (spawnCounter % increaseHpEvery == 0){
			currentIncrease += hpIncrease;
		}
		Enemy enemy = Instantiate (enemyToSpawn, transform.position, transform.rotation) as Enemy;
		CanTakeDamage damageable = enemy.GetComponent<CanTakeDamage>();
		damageable.hp += currentIncrease;
	}
}
