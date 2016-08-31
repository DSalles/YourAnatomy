using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignBehavior : MonoBehaviour {
    public RectTransform ThisRectTransform; // to identify if the event is dealing with this panel
    public Menu.PanelState panelState = Menu.PanelState.Empty;
    private float EnterStayTime = .5f;
    private float EnterTime;
    private float LastInsideTime = 0;
    public int Repeat = 1;
    public RevealManager revealManager;
    public float MaxIdleTime = 3;
    public List<RectTransform> Selectors;

	// Use this for initialization
	void Start () {
        CanvasEvents.OnSpriteInsidePanel += PanelSelected;
        CanvasEvents.OnSpriteOutsidePanel += PanelNotSelected;
        LastInsideTime = Time.time;
	}  

    private void PanelSelected(RectTransform panel, RectTransform selector)
    {

        if (panel == ThisRectTransform)
        {
            if (!Selectors.Contains(selector)) Selectors.Add(selector);
            if (panelState == Menu.PanelState.Empty)
            {
                PanelEnter(selector);
                EnterTime = Time.time;
            }
            panelState = Menu.PanelState.Inside;
            if (EnterTime < Time.time - EnterStayTime)
                PanelStay(selector);
        }
    }
    private void PanelEnter(RectTransform selector)
    {
      //  print("Reached");
    }
    private void PanelStay(RectTransform selector)
    {
      //  print("PANEL STAY"); 
        LastInsideTime = Time.time;
        revealManager.ActivateChildrenInactivateSigns();
    }
    public void PanelNotSelected(RectTransform panel, RectTransform selector)
    {
        if (panel == ThisRectTransform)
        {
            if (Selectors.Contains(selector)) Selectors.Remove(selector);
            else if (Selectors.Count <= 0)
            {  if (LastInsideTime < Time.time - 1 / Repeat)
            {

                if (panelState == Menu.PanelState.Inside)
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
                        if (LastInsideTime < Time.time - MaxIdleTime)
                        {
                            Idle();
                        }
                }
            }}
        }
    }
    private void PanelLeave()
    {
        panelState = Menu.PanelState.Empty;
    }

    private void Idle()
    {

    }

}
