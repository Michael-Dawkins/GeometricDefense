using UnityEngine;
using System.Collections;

public class Spwaner : MonoBehaviour {
	
	public CanTakeDamage enemyToSpawn;
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
		CanTakeDamage enemy = Instantiate (enemyToSpawn, transform.position, transform.rotation) as CanTakeDamage;
		CanTakeDamage damageable = enemy.GetComponent<CanTakeDamage>();
		damageable.InitialHp += currentIncrease;
	}
}
