using UnityEngine;
using System.Collections;

public class SingletonsInitializer : MonoBehaviour {

	void Awake () {
		Singletons.InitializeServicesReferences();
	}
}
