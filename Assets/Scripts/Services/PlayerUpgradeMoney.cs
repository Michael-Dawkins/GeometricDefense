using UnityEngine;
using System.Collections;

public class PlayerUpgradeMoney : MonoBehaviour {

    public static PlayerUpgradeMoney instance;

    float _Money;
    public float Money {
        get { return _Money; }
        set { _Money = value; }
    }

    void Awake() {
        instance = this;
    }
    
    void Start () {
        _Money = 0;
	}

    void OnDestroy() {
        //TODO save in player prefs
    }
	
}
