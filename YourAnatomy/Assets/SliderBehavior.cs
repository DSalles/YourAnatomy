using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliderBehavior : MonoBehaviour {

  public Menu.PanelState panelState = Menu.PanelState.Empty;
  public  RectTransform ThisRectTransform;
  private float EnterTime;
  private float EnterStayTime = .5f;
  private float LastInsideTime = 0;
  public int Repeat = 1;
 
  public RevealManager revealManager;
  public Menu.PanelType ThisPanelType;

  public bool NegativeSlider = false;
  private Vector2 PanelSizeWhenEntered;
  private Vector2 SelectorPosWhenEntered;
  private Vector2 PanelPosWhenEntered;
  public float CHANGETOLERANCE = 10f;

  public int SLIDERMIN = 1;
  public int SLIDERMAX = 0;
  public List<RectTransform> Selectors;

	// Use this for initialization
	void Start () 
    {
        CanvasEvents.OnSpriteInsidePanel += PanelSelected;
        CanvasEvents.OnSpriteOutsidePanel += PanelNotSelected;
        LastInsideTime = Time.time;
        // set each slider width to its panelType value
        if (ThisPanelType == Menu.PanelType.Skeleton)
        {
            ThisRectTransform.sizeDelta = new Vector2(Menu.SkeletonDialValue * 50, ThisRectTransform.sizeDelta.y);
        }
        else if (ThisPanelType == Menu.PanelType.Skin)
        {
            ThisRectTransform.sizeDelta = new Vector2(Menu.SkinDialValue * 50, ThisRectTransform.sizeDelta.y);
        }


        PanelSizeWhenEntered = ThisRectTransform.sizeDelta;
        SelectorPosWhenEntered = Vector2.zero;

	}

 
public void	PanelSelected(RectTransform panel, RectTransform selector)
    {
       // print("Slider Selected");  
        if (panel == ThisRectTransform)
            {
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
        print(selector.gameObject.name);
        if (!Selectors.Contains(selector)) Selectors.Add(selector);
        PanelSizeWhenEntered = ThisRectTransform.sizeDelta;
        PanelPosWhenEntered = ThisRectTransform.anchoredPosition;
        SelectorPosWhenEntered = selector.anchoredPosition; 
        selectorPosXLastChange = SelectorPosWhenEntered.x;

    }

private void PanelStay(RectTransform selector)
    {
      //  print("Slider Panel Stay");
        SliderAction(selector);
        LastInsideTime = Time.time;
      revealManager.RemoveFromIdleChildren(ThisRectTransform.gameObject);
    }

public void PanelNotSelected(RectTransform panel, RectTransform selector)
{
    if (panel == ThisRectTransform)
    {
        if (Selectors.Contains(selector)) Selectors.Remove(selector);
        else if (Selectors.Count <= 0)
        {
            if (LastInsideTime < Time.time - 1 / Repeat)
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


private float selectorPosXLastChange=0;
public string SliderUnit;

private void SliderAction(RectTransform selector)
    {  
        float selectorPosChange = selector.anchoredPosition.x - SelectorPosWhenEntered.x;

        if (SelectorMovedX(selector))
        {
            // save this selector Xpos for next selector move check
            selectorPosXLastChange = selector.anchoredPosition.x;
        
            if (!NegativeSlider) // if this is a right-moving slider
            {
                float xSize = PanelSizeWhenEntered.x + selectorPosChange;
                xSize = Mathf.Max(SLIDERMIN*50, Mathf.Min(SLIDERMAX*50, xSize));
                ThisRectTransform.sizeDelta = new Vector2(xSize, PanelSizeWhenEntered.y);
               // if (xSize < SLIDERMAX * 50 && xSize > SLIDERMIN*50) 
               // ThisRectTransform.anchoredPosition = new Vector2(PanelPosWhenEntered.x + selectorPosChange / 2, ThisRectTransform.anchoredPosition.y);
            }
            else if (NegativeSlider) // if this is a left-moving slider
            {
                float xSize = PanelSizeWhenEntered.x - selectorPosChange;
                xSize = Mathf.Max(SLIDERMIN*50, Mathf.Min(SLIDERMAX*50, xSize));
                ThisRectTransform.sizeDelta = new Vector2(xSize, PanelSizeWhenEntered.y);
             //   if (xSize < SLIDERMAX * 50 && xSize > SLIDERMIN * 50) 
             //   ThisRectTransform.anchoredPosition = new Vector2(PanelPosWhenEntered.x + selectorPosChange / 2, ThisRectTransform.anchoredPosition.y);
            }
            ThisRectTransform.GetComponentInChildren<UnityEngine.UI.Text>().text = (int)ThisRectTransform.sizeDelta.x / 50+SliderUnit;
            CanvasEvents.OnSliderChange(Mathf.Max(0, (int)ThisRectTransform.sizeDelta.x / 50), ThisRectTransform);
        }
    }

private bool SelectorMovedX(RectTransform selector)
    {
        return selector.anchoredPosition.x > selectorPosXLastChange + CHANGETOLERANCE || selector.anchoredPosition.x < selectorPosXLastChange - CHANGETOLERANCE;
    } 




}


