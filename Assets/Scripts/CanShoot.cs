using UnityEngine;
using System.Collections;

public class CanShoot : MonoBehaviour {

	public int Damage = 10;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			anim.SetTrigger("shooting");
		}
	}
}
