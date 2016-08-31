using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// starting as a variation of KinectOverlayDemo. Match gameObjects to screen image
/// </summary>
/// 

namespace YourAnatomyProject
{
    public class YourAnatomy : MonoBehaviour 
    {
        //public float UserFadeDistance = 1.0f;
        //public float fadeSpeed = 1 ;
        public bool RearProjection = true;
        public float HEADSIZE = 0.5f;
        private float TORSOWIDTH =1f;
        public float MAXBONESCALE = 2f;
        public float HIPWIDTH = 2f;
        public float MINNECKPROPORTION = 2.5f;
        public float SHOULDERWIDTH =2;
        private float boneScaleFactor = 1f;
        public float Alpha = 0.5f;
        private float MINBONESCALE = 0.3f;
        private Transform SkeletonFBX;
        public Transform SkeletonFBXRearProjection;
        public Transform SkeletonFBXMirrored;
        public Transform LaCalacaLoca;
        public Transform XRaySkeleton;
        public Transform OrganlessSkeleton;
        public Transform VideoMeshSkeleton;
       // public Canvas OverlayCanvas;


       // public RawImage rawImage;
        public ArtStyle artStyle;
        public float heartRate = 1.88f;// beats per second
        public float breathingRate = 1.5f;//breaths per second
        private Transform handTipRight;
        private Transform handRight;
        private Transform elbowRight;
        private Transform shoulderRight;
        private Transform head;
        private Transform neck;
        private Transform handTipLeft;
        private Transform handLeft;
        private Transform elbowLeft;
        private Transform shoulderLeft;
        private Transform spineShoulder;
        private Transform spineMid;
        private Transform spineBase;
        private Transform hipRight;
        private Transform kneeRight;
        private Transform footRight;
        private Transform hipLeft;
        private Transform kneeLeft;
        private Transform footLeft;
        public GameObject arena;
        public float smoothFactor = 20f;
        //private float distanceToCamera = 10f;
        private List<OverlayObject> OverlayObjectsExample; 
        private float ArenaZ;
        public float ArenaDepth;
        private Color color = Color.black;
        private List<long> userIDs = new List<long>();
        private Dictionary<long, List<OverlayObject>> bagOfBonesDictionary;
        private Dictionary<long, List<Transform>> ChestOrgansBehaviorDictionary;

        public enum ArtStyle
        {
            Anatomy,
            CalacaLoca,
            XRay,
            Organless
        }

