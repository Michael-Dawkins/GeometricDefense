using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTypeLabel : MonoBehaviour {

	Text damageTypeLabel;
	DamageTypeManager damageTypeManager;

	void Start () {
		GameObject playerState = GameObject.Find("PlayerState");
		damageTypeManager = playerState.GetComponent<DamageTypeManager>();
		damageTypeManager.AddDamageTypeSelectionChangeListener(UpdateDamageTypeLabel);
		damageTypeLabel = GetComponent<Text>();
	}

	void UpdateDamageTypeLabel(){
		damageTypeLabel.text = damageTypeManager.GetDamageTypeLabel(damageTypeManager.currentDamageType);
	}

}
