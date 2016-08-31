using UnityEngine;
using System.Collections;

public class UIFollowAvatar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        KinectManager kinectManager = KinectManager.Instance;
        //posJoint is the space position of the joint
        if (!kinectManager || !kinectManager.IsUserDetected()) return;
        long key = kinectManager.GetPrimaryUserID();
        Vector3 posJoint = kinectManager.GetJointKinectPosition(key, (int)KinectInterop.JointType.SpineShoulder);

        // posDepth is the position of the posjoint in the depthmap
        Vector2 posDepth = kinectManager.MapSpacePointToDepthCoords(posJoint);
        ushort depthValue = kinectManager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

        // posColor is the position of the posDepth in the color map
        Vector2 posColor = kinectManager.MapDepthPointToColorCoords(posDepth, depthValue);
        float xNorm = (float)posColor.x / kinectManager.GetColorImageWidth();
        float yNorm = 1.0f - (float)posColor.y / kinectManager.GetColorImageHeight();
        Vector3 jointPlace = new Vector3(Mathf.Min(10000,Mathf.Max(0,xNorm)), Mathf.Min(10000,Mathf.Max(0,yNorm)), posJoint.z);

        this.transform.position= posColor;
	}
}
