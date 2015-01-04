using UnityEngine;
using System.Collections;

public class TowerTypeManager : MonoBehaviour {

	public static TowerTypeManager instance;

	public enum TowerType{
		Circle, 
		Square, 
		Triangle
	}
	
	void Awake(){
		instance = this;
	}

}
