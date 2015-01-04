using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IonChargeAttackSquare : IonChargeAttack {

	List<Tile> adjacentTiles = new List<Tile>();

	protected override void Start() {
		base.Start();
		DamageMultiplier = 1.2f;
		Shoot();
		AudioClip clip = Instantiate(Resources.Load("laser11")) as AudioClip;
		audio.PlayOneShot(clip, 1f);
	}
	
	void Shoot(){
		PulsateAdjacentTiles();
		InflictDamageToEnemiesOnAdjacentTiles();
	}

	void InflictDamageToEnemiesOnAdjacentTiles(){
		GameObject enemySpawner = GameObject.Find("EnemySpawner");
		foreach(CanTakeDamage enemy in enemySpawner.GetComponentsInChildren<CanTakeDamage>()){
			if (IsObjectAdjacent(enemy.gameObject)){
				enemy.takeDamage(BaseDamage * DamageMultiplier, DamageTypeManager.DamageType.IonCharge);
			}
		}
	}

	void PulsateAdjacentTiles(){
		if (adjacentTiles.Count == 0){
			GameObject tilesObj = GameObject.Find("Tiles");
			foreach(Transform tileTrans in tilesObj.transform){
				Tile tile = tileTrans.gameObject.GetComponent<Tile>();
				if (IsObjectAdjacent(tileTrans.gameObject)){
					adjacentTiles.Add(tile);
				}
			}
		}
		foreach (Tile tile in adjacentTiles){
			tile.Pulsate(
				DamageTypeManager.GetDamageTypeColor(DamageTypeManager.DamageType.IonCharge),
				4f,//3 pulsations
				0.25f);//each one lasts for a second
		}
	}

	bool IsObjectAdjacent(GameObject obj){
		return Vector3.Distance(obj.transform.position, transform.position) < (Map.instance.cellSize + Map.instance.cellSize / 2f);
	}
}
