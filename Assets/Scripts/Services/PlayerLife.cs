using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLife : MonoBehaviour {

	public static PlayerLife instance;

	private int _Lives = 10;
	public int Lives {
		get{return _Lives;}
		set{
			if (_Lives > 0){
				_Lives = value;
				UpdateLivesLabel();
				if (_Lives <= 0){
					DisplayGameOver();
				}
			}
		}
	}

	//Prefabs
	public Text label;
	public Text endingSentence;

	void Awake(){
		instance = this;
	}

	void Start(){
		UpdateLivesLabel();
	}

	void UpdateLivesLabel () {
		label.text = "lives: " + _Lives;
	}
	
	void DisplayGameOver () {
		endingSentence.enabled = true;
		endingSentence.text = "Game Over";
	}

	public void WinTheGame(){
		endingSentence.enabled = true;
		endingSentence.text = "You win!";
	}
}
