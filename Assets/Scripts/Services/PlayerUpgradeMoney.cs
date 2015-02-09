using UnityEngine;
using System.Collections;

public class PlayerUpgradeMoney : MonoBehaviour {

    static string PLAYER_PREFS_KEY = "PLAYER_UPGRADE_MONEY";
    public static PlayerUpgradeMoney instance;

    public event OnPlayerUpgradeMoneyChange PlayerUpgradeMoneyChange;
    public delegate void OnPlayerUpgradeMoneyChange();

    float _Money;
    public float Money {
        get { return _Money; }
        set { 
            _Money = value;
            SaveMoneyInPlayerPrefs();
            if (PlayerUpgradeMoneyChange != null)
                PlayerUpgradeMoneyChange();
        }
    }

    void Awake() {
        instance = this;
    }
    
    void Start () {
        _Money = 100;
        if (PlayerPrefs.HasKey(PLAYER_PREFS_KEY)) {
            _Money = PlayerPrefs.GetFloat(PLAYER_PREFS_KEY);
        } else {
            Debug.Log("First load, no playerUpgradeMoney found in PlayerPrefs");
        }
	}

    void SaveMoneyInPlayerPrefs() {
        PlayerPrefs.SetFloat(PLAYER_PREFS_KEY, Money);
        Debug.Log("Player upgrade money saved in PlayerPrefs, new Value : " + Money);
    }

    public void ClearPlayerPrefs(){
        Debug.Log("PlayerPref " + PLAYER_PREFS_KEY + " was deleted from PlayPrefs");
        PlayerPrefs.DeleteKey(PLAYER_PREFS_KEY);
    }
	
}
