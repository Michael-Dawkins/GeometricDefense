using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTypeLabel : MonoBehaviour {

	Text damageTypeLabel;
	DamageTypeManager damageTypeManager;

	void Start () {
		damageTypeManager = Singletons.damageTypeManager;
		damageTypeManager.AddDamageTypeSelectionChangeListener(UpdateDamageTypeLabel);
		damageTypeLabel = GetComponent<Text>();
	}

	void UpdateDamageTypeLabel(){
		damageTypeLabel.text = damageTypeManager.GetDamageTypeLabel(damageTypeManager.currentDamageType);
	}

}
