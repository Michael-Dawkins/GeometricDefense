using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageTypeManager : MonoBehaviour{

	public static DamageTypeManager instance;

	public DamageType currentDamageType = DamageType.Plasma;
	public delegate void OnDamageTypeSelectionChange();
	public List<OnDamageTypeSelectionChange> callbacks = new List<OnDamageTypeSelectionChange>();

	public enum DamageType {
		Plasma,
		Antimatter,
		IonCharge
	}

	void Awake(){
		instance = this;
	}

	public Color GetDamageTypeColor(DamageType damageType){
		switch(damageType){
		case DamageType.Plasma:
			return new Color(0.15f,0.86f,1f);
		case DamageType.Antimatter:
			return new Color(0.82f,0.15f,1f);
		case DamageType.IonCharge:
			return new Color(1f,1f,0.15f);
		}
		return Color.white;
	}

	public void SelectNextDamageType(){
		switch(currentDamageType){
		case DamageType.Plasma:
			currentDamageType = DamageType.Antimatter;
			break;
		case DamageType.Antimatter:
			currentDamageType = DamageType.IonCharge;
			break;
		case DamageType.IonCharge:
			currentDamageType = DamageType.Plasma;
			break;
		}
		DamageTypeSelectionChange();
	}

	public string GetDamageTypeLabel(DamageType DamageType){
		switch(currentDamageType){
			case DamageType.Plasma:
				return "Plasma";
			case DamageType.Antimatter:
				return "Antimatter";
			case DamageType.IonCharge:
				return "Ion Charge";
		}
		return "Wrong damage type";
	}

	public void SelectCurrentDamageType(DamageType DamageType){
		currentDamageType = DamageType;
		DamageTypeSelectionChange();
	}

	public void DamageTypeSelectionChange(){
		foreach(OnDamageTypeSelectionChange callback in callbacks){
			callback();
		}
	}

	public void AddDamageTypeSelectionChangeListener(OnDamageTypeSelectionChange callback){
		callbacks.Add(callback);
	}

	public void RemoveDamageTypeSelectionChangeListener(OnDamageTypeSelectionChange callback){
		callbacks.Remove(callback);
	}
}
