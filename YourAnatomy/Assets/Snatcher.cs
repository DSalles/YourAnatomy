using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Snatcher : MonoBehaviour {

    public RectTransform HeadFiller;
    private List<Transform> Heads = new List<Transform>();
    private KinectManager kinectManager;

    void Start()
    {
        kinectManager = KinectManager.Instance;
        SceneEvents.SkeletonUserCreate += NewUserSkeleton;
        SceneEvents.SkeletonUserDestroy += GoneUserSkeleton;
        if (this.transform.parent.FindChild("Head")!= null) Heads.Add(this.transform.parent.FindChild("Head"));
       var heads =(GameObject.FindGameObjectsWithTag("Head"));
       foreach (GameObject go in heads)
       {
           Heads.Add(go.transform);
       }
    }

   public bool TriggerOn = true; 
   public int OverlapTolerance = 2;

    void Update()
   {
      
        if(Heads.Count > 0)

           for (int i = 0; i < Heads.Count;i++) 
            {
                Transform head = Heads[i];
                if (this.transform.position.x > head.position.x - OverlapTolerance && this.transform.position.x < head.position.x + OverlapTolerance && this.transform.position.y > head.position.y - OverlapTolerance && this.transform.position.y < head.position.y + OverlapTolerance && this.transform.position.z > head.transform.position.z - OverlapTolerance && this.transform.position.z < head.transform.position.z + OverlapTolerance)
                {
                    Intersect(head);
                }
            }
        Vector2 headFillerPos;
        if (HeadFiller!=null&&HeadFiller.gameObject.activeSelf == true)
        {
            headFillerPos = new Vector2(Camera.main.WorldToViewportPoint(this.transform.position).x*1920-960, Camera.main.WorldToViewportPoint(this.transform.position).y*1080-540);
            HeadFiller.anchoredPosition = headFillerPos;
        }
           // print(Camera.main.WorldToViewportPoint(this.transform.position));
    }
    
    void Intersect(Transform other)
    {
        if (!TriggerOn) return;
        TriggerOn = false;
        print("trigger");
        if (other.gameObject.name == "Head")
        {
            SwitchHead(other.transform);
        }
    }

    private void GoneUserSkeleton(Transform entity, long id)
    {
        if (Heads.Contains(entity)) Heads.Remove(entity);
    }

    private void NewUserSkeleton(Transform skeleton, long id)
    {      
       Transform head = skeleton.FindChild("Head");
  //    ChangeDirection(head);
  //     AboutToSwitchHead = true;
       Heads.Add(head);
    }


    private void SwitchHead(Transform head)
    {      
        GameObject NewHead = (GameObject)Instantiate(head.gameObject);
        NewHead.transform.parent = this.transform;
        head.gameObject.SetActive(false);
        GameObject copy = (GameObject)Instantiate(GameObject.Find("HeadFill"));

        HeadFiller = copy.GetComponent<RectTransform>();

        HeadFiller.transform.parent = (GameObject.Find("CanvasCutoutOverlay").transform);
        HeadFiller.FindChild("Decapitated head").GetComponent<RawImage>().texture = kinectManager.GetUsersLblTex();
    }
}
