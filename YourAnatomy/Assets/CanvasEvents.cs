using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CanvasEvents : MonoBehaviour {

    public delegate void CanvasMenuActivateHandler();

    public static CanvasMenuActivateHandler OnStartMenu;
    public static CanvasMenuActivateHandler OnExitMenu;
    

    public delegate void CanvasEventHandler(RectTransform rectTrans, RectTransform selector);
    public static CanvasEventHandler OnSpriteInsidePanel;
    public static CanvasEventHandler OnSpriteOutsidePanel;


    public delegate void CanvasElementValueChange(int value, RectTransform panel);

    public static CanvasElementValueChange OnSliderChange;
    public static CanvasElementValueChange OnChooserPicture;

    public delegate void MenuLifeChange(float value);

    public static MenuLifeChange OnMenuLifeSpanChange;


    public static void SelectorInsidePanel(RectTransform panel, RectTransform selector)
    {
        if (OnSpriteInsidePanel != null)
            OnSpriteInsidePanel(panel, selector);
    }

    private void SelectorOutsidePanel(RectTransform panel, RectTransform selector)
    {
        if (OnSpriteOutsidePanel != null) OnSpriteOutsidePanel(panel, selector);
    }
Dictionary<RectTransform,float> stoppedSelectorsDic = new Dictionary<RectTransform,float>();

	    private float timeLastEntered = 0;
        private int REPEAT = 10;
        private bool INSIDE = false;


        private List<RectTransform> activeSelectors = new List<RectTransform>();
        private Dictionary<RectTransform, Vector2> SelectorsDic = new Dictionary<RectTransform,Vector2>();
     //   private Vector2 LastSelectorPos;
	 void Update () {
        RectTransform[] bodySprites = gameObject.transform.GetComponentsInChildren<RectTransform>();
     
        foreach (RectTransform rt in bodySprites)
        {
            foreach (RectTransform rt2 in bodySprites)
                if (rt2.tag == "Selector") {                    
                    if (SelectorsDic.ContainsKey(rt2))
                    {
                        if (SelectorsDic[rt2] != rt2.anchoredPosition) // selector moved
                        { 
                           
                            if (stoppedSelectorsDic.ContainsKey(rt2)) stoppedSelectorsDic.Remove(rt2);              
                           // LastSelectorPos = (Vector2)SelectorsDic[rt2];
                            if(!activeSelectors.Contains(rt2))activeSelectors.Add(rt2);
                            SelectorsDic[rt2] = rt2.anchoredPosition;
                            continue;
                        }
                        else if (SelectorsDic[rt2] == rt2.anchoredPosition)//not moved
                        {
                            if (!stoppedSelectorsDic.ContainsKey(rt2)) stoppedSelectorsDic.Add(rt2, Time.time);
                            if (stoppedSelectorsDic.ContainsKey(rt2)) if (stoppedSelectorsDic[rt2] + 1 - Time.time < 0) if (activeSelectors.Contains(rt2))
                            {
                                stoppedSelectorsDic.Remove(rt2);
                                activeSelectors.Remove(rt2);
                            }

                            //timing(
                            //print(rt2.gameObject.name +" Stopped");
                            //if (activeSelectors.Contains(rt2)) activeSelectors.Remove(rt2);
                        }                       
                    }
                    else
                    {
                        SelectorsDic.Add(rt2, new Vector2(rt2.anchoredPosition.x,rt2.anchoredPosition.y));
                    }
                }

            if (rt.tag == "Panel")
            {
               // CheckCursorInside(rt);
                foreach (RectTransform rt2 in bodySprites)
                    if (rt2.tag == "Selector")
                    {  // check if the selector has moved
                       if(activeSelectors.Contains(rt2))
                        CheckRectIntersects(rt, rt2);
                    }
            }
        }
       // CheckRectIntersects(bodySprites);
        
	
	}

     private float CheckAge(float p)
     {
         return p;
     }

    //private void CheckCursorInside(RectTransform panel)
    //{

      
    //    Vector2 pSize = panel.sizeDelta;
    //    float pLeft = (panel.anchoredPosition.x - pSize.x * panel.pivot.x);
    //    float pBottom = (panel.anchoredPosition.y - pSize.y / 2);
    //    Rect panelRect = new Rect(pLeft, pBottom, pSize.x, pSize.y);
        
        
    //    if(panelRect.Contains(Input.mousePosition))
    //    {
    //        print("MOUSEOVER!" + Input.mousePosition);
    //        SelectorInsidePanel(panel, DefaultSelector); }
    //    else
    //    { SelectorOutsidePanel(panel, DefaultSelector); }

    //}

    private void CheckRectIntersects(RectTransform panel, RectTransform selector)
    {
        //foreach (RectTransform selector in selectors)
        //{
           
        //        foreach (RectTransform panel in bodySprites)
        //        {
                    //if (panel.tag == "Panel")
                    //{
         Rect panelRect = PanelRect(panel);
         Rect selectorRect = SelectorRect(selector);
                        

                        if (selectorRect.Overlaps(panelRect))
                        {
                            SelectorInsidePanel(panel, selector);
                        }
                        else
                        {
                            SelectorOutsidePanel(panel, selector);
                        }

                //    }
                //}
      //  }
    }

    private static Rect PanelRect(RectTransform panel)
    {
        float pPosX;
        float pPosY;
        var panelOrParent = panel;
        pPosX = panelOrParent.anchoredPosition.x;
        pPosY = panelOrParent.anchoredPosition.y;

        while (panelOrParent.name != "HeadSpot") // regressing additive parent anchoredPosition offset up to the moving parent
        {
            panelOrParent = panelOrParent.parent.gameObject.GetComponent<RectTransform>();
            pPosX += panelOrParent.anchoredPosition.x;
            pPosY += panelOrParent.anchoredPosition.y;
        }
        Vector2 pSize = panel.sizeDelta;
        float pLeft = (pPosX - pSize.x * panel.pivot.x);
        float pBottom = (pPosY - pSize.y / 2);
        Rect panelRect = new Rect(pLeft, pBottom, pSize.x, pSize.y);
        return panelRect;
    }

    private static Rect SelectorRect(RectTransform selector)
    {
        Vector2 sSize = selector.sizeDelta;

        float sRecX = (selector.anchoredPosition.x - sSize.x / 2);
        float sRecY = (selector.anchoredPosition.y - sSize.y / 2);


        Rect selectorRect = new Rect(sRecX, sRecY, sSize.x, sSize.y);
        return selectorRect;
    }


}
