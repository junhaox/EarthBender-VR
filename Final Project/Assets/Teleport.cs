using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    private Camera camera;

    // Use this for initialization
    void Start () {
        camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Tele()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 newPos = hit.point;
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
