using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTypeAdditionalLabel : MonoBehaviour {

    Text damageTypeAdditionalLabel;
    DamageTypeManager damageTypeManager;

    void Start() {
        damageTypeManager = DamageTypeManager.instance;
        damageTypeManager.AddDamageTypeSelectionChangeListener(UpdateDamageTypeLabel);
        damageTypeAdditionalLabel = GetComponent<Text>();
        UpdateDamageTypeLabel();
    }

    void UpdateDamageTypeLabel() {
        damageTypeAdditionalLabel.text = damageTypeManager.GetDamageTypeAdditionalLabel(
            damageTypeManager.currentDamageType);
    }
	
}
