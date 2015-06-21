using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Responsible for the number of booster available in the game
public class PlayerBoosterTiles : MonoBehaviour {

    public static PlayerBoosterTiles instance;

    public int MaxRangeBoosterAmount {
        get { return _maxRangeBoosterAmount; }
        set {
            _maxRangeBoosterAmount = value;
            NotifyRangeBoosterAmountListener();
        }
    }

    public int CurrentRangeBoosterAmount {
        get { return _currentRangeBoosterAmount; }
        set {
            _currentRangeBoosterAmount = value;
            NotifyRangeBoosterAmountListener();
        }
    }

    public int MaxDamageBoosterAmount {
        get { return _maxDamageBoosterAmount; }
        set {
            _maxDamageBoosterAmount = value;
            NotifyDamageBoosterAmountListener();
        }
    }

    public int CurrentDamageBoosterAmount {
        get { return _currentDamageBoosterAmount; }
        set {
            _currentDamageBoosterAmount = value;
            NotifyDamageBoosterAmountListener();
        }
    }

    //The total number of range booster the player can place on the map
    private int _maxRangeBoosterAmount;
    //The available range booster amount (max - number on the map)
    private int _currentRangeBoosterAmount;

    //Same but for damage boosters
    private int _maxDamageBoosterAmount;
    private int _currentDamageBoosterAmount;

    private List<OnAmountChange> rangeBoosterAmountListeners;
    private List<OnAmountChange> damageBoosterAmountListeners;

    public delegate void OnAmountChange(int amount);

    void Awake() {
        instance = this;
        rangeBoosterAmountListeners = new List<OnAmountChange>();
        damageBoosterAmountListeners = new List<OnAmountChange>();
    }

	void Start () {
        //TODO load from player prefs, admin from player or end game loot
        MaxRangeBoosterAmount = 3;
        CurrentRangeBoosterAmount = 3;
        MaxDamageBoosterAmount = 3;
        CurrentDamageBoosterAmount = 3;
	}

    //Observer pattern for booster values (used to update the view
    public void AddRangeBoosterAmountListener(OnAmountChange callback) {
        rangeBoosterAmountListeners.Add(callback);
    }

    public void AddDamageBoosterAmountListener(OnAmountChange callback) {
        damageBoosterAmountListeners.Add(callback);
    }

    public void RemoveRangeBoosterAmountListener(OnAmountChange callback) {
        rangeBoosterAmountListeners.Remove(callback);
    }

    public void RemoveDamageBoosterAmountListener(OnAmountChange callback) {
        damageBoosterAmountListeners.Remove(callback);
    } 
    
    private void NotifyRangeBoosterAmountListener() {
        rangeBoosterAmountListeners.ForEach(c => c(CurrentRangeBoosterAmount));
    }

    private void NotifyDamageBoosterAmountListener() {
        damageBoosterAmountListeners.ForEach(c => c(CurrentDamageBoosterAmount));
    }

}
