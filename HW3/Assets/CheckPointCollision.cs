﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCollision : MonoBehaviour {

    public Load load;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("CheckPoint"))
        {
            Debug.Log("checkpoint collision");
            load.HitCheckPoint(collision.gameObject.GetInstanceID());
        }
    }
}
