using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFurniture : MonoBehaviour {

    [SerializeField]
    private GameObject desk;

    [SerializeField]
    private GameObject chair;

    [SerializeField]
    private GameObject locker;

    [SerializeField]
    private GameObject cabinet;

    [SerializeField]
    private GameObject tv;

    [SerializeField]
    private GameObject whiteboard;

    [SerializeField]
    private GameObject leftHand;

    private int type = 5;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            Debug.Log("X pressed");
            spawnFurniture();
        } else if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            Debug.Log("Y pressed");
            type = (type + 1) % 6;
        }
    }

    private void spawnFurniture()
    {
        GameObject furniture;
        switch (type)
        {
            case 0:
                furniture = desk;
                break;
            case 1:
                furniture = chair;
                break;
            case 2:
                furniture = locker;
                break;
            case 3:
                furniture = cabinet;
                break;
            case 4:
            default:
                furniture = chair;
                break;
        }

        if (type == 5)
        {
            spawnWhiteboard();
        } else
        {
            Instantiate(
                    furniture,
                    new Vector3(0.0f, 20.0f, 0.0f),
                    Quaternion.Euler(new Vector3(0.0f, 1.0f, 0.0f))
                    );
        }
    }

    private void spawnWhiteboard()
    {
        Ray ray = new Ray(leftHand.transform.position, leftHand.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag.Equals("Wall"))
        {
            Debug.Log("Spawn Whiteboard");
            Debug.Log(hit.normal);
            Instantiate(
                    whiteboard,
                    hit.point,
                    Quaternion.LookRotation(whiteboard.transform.forward, -hit.normal)
                    );
        } else
        {
            Debug.Log("not pointing at wall");
        }
    }
}
