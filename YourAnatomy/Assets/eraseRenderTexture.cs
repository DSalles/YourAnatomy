using UnityEngine;
using System.Collections;

public class eraseRenderTexture : MonoBehaviour {
    public RenderTexture rt;
	// Use this for initialization
	void Start () {
     //   rt = new RenderTexture(rt.width,rt.height,rt.depth,rt.format);
	}
	
	// Update is called once per frame
	void Update () {
      
      if((int)Time.time % 2 == 0)
      {  rt.antiAliasing = 2; }
      else
      {  rt.antiAliasing = 4; }
	}
}
