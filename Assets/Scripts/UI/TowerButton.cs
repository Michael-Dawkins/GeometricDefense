using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Button))]
public class TowerButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler  {

	//Used to avoid tower selection when ion charge is launched
	bool isNextTowerSelectionCanceled = false;

	public delegate void OnClickHandler();
	public delegate void OnMouseDownHandler();
	public delegate void OnMouseUpHandler();

	public event OnClickHandler OnClick;
	public event OnMouseDownHandler OnMouseDown;
	public event OnMouseUpHandler OnMouseUp;

	void Start () {
	
	}
	
	void Update () {
	
	}

	public void CancelNextTowerSelection(){
		isNextTowerSelectionCanceled = true;
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(!isNextTowerSelectionCanceled){
			if (OnClick != null){
				OnClick();
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData) {
		if (OnMouseUp != null){
			OnMouseUp();
		}	
	}

	public void OnPointerDown(PointerEventData eventData) {
		isNextTowerSelectionCanceled = false;
		if (OnMouseDown != null){
			OnMouseDown();
		}	
	}

}
