using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestures : MonoBehaviour {

    public GameObject rock;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OpenPalmUp()
    {
        //Debug.Log("OpenPalmUp");
        Vector3 position = new Vector3(25.0f, 4.0f, 25.0f);
        Instantiate(rock, position, Quaternion.identity);
    }

    void Test()
    {
        Debug.Log("palm up");
    }
}
