using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLines : MonoBehaviour {
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private LineRenderer rightLine;

    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private LineRenderer leftLine;

    // Use this for initialization
    void Start () {
        rightLine.startWidth = 0.01f;
        rightLine.endWidth = 0.01f;
        leftLine.startWidth = 0.01f;
        leftLine.endWidth = 0.01f;
    }
	
	// Update is called once per frame
	void Update () {
        Ray rightRay = new Ray(rightHand.transform.position, rightHand.transform.forward);
        RaycastHit rightHit;

        rightLine.SetPosition(0, rightHand.transform.position);
        if (Physics.Raycast(rightRay, out rightHit, Mathf.Infinity))
        {
            rightLine.SetPosition(1, rightHit.point);
        } else
        {
            rightLine.SetPosition(1, rightHand.transform.position + rightHand.transform.forward * 5);
        }

        Ray leftRay = new Ray(leftHand.transform.position, leftHand.transform.forward);
        RaycastHit leftHit;

        leftLine.SetPosition(0, leftHand.transform.position);
        if (Physics.Raycast(leftRay, out leftHit, Mathf.Infinity))
        {
            leftLine.SetPosition(1, leftHit.point);
        }
        else
        {
            leftLine.SetPosition(1, leftHand.transform.position + leftHand.transform.forward * 5);
        }
    }
}
