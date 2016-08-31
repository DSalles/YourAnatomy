using UnityEngine;
using System.Collections;

public class SceneEvents : MonoBehaviour {


    public delegate void SceneEntityExistenceHandler(Transform entity, long id);

    public static SceneEntityExistenceHandler SkeletonUserCreate;
    public static SceneEntityExistenceHandler SkeletonUserDestroy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
