using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Values : MonoBehaviour {

	public static Values instance;
	
	void Awake(){
		instance = this;
	}

	void Start(){
		TowerLevels circleLevels = new TowerLevels();
		TowerLevels squareLevels = new TowerLevels();
		TowerLevels triangleLevels = new TowerLevels();
		Towers.Add(TowerTypeManager.TowerType.Circle, circleLevels);
		Towers.Add(TowerTypeManager.TowerType.Square, squareLevels);
		Towers.Add(TowerTypeManager.TowerType.Triangle, triangleLevels);

		// *********************************** STATS **************************************\\
		//Values.Towers[square].Levels[1].Damage;

		//Circle
		circleLevels.Levels.Add(1, new CircleStats(Damage: 15f, CellRange: 1f, ShootingRate: 1f));
		circleLevels.Levels.Add(2, new CircleStats(Damage: 30f, CellRange: 1.4f, ShootingRate: 1.5f));
		circleLevels.Levels.Add(3, new CircleStats(Damage: 60f, CellRange: 1.8f, ShootingRate: 2.2f));
		circleLevels.Levels.Add(4, new CircleStats(Damage: 110f, CellRange: 3f, ShootingRate: 2.5f));

		//Square
		squareLevels.Levels.Add(1, new SquareStats(Damage: 8f, CellRange: 1f));
		squareLevels.Levels.Add(2, new SquareStats(Damage: 16f, CellRange: 1f));
		squareLevels.Levels.Add(3, new SquareStats(Damage: 28f, CellRange: 1f));
		squareLevels.Levels.Add(4, new SquareStats(Damage: 55f, CellRange: 1f));

		//Triangle
		triangleLevels.Levels.Add(1, new TriangleStats(Damage: 10f, CoolDown: 2f));
		triangleLevels.Levels.Add(2, new TriangleStats(Damage: 18f, CoolDown: 1.4f));
		triangleLevels.Levels.Add(3, new TriangleStats(Damage: 32f, CoolDown: 0.8f));
		triangleLevels.Levels.Add(4, new TriangleStats(Damage: 70f, CoolDown: 0.2f));
	}

	public Dictionary<TowerTypeManager.TowerType, TowerLevels> Towers = new Dictionary<TowerTypeManager.TowerType, TowerLevels>();
	
	public class TowerLevels{
		public Dictionary<int, TowerStats> Levels = new Dictionary<int, TowerStats>();
	}
	
	public class TowerStats{
		public float Damage;
		public float CellRange = 1f;
		public float ShootingRate;
		public float CoolDown;
	}
	
	class CircleStats : TowerStats{
		public CircleStats(float Damage, float CellRange, float ShootingRate){
			this.Damage = Damage;
			this.CellRange = CellRange;
			this.ShootingRate = ShootingRate;
		}
	}

	class SquareStats : TowerStats{
		public SquareStats(float Damage, float CellRange){
			this.Damage = Damage;
			this.CellRange = CellRange;
		}
	}

	class TriangleStats : TowerStats{
		public TriangleStats(float Damage, float CoolDown){
			this.Damage = Damage;
			this.CoolDown = CoolDown;
		}
	}
}
