using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spwaner : MonoBehaviour {
	
	public CanTakeDamage enemyToSpawn;

	public float everyXSeconds = 1.5f;
	public int numberOfWaves = 20;
	public int numberOfAddedEnemyPerWave = 3;
	public float pauseBetweenWaves = 10;

	public float hpIncreaseMultiplier = 1.5f;
	public float currentBaseLife;
	private float nextSpawningTime = 0;
	public int totalNumberOfEnemyInCurrentWave = 10;
	public int currentWaveProgress = 0;
	public int currentWave = 1;
	public Text waveCounterText;


	void Start () {
		UpdateWaveCounterDisplay();
		nextSpawningTime = Time.time + pauseBetweenWaves;
	}

	void Update(){
		if (nextSpawningTime < Time.time){
			if (currentWaveProgress < totalNumberOfEnemyInCurrentWave){
				currentWaveProgress ++;
				nextSpawningTime = Time.time + everyXSeconds;
				SpawnEnemy();
			} else {
				if (currentWave == numberOfWaves){
					Win();
				} else {
					currentWave ++;
					UpdateWaveCounterDisplay();
					nextSpawningTime = Time.time + pauseBetweenWaves;
					currentWaveProgress = 0;
					totalNumberOfEnemyInCurrentWave += numberOfAddedEnemyPerWave;
					SpawnEnemy();
				}
			}
		}
	}

	void UpdateWaveCounterDisplay(){
		waveCounterText.text = currentWave.ToString() + " / " + numberOfWaves.ToString();
	}

	void SpawnEnemy(){
		CanTakeDamage enemy = Instantiate (enemyToSpawn, transform.position, transform.rotation) as CanTakeDamage;
		CanTakeDamage damageable = enemy.GetComponent<CanTakeDamage>();
		if (currentBaseLife == 0){
			currentBaseLife = damageable.InitialHp;
		} else if (currentWaveProgress == 0){
			currentBaseLife *= hpIncreaseMultiplier;
		}
		damageable.InitialHp = currentBaseLife * hpIncreaseMultiplier;
	}

	void Win(){
		PlayerLife playerLife = PlayerLife.instance;
		playerLife.WinTheGame();
	}
}
