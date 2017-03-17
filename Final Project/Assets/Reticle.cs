using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour {

    private static float RETICLE_SCALE = 40.0f;

    private Camera camera;

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
            //Debug.Log("HIT " + hit.point);
            Vector3 hitPos = hit.point;
            //hitPos.y = 2.0f;
            float offset = (hitPos - transform.position).magnitude / 30.0f;
            if ((hitPos - transform.position).magnitude < 0.2f)
            {
                transform.position = camera.transform.position + camera.transform.forward * 50.0f;
            } else
            {
                transform.position = hitPos - (camera.transform.forward.normalized * offset);
            }
            transform.forward = camera.transform.forward;
            float dist = (transform.position - camera.transform.position).magnitude;
            float scale = dist / RETICLE_SCALE;
            transform.localScale = new Vector3(scale, scale);
        } else
        {
            float dist = 50.0f;
            transform.position = camera.transform.position + camera.transform.forward * dist;
            transform.forward = camera.transform.forward;
            float scale = dist / RETICLE_SCALE;
            transform.localScale = new Vector3(scale, scale);
        }
    }
}
