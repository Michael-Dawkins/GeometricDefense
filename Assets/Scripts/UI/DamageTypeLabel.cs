using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTypeLabel : MonoBehaviour {

	Text damageTypeLabel;
	DamageTypeManager damageTypeManager;

	void Start () {
		damageTypeManager = DamageTypeManager.instance;
		damageTypeManager.AddDamageTypeSelectionChangeListener(UpdateDamageTypeLabel);
		damageTypeLabel = GetComponent<Text>();
        UpdateDamageTypeLabel();
	}

	void UpdateDamageTypeLabel(){
		damageTypeLabel.text = damageTypeManager.GetDamageTypeLabel(damageTypeManager.currentDamageType);
	}

}
