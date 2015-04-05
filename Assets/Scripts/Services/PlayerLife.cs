using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLife : MonoBehaviour {

	public static PlayerLife instance;
	public bool playerDied = false;

	private int _Lives = 10;
	public int Lives {
		get{return _Lives;}
		set{
			if (_Lives > 0){
                if (value < _Lives) {
                    Camera.main.GetComponent<CameraShake>().Shake(0.4f);
                }
                _Lives = value;
				UpdateLivesLabel();
				if (_Lives <= 0){
					GameOver();
				}
			}
		}
	}

	//Prefabs
	public Text label;

	void Awake(){
		instance = this;
	}

	void Start(){
		UpdateLivesLabel();
	}

    public void Reset() {
        Lives = 10;
        playerDied = false;
    }

	void UpdateLivesLabel() {
		label.text = _Lives.ToString();
	}
	
	void GameOver() {
        playerDied = true;
        PlayerUpgrades.instance.ResetLastAmountEarned();
        EndGameMenu.instance.Show();
	}

	public void WinTheGame(){
        PlayerUpgrades.instance.WinUpgradePoints();
        EndGameMenu.instance.Show();
	}
}
