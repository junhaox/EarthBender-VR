using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField]
    private GameObject rightHand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A pressed");
            teleport();
        }
	}

    void teleport()
    {
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag.Equals("Floor"))
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
    }
}
