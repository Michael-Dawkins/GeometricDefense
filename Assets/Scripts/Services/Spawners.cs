using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Spawners : MonoBehaviour {

    public static Spawners instance;

    public List<Spawner> SpawnerList {
        get {
            return new List<Spawner>(Transform.FindObjectsOfType<Spawner>());
        }
    }

    void Awake() {
        instance = this;
    }

    public void Reset() {
        SpawnerList.ForEach(s => s.Reset());
    }
	
}
