﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFurniture2 : MonoBehaviour
{

    [SerializeField]
    private GameObject rightHand;

    [SerializeField]
    private GameObject leftLine;

    private GameObject selected = null;
    private float dist = 0;

    //private Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    //private Dictionary<int, List<Color>> colors = new Dictionary<int, List<Color>>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            Debug.Log("R trigger pressed");
            leftLine.GetComponent<Renderer>().enabled = false;
            selectObject();
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            Debug.Log("R trigger released");
            leftLine.GetComponent<Renderer>().enabled = true;
            releaseObject();
        }

        moveObject();
    }

    private void selectObject()
    {
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 4.0f))
        {
            if (hit.collider.tag.Equals("Furniture") || hit.collider.tag.Equals("Whiteboard"))
            {
                selected = hit.collider.gameObject;
                dist = hit.distance;
                Debug.Log("Furniture selected");
            }
        }
    }

    private void releaseObject()
    {
        selected = null;
        dist = 0;
    }

    private void moveObject()
    {
        if (selected != null)
        {
            if (selected.tag.Equals("Furniture"))
            {
                moveFurniture();
            }
            else if (selected.tag.Equals("Whiteboard"))
            {
                Debug.Log("Move Whiteboard");
                moveWhiteboard();
            }
        }
    }

    private void moveFurniture()
    {
        Transform touchTrans = rightHand.transform;
        //selected.transform.rotation = rightHand.transform.rotation;
        selected.transform.position = touchTrans.position + touchTrans.forward * 2.0f;

        Vector2 rot = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        selected.transform.Rotate(new Vector3(rot.y, 0.0f, rot.x));

        Vector2 rotDist = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //dist += 0.2f * rotDist.y;
    }

    private void moveWhiteboard()
    {
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        RaycastHit[] hits;

        hits = Physics.RaycastAll(ray, 4.0f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag.Equals("Wall"))
            {
                selected.transform.position = hit.point;
                selected.transform.rotation = Quaternion.LookRotation(selected.transform.forward, -hit.normal);
                break;
            }
        }
    }
}