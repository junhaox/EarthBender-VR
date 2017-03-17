using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour {

    private Camera camera;

    private GameObject prevObj;
    private int prevObjId;
    private float prevObjTime;

    // Use this for initialization
    void Start () {
        camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        transform.position = camera.transform.position + camera.transform.forward * 3.0f;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject obj = hit.transform.gameObject;
            if (prevObj == null || obj.GetInstanceID() != prevObjId)
            {
                prevObj = obj;
                prevObjId = obj.GetInstanceID();
                prevObjTime = Time.time;
            }
            if (Time.time - prevObjTime >= 2.0f)
            {
                Debug.Log("Looking at object " + prevObjId);
            }
        } else
        {
            prevObj = null;
        }
    }

    public GameObject GetGazedObject()
    {
        if (prevObj != null && Time.time - prevObjTime >= 2.0f)
        {
            return prevObj;
        }
        return null;
    }
}
