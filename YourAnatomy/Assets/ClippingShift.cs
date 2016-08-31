using UnityEngine;
using System.Collections;

public class ClippingShift : MonoBehaviour {

	// Use this for initializationcddc
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// mousewheel affects clipping

		this.GetComponent<Camera>().nearClipPlane += Input.GetAxis ("Mouse ScrollWheel");
	
	}
}
