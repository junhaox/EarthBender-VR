using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Motion : MonoBehaviour {

    private bool forward = false;

    private Controller controller;

	// Use this for initialization
	void Start () {
        controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
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



		if (true)
        {
            float pitch = rightHand.Direction.Pitch;
            float yaw = rightHand.Direction.Yaw;

            Debug.Log("pitch: " + pitch + " yaw: " + yaw);

            Transform T = this.transform;
            //T.position = T.position + T.forward * 1.0f;
            //T.forward = rightHand.Direction.ToVector3();
            //Debug.Log(rightHand.Direction.ToVector3());
        }
	}

    void Pointing()
    {
        Debug.Log("pointing");
        forward = true;
    }

    void StopPointing()
    {
        Debug.Log("stop");
        forward = false;
    }
}
