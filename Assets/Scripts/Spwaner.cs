using UnityEngine;
using System.Collections;

public class Spwaner : MonoBehaviour {
	
	public CanTakeDamage enemyToSpawn;
	public float everyXSeconds = 1.5f;

	public float hpIncrease = 10f;
	public int increaseHpEvery = 5;
	private int spawnCounter = 0;
	private float nextSpawningTime = 0;

	private float currentIncrease = 0f;

	void Start () {
	}

	void Update(){
		if (nextSpawningTime < Time.time){
			nextSpawningTime = Time.time + everyXSeconds;
			SpawnEnemy();
		}
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
