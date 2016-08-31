using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PeriodicFade : MonoBehaviour {
	float Alpha = 1;
	Color color;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Alpha <-1 ){
			Alpha = 1;}

			Alpha -=.01f;
		color = this.GetComponent <RawImage> ().color;
		color.a = Mathf.Abs(Alpha);
		this.GetComponent <RawImage> ().color = color;
		//print (color.a);
	}
}
