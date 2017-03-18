using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject leftHand;

    //private bool show = false;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(transform.position);
        transform.position = leftHand.transform.position;
	}

    public void ShowMenu()
    {
        Debug.Log("Show menu");
        gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        Debug.Log("Hide menu");
        gameObject.SetActive(false);
    }
}
