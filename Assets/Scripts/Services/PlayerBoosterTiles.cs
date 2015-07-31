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

    private static string MAX_RANGE_BOOSTER_AMOUNT = "MaxRangeBoosterAmount";
    private static string MAX_DAMAGE_BOOSTER_AMOUNT = "MaxDamageBoosterAmount";

    public delegate void OnAmountChange(int amount);

    void Awake() {
        instance = this;
        rangeBoosterAmountListeners = new List<OnAmountChange>();
        damageBoosterAmountListeners = new List<OnAmountChange>();
    }

	void Start () {
        if (PlayerPrefs.HasKey(MAX_RANGE_BOOSTER_AMOUNT)) {
            MaxRangeBoosterAmount = (int)SaveTools.LoadFromPlayerPrefs(MAX_RANGE_BOOSTER_AMOUNT);
            MaxDamageBoosterAmount = (int)SaveTools.LoadFromPlayerPrefs(MAX_DAMAGE_BOOSTER_AMOUNT);
        } else {
            Debug.Log("First load, no booster amount found in player prefs");
            MaxDamageBoosterAmount = 0;
            MaxRangeBoosterAmount = 0;
            SaveTools.SaveInPlayerPrefs(MAX_RANGE_BOOSTER_AMOUNT, MaxRangeBoosterAmount);
            SaveTools.SaveInPlayerPrefs(MAX_DAMAGE_BOOSTER_AMOUNT, MaxDamageBoosterAmount);
        }
        CurrentRangeBoosterAmount = MaxRangeBoosterAmount;
        CurrentDamageBoosterAmount = MaxDamageBoosterAmount;
    }

    public void EarnOneDamageBooster() {
        CurrentDamageBoosterAmount++;
        MaxDamageBoosterAmount++;
        SaveTools.SaveInPlayerPrefs(MAX_DAMAGE_BOOSTER_AMOUNT, MaxDamageBoosterAmount);
    }

    public void EarnOneRangeBooster() {
        CurrentRangeBoosterAmount++;
        MaxRangeBoosterAmount++;
        SaveTools.SaveInPlayerPrefs(MAX_RANGE_BOOSTER_AMOUNT, MaxRangeBoosterAmount);
    }

    public void ClearPlayerPrefs() {
        Debug.Log("PlayerPref " + MAX_DAMAGE_BOOSTER_AMOUNT + " was deleted from PlayPrefs");
        PlayerPrefs.DeleteKey(MAX_DAMAGE_BOOSTER_AMOUNT);
        Debug.Log("PlayerPref " + MAX_RANGE_BOOSTER_AMOUNT + " was deleted from PlayPrefs");
        PlayerPrefs.DeleteKey(MAX_RANGE_BOOSTER_AMOUNT);
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
