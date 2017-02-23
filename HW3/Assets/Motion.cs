using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Motion : MonoBehaviour {

    private bool forward = false;

    private Controller controller;
    private LeapServiceProvider sp;

	// Use this for initialization
	void Start () {
        controller = new Controller();
        sp = GameObject.Find("LeapHandController").GetComponent<LeapServiceProvider>();
	}
	
	// Update is called once per frame
	void Update () {
        //Frame frame = controller.Frame();
        Frame frame = sp.CurrentFrame;
        Hand rightHand = null;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                rightHand = hand;
            }
        }
        if (rightHand == null)
        {
            Debug.LogError("NO HAND FOUND");
            return;
        }
        //Finger index = rightHand.Fingers[1];

        Vector3 dir = (rightHand.PalmPosition.ToVector3() - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.forward) / 90;
        float sharpness = Mathf.Pow((1 + angle), 2.0f);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.02f * sharpness);

        if (forward)
        {
            Debug.Log(transform.forward);
            transform.position = transform.position + transform.forward * 0.7f;
        }
	}

    void Go()
    {
        Debug.Log("pointing");
        forward = true;
    }

    void Stop()
    {
        Debug.Log("stop");
        forward = false;
    }
}
