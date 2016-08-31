using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointTree : MonoBehaviour {
    public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Direction;

    private Vector3 StartPosition;
    private float startTime;
    private float journeyLength;
    private bool AboutToSwitchHead = false;
    private List<Transform> Heads = new List<Transform>();


	// Use this for initialization
	void Start () {

        SceneEvents.SkeletonUserCreate += NewUserSkeleton;
        SceneEvents.SkeletonUserDestroy += GoneUserSkeleton;
        ChangeDirection(Waypoint1);
	}

    private void GoneUserSkeleton(Transform entity, long id)
    {
        if (Heads.Contains(entity)) Heads.Remove(entity);
    }

    private void NewUserSkeleton(Transform skeleton, long id)
    {      
       Transform head = skeleton.FindChild("Head");
       ChangeDirection(head);
       AboutToSwitchHead = true;
       Heads.Add(head);
    }
	

    private void ChangeDirection(Transform wayPoint){
        StartPosition = transform.position;
        startTime = Time.time;
        Direction = wayPoint;
        journeyLength = Vector3.Distance(StartPosition, Direction.position);

    }

	// Update is called once per frame
	void Update () {	
        

        // decide between motion one and motion two
        float distCovered = (Time.time - startTime) * 1;
        float fracJourney = distCovered / journeyLength;

        if (fracJourney > 1)
        {
            if (AboutToSwitchHead) { AboutToSwitchHead = false;
          //  SwitchHead(Direction);
            }
            ChangeDirection(Waypoint2);
            
            return;
        }

        transform.position = Vector3.Lerp(StartPosition, Direction.position, fracJourney);
       


	}

    //private void SwitchHead(Transform head)
    //{
    //    GameObject NewHead = (GameObject)Instantiate(head.gameObject);
    //    NewHead.transform.parent = this.transform;
    //    head.gameObject.SetActive(false);
    //}
}
