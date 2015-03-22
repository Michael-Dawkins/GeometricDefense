using UnityEngine;
using System.Collections;

public class DeleteParentOnLeave : MonoBehaviour {

	bool hasAppeared = false;

	// Update is called once per frame
	void Update () {
		if (!GetComponent<Renderer>().isVisible && hasAppeared){
			Destroy(gameObject.transform.parent.gameObject);
		} else {
			hasAppeared = true;
		}
	}
}
