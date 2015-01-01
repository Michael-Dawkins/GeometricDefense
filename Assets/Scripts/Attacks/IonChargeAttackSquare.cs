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
		if (adjacentTiles.Count == 0){
			GameObject tilesObj = GameObject.Find("Tiles");
			foreach(Transform tileTrans in tilesObj.transform){
				Tile tile = tileTrans.gameObject.GetComponent<Tile>();
				if (IsTileAdjacent(tileTrans.gameObject)){
					adjacentTiles.Add(tileTrans.gameObject.GetComponent<Tile>());
					tile.Pulsate(
						DamageTypeManager.instance.GetDamageTypeColor(DamageTypeManager.DamageType.IonCharge),
						4f,//3 pulsations
						0.25f);//each one lasts for a second
				}
			}
		}

	}

	bool IsTileAdjacent(GameObject tile){
		return Vector3.Distance(tile.transform.position, transform.position) < (Map.instance.cellSize + 0.2f);
	}
}
