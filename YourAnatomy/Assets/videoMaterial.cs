using UnityEngine;
using System.Collections;

public class videoMaterial : MonoBehaviour
{
    public Material material;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KinectManager kinectManager = KinectManager.Instance;
        material.mainTexture = kinectManager.GetUsersClrTex();
    }
}