using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Towers : MonoBehaviour {

    public static Towers instance;

    public List<GameObject> TowerList{
        get {
            return new List<GameObject>(GameObject.FindGameObjectsWithTag("tower"));
        }
    }

    void Awake() {
        instance = this;
    }
    
    public void DestroyAllTowers(){
        foreach(GameObject tower in TowerList){
            Destroy(tower);
        }
    }
	
}
