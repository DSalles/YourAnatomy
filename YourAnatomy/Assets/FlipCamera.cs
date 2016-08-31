using UnityEngine;
using System.Collections;

public class FlipCamera : MonoBehaviour {
	public Camera flippedcamera;
	// Use this for initialization
	void Start () {
		Matrix4x4 mat = Camera.main.projectionMatrix;
		mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
		flippedcamera.projectionMatrix = mat;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
