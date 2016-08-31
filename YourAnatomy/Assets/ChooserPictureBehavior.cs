using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooserPictureBehavior : MonoBehaviour {
    public Menu.PanelState panelState = Menu.PanelState.Empty;
    public RectTransform ThisRectTransform;
    public float EnterTime;
    private float EnterStayTime = .5f;
    private float LastInsideTime = 0;
    public int Repeat = 1;
    public List<RectTransform> Selectors;
    public RevealManager revealManager;
    public Menu.PanelType ThisPanelType;
    public int CaptureWaitTime;
    public Texture2D Picture;

	// Use this for initialization
	void Start () {
        CanvasEvents.OnSpriteInsidePanel += PanelSelected;
        CanvasEvents.OnSpriteOutsidePanel += PanelNotSelected;
        LastInsideTime = Time.time;
	}

    public void PanelSelected(RectTransform panel, RectTransform selector)
    {
        
        if (panel == ThisRectTransform)
        {
            if(!Selectors.Contains(selector))  Selectors.Add(selector);
            if (panelState == Menu.PanelState.Empty)
            {           
                EnterTime = Time.time;
                panelState = Menu.PanelState.Inside;
            }         

            if (EnterTime < Time.time - EnterStayTime )
            {  
                PanelStay(selector);
                if (panelState == Menu.PanelState.Inside)
                {                    
                    CanvasEvents.OnChooserPicture(CaptureWaitTime, ThisRectTransform);
                    panelState = Menu.PanelState.Stay;
                }
            }
        }
    }
    
    private void PanelStay(RectTransform selector)
    {       
        LastInsideTime = Time.time;
        revealManager.RemoveFromIdleChildren(ThisRectTransform.gameObject);
    }

    public void PanelNotSelected(RectTransform panel, RectTransform selector)
    {
     
        if (panel == ThisRectTransform)
        {
          
            if (Selectors.Contains(selector))  Selectors.Remove(selector);
            else if (Selectors.Count <= 0)
            {
                if (LastInsideTime < Time.time - 1 / Repeat)
                {
                    if (panelState == Menu.PanelState.Inside || panelState == Menu.PanelState.Stay)
                    {
                        panelState = Menu.PanelState.Leaving;
                    }
                    else
                    {
                        if (panelState == Menu.PanelState.Leaving)
                        {
                            PanelLeave();
                        }
                        else
                        {
                            Idle();
                        }

                    }
                }
            }
        }
    }

    private void Idle()
    {
        revealManager.IdleThis(ThisRectTransform.gameObject);
    }

    private void PanelLeave()
    {
        panelState = Menu.PanelState.Empty;
    }

}
