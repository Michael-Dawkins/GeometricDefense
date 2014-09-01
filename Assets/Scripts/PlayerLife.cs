using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLife : MonoBehaviour {

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
}
