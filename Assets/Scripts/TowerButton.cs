using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Button))]
public class TowerButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler  {

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

	public void OnPointerClick(PointerEventData eventData) {
		if (OnClick != null){
			OnClick();
		}
	}

	public void OnPointerUp(PointerEventData eventData) {
		if (OnMouseUp != null){
			OnMouseUp();
		}	
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (OnMouseDown != null){
			OnMouseDown();
		}	
	}

}
