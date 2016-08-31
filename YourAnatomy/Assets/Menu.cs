using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public  class Menu : MonoBehaviour {

  
    public RectTransform HeadSprite;
    public RectTransform HandLeftSprite;
    public RectTransform MouseSelector;
    public RectTransform HandRightSprite;
    public RawImage Background;
    public GameObject MenuOverlay;
    public RawImage MenuOverlayImage;
    public GameObject FullOverlay;
    public RawImage FullOverlayImage;
    public GameObject CutoutOverlay;
    public RawImage FantasyOverlayUsersImage;
    public Text UIText;
    public Text CornerUIText;
    public AudioSource Shutter;

	private Texture2D UsersCutout;
    public Texture2D Rainforest;
    private Dictionary<int, RectTransform> BodySpritesDictionary;
    private Color FadedUserImageColor;

	// the Kinect manager
	private KinectManager kinectManager;
    public float Alpha=    .5f;
    public float FADETIME;
    public float UserImageColorPeriod = 2;
    public float UserImageColorPeriodOffset = 1;
    public static float SkeletonDialValue = 3;
    public static float SkinDialValue = 3;
    private SpeechManager speechManager;
    private bool MenuStarted = false;
    public bool FullVideoOverlay = false;
    public int CaptureWaitTime = 5;
    private Vector2 BodySpriteSpotDefaultPos = new Vector2(600, 350);
    public bool Fade = true;
    private float MenuStartTime;
    private float LifeSpan = 5;

	void Start () 
	{
       
        if(FullVideoOverlay) FullOverlay.SetActive(true);

        CanvasEvents.OnStartMenu += StartMenu;
        CanvasEvents.OnSliderChange += SliderChangedValue;
        CanvasEvents.OnChooserPicture += PictureButtonEnter;
        CanvasEvents.OnExitMenu += ExitMenu;
        CanvasEvents.OnMenuLifeSpanChange += LifeSpanChanged;
        kinectManager = KinectManager.Instance;
        FadedUserImageColor = FullOverlayImage.color;

		if(kinectManager && kinectManager.IsInitialized())
		{              
			KinectInterop.SensorData sensorData = kinectManager.GetSensorData();
		}

        BodySpritesDictionary = new Dictionary<int, RectTransform>() {{ (int)KinectInterop.JointType.Head, HeadSprite },{(int)KinectInterop.JointType.HandRight, HandRightSprite}, {(int)KinectInterop.JointType.HandLeft, HandLeftSprite}};
        FadeImageOnCanvas(0, 1);
    }

    private void LifeSpanChanged(float value)
    {
        MenuStartTime = Time.time;
        LifeSpan = value;
    }

    private void PictureButtonEnter(int value,  RectTransform panel)
    {
      
        var chooserPic = panel.gameObject.GetComponent<ChooserPictureBehavior>();
        var panelType = chooserPic.ThisPanelType;
        if (panelType == PanelType.Rainforest||panelType == PanelType.Gallery)
        {
            FullVideoOverlay = false;
            
            Background.texture = chooserPic.Picture;
        }
        else if(panelType == PanelType.Capture)
        {
           
            CaptureWaitTime = value;
            StartCapture();
        }
        else if (panelType == PanelType.Calaca) { 
        
        }
        else if (panelType == PanelType.Anatomy) {
        
        }
    }

    private void StartCapture()
    {
        //FullVideoOverlay = true;
        if (Background.gameObject.activeSelf == false) Background.gameObject.SetActive(true);
        Background.texture = kinectManager.GetUsersClrTex();
        CanvasEvents.OnExitMenu();
        StartCoroutine(Capture(CaptureWaitTime));
        StartCoroutine(CaptureCountdown(CaptureWaitTime));
    }

    IEnumerator Capture(int captureWaitTime)
    {

        yield return new WaitForSeconds(captureWaitTime);

        if (kinectManager && kinectManager.IsInitialized())
        {
            //print("capturing");

            Texture2D grab = new Texture2D(kinectManager.GetUsersClrTex().width, kinectManager.GetUsersClrTex().height, TextureFormat.RGBA32, false);
            grab.LoadRawTextureData(kinectManager.GetSensorData().colorImage);
            grab.Apply();
            Background.texture = grab;
        }

    }
    IEnumerator CaptureCountdown(int captureWaitTime)
    {        
        UIText.text = "Clear all players from view in " + captureWaitTime + " seconds";
        yield return new WaitForSeconds(1);
        captureWaitTime -= 1;
        if (captureWaitTime > 0)
        {
            UIText.text = "Clear all players from view in " + (captureWaitTime) + " seconds";
            yield return new WaitForSeconds(1);
            captureWaitTime -= 1;
            if (captureWaitTime > 0)
            {
                UIText.text = "Clear all players from view in " + (captureWaitTime) + " seconds";
                yield return new WaitForSeconds(1);
                captureWaitTime -= 1;

                if (captureWaitTime > 0)
                {
                      UIText.text = "Clear all players from view in " + (captureWaitTime) + " seconds";
                    yield return new WaitForSeconds(1);
                    captureWaitTime -= 1;
                }
            }
           
            Shutter.Play();
        }

        UIText.text = "";


        yield return null;
    }
    private void SliderChangedValue(int value, RectTransform panel)
    {
       // LifeSpan += Time.deltaTime;
       var panelType = panel.gameObject.GetComponent<SliderBehavior>().ThisPanelType;
       if (panelType == PanelType.Skin) { SkinDialValue = value; Fade = true; }
       else if (panelType == PanelType.Skeleton) { SkeletonDialValue = value; Fade = true; }
       else if (panelType == PanelType.Transparency)
       {
          
           if (Fade)
           { 
               Fade = false;
             
       }
           Alpha = (float)value / (float)panel.GetComponent<SliderBehavior>().SLIDERMAX;
           FadeImageOnCanvas(0, 1);
       }
    }
	
	void Update () 
	{
        if (LifeSpan + MenuStartTime < Time.time) ExitMenu();
       // if (speechManager == null) speechManager = SpeechManager.Instance;
     //   if(speechManager && speechManager.IsSapiInitialized())
     /*   {
            if(speechManager.IsPhraseRecognized())
            {
                string phraseTag = speechManager.GetPhraseTagRecognized();

                switch(phraseTag)
                {
                    case "JELLO":
                        print("HELLO");
                        break;
                    case "MENU":
                        CanvasEvents.OnStartMenu();
                        StartMenu();
                        break;
                    case "EXIT":
                        
                        CanvasEvents.OnExitMenu();
                        break;
                    case "RAINFOREST":
                        ReplaceBackground(Rainforest);
                        break;
                    case "CAPTURE":
                        StartCapture();
                        break;
                }
                speechManager.ClearPhraseRecognized();
            }
        }*/
        MouseSelector.anchoredPosition = new Vector2((Input.mousePosition.x/Screen.width)*1920, 1080-(Input.mousePosition.y/Screen.height)*1080);
        if (kinectManager && kinectManager.IsInitialized())
        {
            Texture2D kinectColorTex = kinectManager.GetUsersClrTex();
            if (FullOverlay)
            {
                FullOverlayImage.texture = kinectColorTex;
                FullOverlayImage.color = FadedUserImageColor;
            }
             if (MenuOverlay.activeSelf == true)
            {
                UsersCutout = kinectManager.GetUsersLblTex();
                MenuOverlayImage.texture = UsersCutout;
                 
                MenuOverlayImage.color = FadedUserImageColor;
            }
            if(!FullVideoOverlay)
            {
                UsersCutout = kinectManager.GetUsersLblTex();
                FantasyOverlayUsersImage.texture = UsersCutout;
                FantasyOverlayUsersImage.color = FadedUserImageColor;
            }

            if (Input.GetMouseButtonDown(0))
                if (MenuStarted&&CanvasEvents.OnExitMenu!=null) CanvasEvents.OnExitMenu();
                else if(CanvasEvents.OnStartMenu!=null) CanvasEvents.OnStartMenu();

            long UserID = kinectManager.GetPrimaryUserID();
         foreach (var d in BodySpritesDictionary)
            {
                int iJoint = d.Key;
                if (kinectManager.GetJointTrackingState(UserID, iJoint) == KinectInterop.TrackingState.Tracked)
                {
                    var posKinect = kinectManager.GetJointKinectPosition(UserID, iJoint);
                    Vector2 posDepth = kinectManager.MapSpacePointToDepthCoords(posKinect);
                    ushort depthValue = kinectManager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);
                    Vector2 posColor = kinectManager.MapDepthPointToColorCoords(posDepth, depthValue);
                    if (posColor.x > 0 && posColor.x < 2000 && posColor.y > 0 && posColor.y < 1200)
                    {
                        d.Value.anchoredPosition = posColor;
                    }
                }
                else
                {
                    d.Value.anchoredPosition = BodySpriteSpotDefaultPos;
                }
             }  
        }
       
      if(Fade)  FadeUserImage();


	}

    private void ReplaceBackground(Texture2D newImage)
    {
        Background.texture = newImage;
    }

  

    private void StartMenu()
    {
        if (!MenuStarted)
        {
            MenuStartTime = Time.time;
            LifeSpan = 5;
            MenuStarted = true;
            MenuOverlay.SetActive(true);
            FullOverlay.SetActive(false);
            CutoutOverlay.SetActive(false);
            CornerUIText.text = ("Mouseclick to exit Menu");
        }
    }

  private void ExitMenu()
    {
        if (MenuStarted)
        {
            MenuStarted = false;
            MenuOverlay.SetActive(false);
            FullVideoOverlay = false;
            if (FullVideoOverlay)
            {
                FullOverlay.SetActive(true);
            }
            else
            {
                CutoutOverlay.SetActive(true);
            }
            CornerUIText.text = ("Mouseclick for Menu");
        }
    }

    private void FadeUserImage()
    {
        float periodOff = SkeletonDialValue; // + UserImageColorPeriodOffset ;
        float period = UserImageColorPeriod + (SkinDialValue-2);
        float fadeTime = FADETIME + period / 3 + periodOff / 3;
        Alpha -= 1 / fadeTime * Time.deltaTime;
        Alpha = Wrap(Alpha);

        FadeImageOnCanvas(periodOff, period);
       
        
    }

    private void FadeImageOnCanvas(float periodOff, float period)
    {
        FadedUserImageColor.a = (Mathf.Abs((period + periodOff) * Alpha) - periodOff);
    }

    float Wrap(float value)
    {
        if (value < -1)
        {
            value = 1;
        }
        return value;
    }

    //public void PanelSelected(RectTransform panel)
    //{
    //    print("Panel Seletced!!!");
    //}



    public enum PanelType
    {
        Skeleton,
        Skin,
        Transparency,
        Rainforest,
        Gallery,
        Capture,
        Calaca,
        Anatomy,
        Xray,
        Organless
    }
    public enum PanelState
    {
        Empty = 0,
        Inside = 1,
        Stay,
        Leaving,
    }
    public enum LifeSpans
    {
        Min = 10,
        Max = 20
    }
}


