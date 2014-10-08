using UnityEngine;
using System.Collections;

public static class Singletons {
	public static PlayerLife playerLife;
	public static PlayerMoney playerMoney;
	public static Values values;
	public static DamageTypeManager damageTypeManager;
	public static TowerTypeManager towerTypeManager;
	public static Map map;
	public static PathFinder pathFinder;

	public static void InitializeServicesReferences(){
		GameObject services = GameObject.Find("Services");
		playerLife = services.GetComponent<PlayerLife>();
		playerMoney = services.GetComponent<PlayerMoney>();
		values = services.GetComponent<Values>();
		damageTypeManager = services.GetComponent<DamageTypeManager>();
		towerTypeManager = services.GetComponent<TowerTypeManager>();
		map = services.GetComponent<Map>();
		pathFinder = services.GetComponent<PathFinder>();
	}
}
