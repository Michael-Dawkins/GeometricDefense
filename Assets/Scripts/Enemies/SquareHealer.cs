using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareHealer : MonoBehaviour {

    float minSecondsBeforeHeal = 4f;
    float maxSecondsBeforeHeal = 25f;
    float healPercentage = 40f; //At 40f, all surrounding enemies can be healed up to 40% of their max HP
    float nextHealingTime;
    bool isSpinning = false;
    float timeToSpin = 0.7f; //in seconds
    float spinningTimer;
    float numberOfSpins = 1f;
    Transform centerTransform;
    Transform glowTransform;
    Quaternion initialRotation;

    void Start() {
        UpdateNextHealingTime();
        centerTransform = transform.Find("NeonCenter");
        glowTransform = transform.Find("NeonGlow");
        initialRotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
	
	void Update () {
        if (Time.time > nextHealingTime) {
            HealSurroundingEnemies();
            StartSpinningAnimation();
            UpdateNextHealingTime();
        }
        if (isSpinning) {
            SpinningAnimation();
        }
	}

    void UpdateNextHealingTime() {
        nextHealingTime = Time.time + Random.Range(minSecondsBeforeHeal, maxSecondsBeforeHeal);
    }

    void HealSurroundingEnemies(){
        SoundManager.instance.PlaySound(SoundManager.SQUARE_HEALING);
        List<CanTakeDamage> surroundingEnemies = FindAllSurroundingEnemies();
        foreach (CanTakeDamage enemy in surroundingEnemies) {
            enemy.HealPercentage(healPercentage);
        }
    }

    void StartSpinningAnimation() {
        isSpinning = true;
        spinningTimer = 0f;
    }

    void SpinningAnimation() {
        spinningTimer += Time.deltaTime * (1f / timeToSpin);
        spinningTimer = Mathf.Clamp(spinningTimer, 0f, 1f);
        float slerpedTimer = Mathfx.Sinerp(0f, 1f, spinningTimer);//sinerp for easeOut
        Quaternion currentRotation = Quaternion.AngleAxis(360f * numberOfSpins * slerpedTimer, Vector3.forward);
        centerTransform.localRotation = currentRotation;
        glowTransform.localRotation = currentRotation;
        if (spinningTimer == 1f) {
            isSpinning = false;
            centerTransform.localRotation = initialRotation;
            glowTransform.localRotation = initialRotation;
        }
    }

    List<CanTakeDamage> FindAllSurroundingEnemies() {
        List<CanTakeDamage> surroundingEnemies = new List<CanTakeDamage>();
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        foreach (CanTakeDamage enemy in enemySpawner.GetComponentsInChildren<CanTakeDamage>()) {
            if (IsObjectAdjacent(enemy.gameObject)) {
                surroundingEnemies.Add(enemy);
            }
        }
        return surroundingEnemies;
    }

    bool IsObjectAdjacent(GameObject obj) {
        return Vector3.Distance(obj.transform.position, transform.position) < (Map.instance.cellSize + Map.instance.cellSize / 2f);
    }

}