        void Start()
        {
            CanvasEvents.OnChooserPicture += pictureButtonEnter;
            if (artStyle == ArtStyle.Anatomy)
            {
                if (RearProjection) SkeletonFBX = SkeletonFBXRearProjection;
                else SkeletonFBX = SkeletonFBXMirrored;
            }
            else if (artStyle ==ArtStyle.CalacaLoca)
            {
                SkeletonFBX = LaCalacaLoca;
            }else if(artStyle == ArtStyle.XRay)
            {
                SkeletonFBX = XRaySkeleton;
            }
            handTipRight = SkeletonFBX.FindChild("RightHandTip");
            handRight = SkeletonFBX.FindChild("RightHand");
            elbowRight = SkeletonFBX.FindChild("RightElbow");
            shoulderRight = SkeletonFBX.FindChild("RightShoulder");
            head = SkeletonFBX.FindChild("Head");
            neck = SkeletonFBX.FindChild("Neck");
            handTipLeft = SkeletonFBX.FindChild("LeftHandTip");
            handLeft = SkeletonFBX.FindChild("LeftHand");
            elbowLeft = SkeletonFBX.FindChild("LeftElbow");
            shoulderLeft = SkeletonFBX.FindChild("LeftShoulder");
            spineShoulder = SkeletonFBX.FindChild("SpineShoulder");
            spineMid = SkeletonFBX.FindChild("SpineMid");
            spineBase = SkeletonFBX.FindChild("SpineBase");
            hipRight = SkeletonFBX.FindChild("RightHip");
            kneeRight = SkeletonFBX.FindChild("RightKnee");
            footRight = SkeletonFBX.FindChild("RightFoot");
            hipLeft = SkeletonFBX.FindChild("LeftHip");
            kneeLeft = SkeletonFBX.FindChild("LeftKnee");
            footLeft = SkeletonFBX.FindChild("LeftFoot");
            OverlayObject handTipRightObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HandTipRight, BoneGameObject = handTipRight };
                OverlayObject handRightObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.WristRight, BoneGameObject = handRight, InitialRotation = handRight.transform.rotation, ReferenceGameObjectName = handTipRight };
                OverlayObject elbowRightObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ElbowRight, BoneGameObject = elbowRight, InitialRotation = elbowRight.transform.rotation, ReferenceGameObjectName = handRight };
                OverlayObject shoulderRightObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ShoulderRight, BoneGameObject = shoulderRight, InitialRotation = shoulderRight.transform.rotation, ReferenceGameObjectName = elbowRight };
                OverlayObject hipRightObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HipRight, BoneGameObject = hipRight, ReferenceGameObjectName = kneeRight };
                OverlayObject kneeRightObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.KneeRight, BoneGameObject = kneeRight, ReferenceGameObjectName = footRight };
                OverlayObject footRightObject;// = new OverlayObject (){TrackedJointType = KinectInterop.JointType.FootRight, BoneGameObject = footRight, ReferenceGameObjectName = kneeRight};
                OverlayObject hipLeftObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HipLeft, BoneGameObject = hipLeft, ReferenceGameObjectName = kneeLeft };
                OverlayObject kneeLeftObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.KneeLeft, BoneGameObject = kneeLeft, ReferenceGameObjectName = footLeft };
                OverlayObject footLeftObject;// = new OverlayObject () { TrackedJointType = KinectInterop.JointType.FootLeft, BoneGameObject = footLeft, ReferenceGameObjectName = kneeLeft};
                OverlayObject handTipLeftObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HandTipLeft, BoneGameObject = handTipLeft };
                OverlayObject handLeftObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.WristLeft, BoneGameObject = handLeft, InitialRotation = handLeft.transform.rotation, ReferenceGameObjectName = handTipLeft };
                OverlayObject elbowLeftObject;// = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ElbowLeft, BoneGameObject = elbowLeft, InitialRotation = elbowLeft.transform.rotation, ReferenceGameObjectName = handLeft };
                OverlayObject shoulderLeftObject;
            if (RearProjection)
            {
                 handTipRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HandTipRight, BoneGameObject = handTipRight };
                 handRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.WristRight, BoneGameObject = handRight, InitialRotation = handRight.transform.rotation, ReferenceGameObjectTransform = handTipRight };
                 elbowRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ElbowRight, BoneGameObject = elbowRight, InitialRotation = elbowRight.transform.rotation, ReferenceGameObjectTransform = handRight };
                 shoulderRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ShoulderRight, BoneGameObject = shoulderRight, InitialRotation = shoulderRight.transform.rotation, ReferenceGameObjectTransform = elbowRight };
                 hipRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HipRight, BoneGameObject = hipRight, ReferenceGameObjectTransform = kneeRight };
                 kneeRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.KneeRight, BoneGameObject = kneeRight, ReferenceGameObjectTransform = footRight };
                 footRightObject = new OverlayObject (){TrackedJointType = KinectInterop.JointType.FootRight, BoneGameObject = footRight, ReferenceGameObjectTransform = kneeRight};
                 hipLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HipLeft, BoneGameObject = hipLeft, ReferenceGameObjectTransform = kneeLeft };
                 kneeLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.KneeLeft, BoneGameObject = kneeLeft, ReferenceGameObjectTransform = footLeft };
                 footLeftObject = new OverlayObject () { TrackedJointType = KinectInterop.JointType.FootLeft, BoneGameObject = footLeft, ReferenceGameObjectTransform = kneeLeft};
                 handTipLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HandTipLeft, BoneGameObject = handTipLeft };
                 handLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.WristLeft, BoneGameObject = handLeft, InitialRotation = handLeft.transform.rotation, ReferenceGameObjectTransform = handTipLeft };
                 elbowLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ElbowLeft, BoneGameObject = elbowLeft, InitialRotation = elbowLeft.transform.rotation, ReferenceGameObjectTransform = handLeft };
                 shoulderLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ShoulderLeft, BoneGameObject = shoulderLeft, InitialRotation = shoulderLeft.transform.rotation, ReferenceGameObjectTransform = elbowLeft };
                                   
            }
            else // non-rear projection is mirrored, so reverse the left and right game objects' respective joint assignments.
            { 
                 handTipRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HandTipRight, BoneGameObject = handTipLeft };
                 handRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.WristRight, BoneGameObject = handLeft, InitialRotation = handLeft.transform.rotation, ReferenceGameObjectTransform = handTipLeft };
                 elbowRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ElbowRight, BoneGameObject = elbowLeft, InitialRotation = elbowLeft.transform.rotation, ReferenceGameObjectTransform = handLeft };
                 shoulderRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ShoulderRight, BoneGameObject = shoulderLeft, InitialRotation = shoulderLeft.transform.rotation, ReferenceGameObjectTransform = elbowLeft };
                 hipRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HipRight, BoneGameObject = hipLeft, ReferenceGameObjectTransform = kneeLeft };
                 kneeRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.KneeRight, BoneGameObject = kneeLeft, ReferenceGameObjectTransform = footLeft };
                 footRightObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.FootRight, BoneGameObject = footLeft, ReferenceGameObjectTransform = kneeLeft };
                 hipLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HipLeft, BoneGameObject = hipRight, ReferenceGameObjectTransform = kneeRight };
                 kneeLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.KneeLeft, BoneGameObject = kneeRight, ReferenceGameObjectTransform = footRight };
                 footLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.FootLeft, BoneGameObject = footRight, ReferenceGameObjectTransform = kneeRight };
                 handTipLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.HandTipLeft, BoneGameObject = handTipRight };
                 handLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.WristLeft, BoneGameObject = handRight, InitialRotation = handRight.transform.rotation, ReferenceGameObjectTransform = handTipRight };
                 elbowLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ElbowLeft, BoneGameObject = elbowRight, InitialRotation = elbowRight.transform.rotation, ReferenceGameObjectTransform = handRight };
                 shoulderLeftObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.ShoulderLeft, BoneGameObject = shoulderRight, InitialRotation = shoulderRight.transform.rotation, ReferenceGameObjectTransform = elbowRight };

            }
            OverlayObject headObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.Head, BoneGameObject = head, InitialRotation = head.transform.rotation, ReferenceGameObjectTransform = spineBase };
            OverlayObject neckObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.Neck, BoneGameObject = neck, InitialRotation = neck.transform.rotation, ReferenceGameObjectTransform = head };
            OverlayObject spineShoulderObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.SpineShoulder, BoneGameObject = spineShoulder, ReferenceGameObjectTransform = neck };
            OverlayObject spineMidObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.SpineMid, BoneGameObject = spineMid, ReferenceGameObjectTransform = spineShoulder };
            OverlayObject spineBaseObject = new OverlayObject() { TrackedJointType = KinectInterop.JointType.SpineBase, BoneGameObject = spineBase, ReferenceGameObjectTransform = spineMid };
 
           
            OverlayObjectsExample = new List<OverlayObject>() { handTipRightObject, handRightObject, elbowRightObject, shoulderRightObject, headObject, neckObject, handTipLeftObject, handLeftObject, elbowLeftObject, shoulderLeftObject, spineShoulderObject, spineMidObject, spineBaseObject, hipRightObject, kneeRightObject, hipLeftObject, kneeLeftObject, footLeftObject,footRightObject };
            bagOfBonesDictionary = new Dictionary<long, List<OverlayObject>>();
            ChestOrgansBehaviorDictionary = new Dictionary<long, List<Transform>>();
	        ArenaZ = arena.transform.lossyScale.z;
           // print("bag of bpnes initialized");
            foreach (OverlayObject oo in OverlayObjectsExample)
            {              
                oo.BoneGameObject.transform.rotation = Quaternion.identity;
                if (oo.ReferenceGameObjectTransform != null) oo.InitialBoneLength = BoneLength(oo, oo.ReferenceGameObjectTransform.position);
                else oo.InitialBoneLength = 1;
            }
    
        }

        private void pictureButtonEnter(int number, RectTransform panel)
        {
            var panelType = panel.gameObject.GetComponent<ChooserPictureBehavior>().ThisPanelType;
            if (panelType == Menu.PanelType.Calaca)
            {
               ChangeSkeleton(ArtStyle.CalacaLoca);
            }
            else if(panelType == Menu.PanelType.Anatomy)
            {
                ChangeSkeleton(ArtStyle.Anatomy);
            }
            else if (panelType == Menu.PanelType.Organless)
            {
                ChangeSkeleton(ArtStyle.Organless);
            }
            else if(panelType == Menu.PanelType.Xray)
            {
                ChangeSkeleton(ArtStyle.XRay);
            }
        }

        public float heartBeat = 0; 
        public float breath = 0;
        public float diaphragmScaleMultiply = 0.2f;
        public float breathScaleMin = 0.0f;
        public float breathScaleMax = 3.0f;
        public float diaphragmScaleAdd = 0.9f;
        private float MINTORSOLENGTH = 0.4f; // length of child's torso in meters
        private float TORSOLENGTHRANGE = 0.4f;
        public float JAWROTATEMULIPLIER = 10f;
        public float VALUESHOULDER = 0.8f;
        public float RIBSWIDTH = 1f;
        private int captureFileIncrement = 0;
        public bool CaptureScreenshot;
        private float diaphragmOffset = 0.5f;
        private float diaphragmMove = 0.5f;
        public float FADETIME;
        internal enum ChestOrgansEnum : int
        {
        HeartBeat = 0,
        HeartSys = 1,
        DiaphragmUp = 2,
        DiaphragmOut = 3
        }

        void Update()
        {           

            KinectManager kinectManager = KinectManager.Instance;           
            FacetrackingManager facetrackingManager = FacetrackingManager.Instance;
            
            kinectManager.computeColorMap = true;
            if (kinectManager && kinectManager.IsInitialized())
            {
                // get the kinect video stream
                //Texture2D kinectColorTex = kinectManager.GetUsersClrTex();         


                RemoveSkeletonsForUnusedIDs(kinectManager);

                // adding userIDs to the USerID list
                if (kinectManager.IsUserDetected())
                {
                   for (int i = 0; i < kinectManager.GetUsersCount(); i++)
                    {
                        bool found = false;
                        long ID = kinectManager.GetUserIdByIndex(i);
                        for (int j = 0; j < userIDs.Count; j++)
                        {
                            if (ID == userIDs[j])
                            { found = true; }
                        }
                        if (!found)
                        {
                            userIDs.Add(ID);
                          
                        }
                    }
            

                   // add bones for any new users
                        for (int i = 0; i < userIDs.Count; i++)
                        {
                            long id = userIDs[i];
                            if (!bagOfBonesDictionary.ContainsKey(id) || !ChestOrgansBehaviorDictionary.ContainsKey(id)) // for each new user make a boneslist associated with his or her id.
                            {
                                if (bagOfBonesDictionary.ContainsKey(id)) bagOfBonesDictionary.Remove(id); // if the chestorgans dictionary did not have a list for this user, but the bones dictionary did, remove the bonesdictionary list to start with a new bones list, so that both dictionaries use child objects of the same skeleton.
                                List<OverlayObject> newUserBonesList = new List<OverlayObject>();
                              Transform thisUserSkeleton;
                              thisUserSkeleton = (Transform)Instantiate(SkeletonFBX);
                                CreateUserSkeleton(thisUserSkeleton, id);
                                foreach (OverlayObject oo in OverlayObjectsExample)
                                {
                                    foreach (Transform child in thisUserSkeleton)
                                    {
                                        if (oo.BoneGameObject.gameObject.name == child.gameObject.name) // this user's skeleton bone matches bone in example bonelist
                                        {                                            
                                            OverlayObject boneOverlayObject = new OverlayObject() { TrackedJointType = oo.TrackedJointType, BoneGameObject = child, InitialBoneLength = oo.InitialBoneLength }; // uses initial values from example bones
                                            if (oo.ReferenceGameObjectTransform != null) { boneOverlayObject.ReferenceGameObjectTransform = FindReferenceJointInThisSkeleton(oo.ReferenceGameObjectTransform.gameObject.name, thisUserSkeleton.transform); }
                                            oo.InitialRotation = oo.BoneGameObject.transform.rotation;
                                            oo.BoneGameObject.transform.localScale = new Vector3(1, 1, 1);
                                            newUserBonesList.Add(boneOverlayObject);
                                        }
                                    } 
                                }
                               bagOfBonesDictionary.Add(id, newUserBonesList);
                               if (ChestOrgansBehaviorDictionary.ContainsKey(id)) ChestOrgansBehaviorDictionary.Remove(id);
                               List<Transform> newUserChestOrgansList = new List<Transform>();
                               Transform spineMid = thisUserSkeleton.FindChild("SpineMid");
                               Transform heartBox1 = spineMid.FindChild("HeartBox1");
                               newUserChestOrgansList.Insert((int)ChestOrgansEnum.HeartBeat,heartBox1);
                               Transform heartBox2 = spineMid.FindChild("HeartBox2");
                               newUserChestOrgansList.Insert((int)ChestOrgansEnum.HeartSys, heartBox2);
                               Transform diaphragmBoneMid = spineMid.FindChild("Diaphragm Bone Mid");
                               newUserChestOrgansList.Insert((int)ChestOrgansEnum.DiaphragmUp, diaphragmBoneMid);
                               Transform diaphragmBoneOut = spineMid.FindChild("Diaphragm Bone Out");
                               newUserChestOrgansList.Insert((int)ChestOrgansEnum.DiaphragmOut, diaphragmBoneOut);                            
                               ChestOrgansBehaviorDictionary.Add(id, newUserChestOrgansList);
                            }
                        }
                  var jp = TransformBones(kinectManager, facetrackingManager);
                  HeartAndLungTransform();
                }
            }
        }

        private void CreateUserSkeleton(Transform thisUserSkeleton, long id)
        {
         if(SceneEvents.SkeletonUserCreate != null)   SceneEvents.SkeletonUserCreate(thisUserSkeleton, id);
        }

        private void RemoveSkeletonsForUnusedIDs(KinectManager kinectManager)
        {
            // getting rid of unused skeletons
            for (int i = 0; i < userIDs.Count; i++)
            {
                bool found = false;
                long id = userIDs[i];
                for (int j = 0; j < kinectManager.GetUsersCount(); j++)
                {
                    if (id == kinectManager.GetUserIdByIndex(j))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    destroySkeletonandID(id);
                }
            }
        }

        private void destroySkeletonandID(long id)
        {
            //	print ("removing " + id);
            if (bagOfBonesDictionary.ContainsKey(id))
            {
                Transform skeleton = bagOfBonesDictionary[id][0].BoneGameObject.transform.parent;
                SceneEvents.SkeletonUserDestroy(skeleton, id);
                Destroy(skeleton.gameObject);
                bagOfBonesDictionary.Remove(id);
            }
            userIDs.Remove(id);
        }

        //private void FadeUserImage()
        //{           
        //    Alpha -= FADETIME * Time.deltaTime;
        //    Alpha = Wrap(Alpha);

        //    color.a = Mathf.Abs(2 * Alpha) - 0.5f;           
        //}

        private float Offset(float heartoff)
        {
            heartoff = Mathf.Abs(heartoff) -1;
            return heartoff;
        }

        private Vector3 TransformBones(KinectManager kinectManager, FacetrackingManager facetrackingManager)
        {
          
            Vector3 jp = Vector3.zero;
            for (int i = 0; i < userIDs.Count; i ++)   // do this for every USer ID in 
            {
                List<OverlayObject> bonesList;
                long userID = userIDs[i];
                if(bagOfBonesDictionary.ContainsKey(userID))
                {
                    bonesList = bagOfBonesDictionary[userID];
                foreach (OverlayObject oo in bonesList)
                {
                    
                    int iJointIndex = (int)oo.TrackedJointType;
                    
                 
                        if (kinectManager.IsJointTracked(userID, iJointIndex) )
                        {                           
                                var TryPos = TranslateBone(oo, userID, kinectManager);
                                if (TryPos !=Vector3.zero) oo.BoneGameObject.position = TryPos;

                            oo.BoneGameObject.rotation = RotateBone(oo, kinectManager, facetrackingManager, userID); //Quaternion.Slerp(oo.BoneGameObject.transform.rotation, qRotObject, smoothFactor * Time.deltaTime);

                            if (oo.ReferenceGameObjectTransform != null)
                            {
                                oo.BoneGameObject.localScale = ScaleBone(kinectManager, userID, oo, iJointIndex);
                            }
                            else { oo.BoneGameObject.localScale = new Vector3(1, 1, 1); }
                        }
                   
                    else // joint not tracked
                    {
                        oo.BoneGameObject.position = oo.BoneGameObject.position;
                        oo.BoneGameObject.localRotation = oo.BoneGameObject.localRotation;
                        oo.BoneGameObject.localScale = oo.BoneGameObject.localScale;
                    }
                }
                }
                else { print("Bag of Bones is Missing userID"); }

            }
            return jp;
        }
        public Vector3 ScaleBone(KinectManager kinectManager, long key, OverlayObject oo, int iJointIndex)
        {

            float boneScale;
            if (iJointIndex == 3)////special scale for head
            {
                boneScale = ScaleHead(kinectManager, key, oo, iJointIndex);

            }
            else if (iJointIndex >= 0 && iJointIndex <= 2 || iJointIndex == 20) /// special scale for torso & neck
            {
                float torsoScale = ScaleTorso(kinectManager, key, iJointIndex);
                float torsoScaleY = LimbBoneScale(oo);
                torsoScale *= torsoScaleY;
                return new Vector3(torsoScale, torsoScaleY, torsoScale);
            }
            else if (iJointIndex == (int)KinectInterop.JointType.FootLeft || iJointIndex == (int)KinectInterop.JointType.FootRight)
            {
                boneScale = FootBoneScale(oo);
            }
            else // the rest should be limbs
            {
                boneScale = LimbBoneScale(oo);
            }
            Vector3 scaleBone = new Vector3(boneScale, boneScale, boneScale);

            return scaleBone;
        }

        private float ScaleHead(KinectManager kinectManager, long key, OverlayObject oo, int iJointIndex)
        {
            float boneScale = AdultTorsoQuotient(kinectManager, key); // how much of this torso height is adult height 1 = adult 0 = child
            boneScale *= -0.16666f;
            boneScale += 0.5f;
            float boneLength = BoneLength(oo, oo.ReferenceGameObjectTransform.position);
            boneScale *= boneLength;
            boneScale *= HEADSIZE;
            return boneScale;
        }

        private void HeartAndLungTransform(){
            List<Transform> chestOrgans;
            for(int i = 0; i< userIDs.Count; i++){
                
                long userID = userIDs[i];
                if (ChestOrgansBehaviorDictionary.ContainsKey(userID))
                {
                    chestOrgans = ChestOrgansBehaviorDictionary[userID];
                    heartBeat -= (Time.deltaTime * heartRate * 2);
                    heartBeat = Wrap(heartBeat);
                    float heartScale = Mathf.Max(0.1f, Mathf.Abs(heartBeat) + 1.6f) * .4f;
                    chestOrgans[(int)ChestOrgansEnum.HeartBeat].localScale = new Vector3(heartScale, heartScale, heartScale);
                    breath -= Time.deltaTime * breathingRate * 2;
                    breath = Wrap(breath); 
                    float absBreath = Mathf.Abs(breath)* Mathf.Abs(breath) *(3f-2*Mathf.Abs(breath));
                    Transform diaphragmUp = chestOrgans[(int)ChestOrgansEnum.DiaphragmUp];
                    //var diaphragmEndPos = new Vector3(diaphragmUp.localPosition.x, , diaphragmUp.localPosition.z);
                    diaphragmUp.localPosition = new Vector3(diaphragmUp.localPosition.x,  diaphragmOffset-diaphragmMove*absBreath, diaphragmUp.localPosition.z);
                    float breathScale = Mathf.Max(breathScaleMin, Mathf.Min(breathScaleMax, absBreath * diaphragmScaleMultiply + diaphragmScaleAdd));
                    Transform diaphragmOut = chestOrgans[(int)ChestOrgansEnum.DiaphragmOut];
                    diaphragmOut.localScale = new Vector3(breathScale , breathScale , 1 );
                    float heartsys = Offset(heartBeat);
                    heartScale = Mathf.Max(0.1f, (Mathf.Abs(heartsys) + 1.6f) * .3f);
                    chestOrgans[(int)ChestOrgansEnum.HeartSys].localScale = new Vector3(heartScale, heartScale, heartScale);
                }
            }
        }

        private float AdultTorsoQuotient(KinectManager kinectManager, long key)
        {
            int spineBaseIndex = 0;
            int headIndex = 3;
            float torsoLength = (kinectManager.GetJointPosition(key, headIndex) - kinectManager.GetJointPosition(key, spineBaseIndex)).magnitude;
            float boneScale = torsoLength - MINTORSOLENGTH;
            boneScale /= TORSOLENGTHRANGE;
            return boneScale;
        }

        private float ScaleTorso(KinectManager kinectManager, long key, int iJointIndex)
        {
            int leftShoulderIndex = 4;
            int rightShoulderIndex = 8;
            int leftHipindex = 12;
            int rightHipIndex = 16;

            float shoulderWidth = (kinectManager.GetJointPosition(key, rightShoulderIndex) - kinectManager.GetJointPosition(key, leftShoulderIndex)).magnitude*SHOULDERWIDTH;
            float hipWidth = (kinectManager.GetJointPosition(key, rightHipIndex) - kinectManager.GetJointPosition(key, leftHipindex)).magnitude*HIPWIDTH;
            float torsoSize = AdultTorsoQuotient(kinectManager, key); // how much of this torso height is adult height 1 = adult 0 = child

            if (iJointIndex == 2)//neck
            {
                torsoSize *= -0.16f;
                torsoSize += MINNECKPROPORTION;
                torsoSize *= shoulderWidth;
            }
            else
            {
                torsoSize *= -0.24f;
                torsoSize += 1f;

                if (iJointIndex == 0)// spineBase
                { torsoSize *= hipWidth; }
                else if (iJointIndex == 1) // spinMid
                { torsoSize *= Average(shoulderWidth, hipWidth, VALUESHOULDER) * RIBSWIDTH; }
                else { torsoSize *= shoulderWidth; }// spinShoulder should be the only one left

            }
            torsoSize *= TORSOWIDTH;
            return torsoSize;
        }

        private float Average(float value1, float value2, float value1Favor)
        {
            value1Favor = Mathf.Max(0, Mathf.Min(1, value1Favor))*2;
            return (value1 * Mathf.Max(0, Mathf.Min(value1Favor),2) + value2 * Mathf.Max(0, Mathf.Min(value1Favor - 2,2) / 2));
        }
        
        private Vector3 TranslateBone(OverlayObject oo, long key, KinectManager kinectManager)
        {
            
            int iJointIndex = (int)oo.TrackedJointType;

            Vector3 jp = PlaceJoint(iJointIndex, key, kinectManager);
            Vector3 vPosObject = Vector3.zero;
            float xNorm = Mathf.Max(-10,Mathf.Min(jp.x,10)); // addded the cap to avoid the out of camera frustrum error that resulted from infinty in these values
            float yNorm = Mathf.Max(-10,Mathf.Min(jp.y,10));
            if (xNorm < -9.9f || xNorm > 9.9f) return Vector3.zero;
            float posJointZ = jp.z;
            // normalize to camera space
           Vector3 foo ;
               if(RearProjection)  foo = new Vector3(1 - xNorm, yNorm, (posJointZ / ArenaDepth) * ArenaZ - Camera.main.transform.position.z);
               else foo = new Vector3( xNorm, yNorm, (posJointZ / ArenaDepth) * ArenaZ - Camera.main.transform.position.z);
 
            vPosObject = Camera.main.ViewportToWorldPoint(foo);
               // print(foo);

 return Vector3.Lerp(oo.BoneGameObject.position, vPosObject, smoothFactor * Time.deltaTime);
            
        }


        private Quaternion RotateBone(OverlayObject oo, KinectManager kinectManager, FacetrackingManager facetrackingManager, long key)
        {
            int iJointIndex = (int)oo.TrackedJointType;
            Quaternion qRotObject = Quaternion.identity;
            if (iJointIndex == 3) // if it's the head
            {
                if (facetrackingManager) // && facetrackingManager.IsFaceTrackingInitialized())
                { 
                    qRotObject *= facetrackingManager.GetHeadRotation(key, bMirroredMovement: !RearProjection);                
                
                if (oo.BoneGameObject.FindChild("Jaw") != null)
                        {
                            Transform Jaw;
                            if (facetrackingManager.bGotAU && oo.BoneGameObject.FindChild("Jaw") != null)
                            {
                                  Jaw = oo.BoneGameObject.FindChild("Jaw");
                                  //  print("Jaw angle : " + facetrackingManager.dictAU[0] * JAWROTATEMULIPLIER);
                                  Jaw.localRotation = Quaternion.AngleAxis(facetrackingManager.dictAU[0] * JAWROTATEMULIPLIER, Vector3.right);
                            }
                        }
                }

            }
            else // all joints not the head
            {

                qRotObject = kinectManager.GetJointOrientation(key, iJointIndex, flip: RearProjection);
                Vector3 rotAngles = qRotObject.eulerAngles - oo.InitialRotation.eulerAngles;
                qRotObject = Quaternion.Euler(rotAngles);
            }
            return Quaternion.Slerp(oo.BoneGameObject.transform.rotation, qRotObject, smoothFactor * Time.deltaTime);
        }


        private float FootBoneScale(OverlayObject oo)
        {            
           return oo.ReferenceGameObjectTransform.localScale.y;           
        }

        private float LimbBoneScale(OverlayObject oo)
        {
          
            Vector3 endJointPos = oo.ReferenceGameObjectTransform.position;
            float boneLength = BoneLength(oo, endJointPos);
            float boneScale;
            boneScale = Mathf.Max(Mathf.Min(boneLength, oo.InitialBoneLength * MAXBONESCALE), oo.InitialBoneLength * MINBONESCALE) / oo.InitialBoneLength;
            return boneScale;
        }

        private float BoneLength(OverlayObject oo, Vector3 endJointPos)
        {
            float boneLength = (oo.BoneGameObject.position - endJointPos).magnitude * boneScaleFactor;
            return boneLength;
        }

        private Transform FindReferenceJointInThisSkeleton(string name, Transform skeleton)
        {
            Transform endJoint = null;

            if(endJoint = skeleton.FindChild(name))return endJoint;
            return endJoint;
            
        }

        public OverlayObject findParent(string name, List<OverlayObject> listofBones)
        {
            OverlayObject boneOO = new OverlayObject();
            foreach (OverlayObject oo in listofBones)
            {
                if (oo.BoneGameObject.gameObject.name == name)
                {//print (name);
                    boneOO = oo;
                }
            }
            return boneOO;
        }

        float Wrap(float value)
        {
            if (value < -1)
            {
                value = 1;
            }
            return value;
        }

        public Vector3 PlaceJoint(int iJointIndex, long key, KinectManager kinectManager)
        {
            //posJoint is the space position of the joint
           
            Vector3 posJoint = kinectManager.GetJointKinectPosition(key, iJointIndex);
         
            // posDepth is the position of the posjoint in the depthmap
            Vector2 posDepth = kinectManager.MapSpacePointToDepthCoords(posJoint);
            ushort depthValue = kinectManager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);
            
            // posColor is the position of the posDepth in the color map
            Vector2 posColor = kinectManager.MapDepthPointToColorCoords(posDepth, depthValue);
            float xNorm = (float)posColor.x / kinectManager.GetColorImageWidth();
            float yNorm = 1.0f - (float)posColor.y / kinectManager.GetColorImageHeight();
            Vector3 jointPlace = new Vector3(xNorm, yNorm, posJoint.z);
           
            return jointPlace;
        }

        public void ChangeSkeleton(ArtStyle newArtStyle)
        {
            print("changeSkeletons" + newArtStyle);
            if (artStyle == newArtStyle) return;
            artStyle = newArtStyle;
            if (artStyle == ArtStyle.Anatomy)
            {
                ReplaceExistingSkeletons(SkeletonFBXMirrored);
            }
            else if(artStyle == ArtStyle.Organless)
            { 
                ReplaceExistingSkeletons(OrganlessSkeleton);
            }
        }

        private void ReplaceExistingSkeletons(Transform newSkeleton)
        {
            SkeletonFBX = newSkeleton;

            for (int i = 0; i < userIDs.Count; i++)
            {
                long id = userIDs[i];
                destroySkeletonandID(id);
            }
        }
    }

    

    public class OverlayObject
    {
        // all the bones
        public KinectInterop.JointType TrackedJointType { get; set; }
        public Transform BoneGameObject { get; set; }
        public Quaternion InitialRotation;
        public Transform ReferenceGameObjectTransform { get; set; }
        public float InitialBoneLength { get; set; }
       
    }

    public class JointPlace
    {
        public float XNorm { get; set; }
        public float YNorm { get; set; }
        public Vector3 PosJoint { get; set; }
    }
}