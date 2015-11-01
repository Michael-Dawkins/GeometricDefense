using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spawner : MonoBehaviour {

	public CanTakeDamage enemyToSpawn1;
	public CanTakeDamage enemyToSpawn2;
	public CanTakeDamage enemyToSpawn3;
	public CanTakeDamage enemyToSpawnBoss1;
	public CanTakeDamage enemyToSpawnBoss2;
	public CanTakeDamage enemyToSpawnBoss3;
    public GameObject startWaveLabel;

	public float everyXSeconds = 1.5f;
	public int numberOfWaves = 4;
	public int numberOfAddedEnemyPerWave = 3;

	public float hpIncreaseMultiplier = 1.5f;
	public float currentBaseLife;
	private float nextSpawningTime = 0;
	public int totalNumberOfEnemyInCurrentWave = 10;
	public int currentWaveProgress = 0;
	public int currentWave = 0;
	public Text waveCounterText;
	private bool waitingForUserToStartWave = true;
	private int enemiesAlive = 0;

	void Start () {
		UpdateWaveCounterDisplay();
	}

	void Update(){
		if(!waitingForUserToStartWave){
			if (nextSpawningTime < Time.time){
				if (currentWaveProgress < totalNumberOfEnemyInCurrentWave){
					currentWaveProgress ++;
					nextSpawningTime = Time.time + everyXSeconds;
					SpawnEnemy();
				} else {
					waitingForUserToStartWave = true;
                    if (currentWave != numberOfWaves)
                        startWaveLabel.SetActive(true);
				}
			}
		}
	}

    public void Reset() {
        waitingForUserToStartWave = true;
        currentWaveProgress = 0;
        nextSpawningTime = 0;
        currentWave = 0;
        enemiesAlive = 0;
        totalNumberOfEnemyInCurrentWave = 10;
        UpdateWaveCounterDisplay();
        startWaveLabel.SetActive(true);
    }

	public void StartNextWave(){
		if(waitingForUserToStartWave && !PlayerLife.instance.playerDied && currentWave < numberOfWaves) {
			currentWave ++;
			UpdateWaveCounterDisplay();
			nextSpawningTime = 0f;//start now
			currentWaveProgress = 0;
			totalNumberOfEnemyInCurrentWave += numberOfAddedEnemyPerWave;
			waitingForUserToStartWave = false;
            startWaveLabel.SetActive(false);
            if (Time.timeScale == 0f) {
                PlayPause.instance.TogglePlayPause();
            }
		}
	}

	public void NotifyThatEnemyDied(){
		enemiesAlive --;
		if (currentWave == numberOfWaves && enemiesAlive == 0 && currentWaveProgress == totalNumberOfEnemyInCurrentWave){
			if(!PlayerLife.instance.playerDied){
				Win();
			}
		}
	}

	void UpdateWaveCounterDisplay(){
		waveCounterText.text = currentWave.ToString() + " / " + numberOfWaves.ToString();
	}

	void SpawnEnemy(){
		CanTakeDamage enemy = Instantiate(GetEnemyToSpaw(), transform.position, transform.rotation) as CanTakeDamage;
		enemy.spawner = this;
		enemy.transform.SetParent(transform);
		CanTakeDamage damageable = enemy.GetComponent<CanTakeDamage>();
		damageable.InitialHp = currentBaseLife * hpIncreaseMultiplier;
        if (EnemyShouldBecoreABoss()) {
            TransformEnemyIntoBoss(enemy);
        }
		enemiesAlive++;
        if (currentWaveProgress == totalNumberOfEnemyInCurrentWave) {
            currentBaseLife *= hpIncreaseMultiplier;
        }
	}

	CanTakeDamage GetEnemyToSpaw(){
		float random = Random.value;
		if(random < 0.33f){
			return enemyToSpawn1;
		} else if(random < 0.66f){
			return enemyToSpawn2;
		} else {
			return enemyToSpawn3;
		}
	}

    //Should be called each time a enemy spawns, return true if the enemy should becore a boss
    bool EnemyShouldBecoreABoss() {
        //This is potentially unfair if two or three bosses succeed
        return Random.value < 0.05f;
    }

    //Turns a normal eny into its boss version, with more life, slower, different skin, larger
    void TransformEnemyIntoBoss(CanTakeDamage enemy) {
        enemy.InitialHp = enemy.InitialHp * 2.5f;
        SpriteRenderer centerRenderer = enemy.transform.Find("NeonCenter").gameObject.GetComponent<SpriteRenderer>();
        Debug.Log(centerRenderer.sprite.name + "-boss");
        centerRenderer.sprite = Resources.Load(centerRenderer.sprite.name + "-boss", typeof(Sprite)) as Sprite;
        CanMove canMove = enemy.gameObject.GetComponent<CanMove>();
        canMove.speed = canMove.speed / 1.5f;
        canMove.transform.localScale *= 1.3f;
    }

	void Win(){
		PlayerLife playerLife = PlayerLife.instance;
		playerLife.WinTheGame();
	}
}
