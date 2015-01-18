using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickReceptor : MonoBehaviour {

	public delegate void OnClickListener();

	List<OnClickListener> callbacks = new List<OnClickListener>();

	public void OnClick(){
		foreach (OnClickListener callback in callbacks){
			callback();
		}
	}

	public void AddOnClickListener(OnClickListener callback){
		callbacks.Add(callback);
	}

	public void RemoveOnClickListener(OnClickListener callback){
		callbacks.Remove(callback);
	}
}
