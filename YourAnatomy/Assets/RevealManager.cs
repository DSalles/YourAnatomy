using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RevealManager : MonoBehaviour {
    public GameObject[] SignObjects;
    public GameObject[] ActiveElements;
    private List<GameObject> IdleChildren = new List<GameObject>();
    private float SkinSliderValue;
    private float SkeletonSliderValue;


	// Use this for initialization
	void Start () {

        CanvasEvents.OnStartMenu += StartMenu;
        SkinSliderValue = Menu.SkinDialValue;
        SkeletonSliderValue = Menu.SkeletonDialValue;
        CanvasEvents.OnSliderChange += SliderChangedValue;
	}

    private void StartMenu()
    {
        IdleChildren.Clear();
        AllChildrenAreIdle = false;
        StopCoroutine("IdleWait");
        InactivateChildrenActivateSigns();
    }

    private void SliderChangedValue(int value, RectTransform panel)
    {
        var dialType = panel.gameObject.GetComponent<SliderBehavior>().ThisPanelType;
        if (dialType == Menu.PanelType.Skin)
        {  SkinSliderValue = value ; }
        else if (dialType == Menu.PanelType.Skeleton)
        { SkeletonSliderValue = value ; }
    }
	
    public void IdleThis(GameObject child)
    {
        if (!IdleChildren.Contains(child))
        { IdleChildren.Add(child); }
        CheckIfAllChildrenIdling();
    }

 public void CheckIfAllChildrenIdling()
    {
        if (!AllChildrenAreIdle)
        {
            bool ThisSliderIsIdle = true;
            
            foreach (GameObject ae in ActiveElements)
            {
                if (!IdleChildren.Contains(ae))
                {
                    ThisSliderIsIdle = false;
                    AllChildrenAreIdle = false;
                }
            }

            if (ThisSliderIsIdle)
            {
                AllChildrenAreIdle = true;              
                StartCoroutine("IdleWait");
            }
        }
    }

 
 public void ActivateChildrenInactivateSigns()
 {  //  all children become active and the sign becomes inactive.
     CanvasEvents.OnMenuLifeSpanChange((float)Menu.LifeSpans.Max);
         AllChildrenAreIdle = false;
            foreach (GameObject slider in ActiveElements)
        {
            slider.SetActive(true);         
        }

     foreach(GameObject sign in SignObjects)
         {
             sign.SetActive(false);
         }       
     }

    bool AllChildrenAreIdle = false;

    public float IdleWaitTime = 1;

 private void InactivateChildrenActivateSigns()
    {       //  all children become inactive and the sign becomes active.
        CanvasEvents.OnMenuLifeSpanChange((float)Menu.LifeSpans.Min);
        foreach (GameObject child in ActiveElements)
        {
            child.SetActive(false);
        }
        foreach (GameObject sign in SignObjects)
        {
            sign.SetActive(true); /*
            if (sign.transform.FindChild("SkinText") != null)
            { sign.transform.FindChild("SkinText").GetComponent<UnityEngine.UI.Text>().text = "Skin: " + SkinSliderValue + " seconds"; }
            if (sign.transform.FindChild("SkeletonText") != null)
            { sign.transform.FindChild("SkeletonText").GetComponent<UnityEngine.UI.Text>().text = "Skeleton: " + SkeletonSliderValue + " seconds"; }
          */ 
        }
    }

    public void RemoveFromIdleChildren(GameObject child)
    { 
        IdleChildren.Remove(child);
        AllChildrenAreIdle = false;
        StopCoroutine("IdleWait");
    }

    IEnumerator IdleWait()
    {
        yield return new WaitForSeconds(IdleWaitTime);
        InactivateChildrenActivateSigns();
        
    }

}
