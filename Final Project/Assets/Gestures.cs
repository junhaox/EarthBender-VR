using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestures : MonoBehaviour {

    //private static float MAX_Y_ACCEL = 0.0015f;
    private static float R_ACCEL = 0.0017f;
    private static float PUNCH_TRIGGER_RADIUS = 0.14f;

    private static int NO_JAB = 0;
    private static int POUND = 1;
    private static int PUNCH = 2;

    public GameObject rockPrefab;
    public GameObject rightPalm;
    public GameObject player;
    //public GameObject cube;
    public GameObject sphere;

    private Camera camera;

    private Vector3 rightHandPos;
    private Vector3 prevRightHandPos;
    private bool rightFist = false;

    //private float maxHandY = -1.0f;
    //private float maxYVel = MAX_Y_ACCEL;

    private Vector3 jabStartPos;
    private float maxRVel = R_ACCEL;
    private Vector3 jabDiff;
    
    private GameObject spawningRock;
    private GameObject gazedRock;
    private GameObject grabbedRock;

    private Vector3 grabStartPos;
    private Vector3 grabStartHandPos;
    private Vector3 prevGrabbedRockPos;
    private Vector3 grabbedRockDir;
    private Vector3 grabbedRockVel;

    //private bool leftOpen = false;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        rightHandPos = rightPalm.transform.position;

        if (spawningRock == null)
        {
            GameObject gazedObj = GetComponent<Gaze>().GetGazedObject();
            if (gazedObj != null && gazedObj.tag.Equals("Rock"))
            {
                Debug.Log("Setting gazedObj");
                gazedRock = gazedObj;
            }
            bool grabbingRock = ManageGrabbedRock();
        }
        
        if (grabbedRock == null)
        {
            int jab = DetectJab();
            if (jab == POUND)
            {
                SpawnRock();
            }

            if (jab == PUNCH)
            {
                Debug.Log("PUNCH");
                LaunchRock();
            }

            ManageSpawningRock();
        }
        
        //Debug.Log("AAA " + (rightHandPos - prevRightHandPos).magnitude);
        prevRightHandPos = rightHandPos;
	}

    int DetectJab()
    {
        //maxHandR -= maxRVel;
        //maxRVel += MAX_Y_ACCEL * 0.18f;

        sphere.transform.position = jabStartPos;
        //sphere.transform.localScale = new Vector3(PUNCH_TRIGGER_RADIUS, PUNCH_TRIGGER_RADIUS, PUNCH_TRIGGER_RADIUS);

        if (!rightFist || jabStartPos == null)
        {
            maxRVel = R_ACCEL;
            jabStartPos = rightHandPos;
            return NO_JAB;
        }

        Vector3 diff = rightHandPos - jabStartPos;
        jabStartPos += (diff.magnitude > maxRVel) ? diff.normalized * maxRVel : diff;
        maxRVel += R_ACCEL * 0.18f;
        //punchStartPos = rightHandPos;

        if (diff.magnitude < 0.02f)
        {
            maxRVel = R_ACCEL;
        }

        if (diff.magnitude >= PUNCH_TRIGGER_RADIUS)
        {
            //Debug.Log("punch");
            jabStartPos = rightHandPos;
            maxRVel = R_ACCEL;
            jabDiff = diff;
            return JabType(diff);
        }

        return NO_JAB;
    }

    // diff - rightHandPos - jabStartPos
    int JabType(Vector3 diff)
    {
        if (Vector3.Angle(Vector3.down, diff) < 30)
        {
            return POUND;
        }
        return PUNCH;
    }

    void SpawnRock()
    {
        if (spawningRock != null)
        {
            return;
        }

        Vector3 playerPos = player.transform.position;
        Vector3 spawnPos = playerPos + camera.transform.forward * 1.1f;
        spawnPos.y = -0.2f;
        GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);

        Collider rockCollider = rock.GetComponent<Collider>();
        rockCollider.enabled = false;

        Rigidbody rockRigidbody = rock.GetComponent<Rigidbody>();
        rockRigidbody.AddForce(Vector3.up * 360.0f);

        spawningRock = rock;
    }

    // returns true if a rock is currently grabbed
    bool ManageGrabbedRock()
    {
        if (grabbedRock == null)
        {
            return false;
        }

        //grabbedRock.transform.position = rightHandPos + new Vector3(0.0f, 0.0f, 2.0f);
        Vector3 newPos = grabStartPos + (rightHandPos - grabStartHandPos) * 60.0f;
        Vector3 offset = Vector3.zero;
        if (newPos.y < 1.0f)
        {
            newPos.y = 1.0f;
        }
        if (prevGrabbedRockPos.y > 1.0f)
        {
            offset = Vector3.up * Mathf.Sin(Time.time * 2.5f) * 0.3f;
        }
        Debug.Log(offset);
        //Vector3 newPos = grabbedRock.transform.position + (rightHandPos - prevRightHandPos) * 100.0f;

        Vector3 diff = newPos - prevGrabbedRockPos;

        prevGrabbedRockPos = prevGrabbedRockPos + diff.normalized * (diff.magnitude / 30.0f);
        grabbedRock.transform.position = prevGrabbedRockPos + offset;

        grabbedRockDir = diff;

        return true;
    }

    void ManageSpawningRock()
    {
        if (spawningRock == null)
        {
            return;
        }

        Collider rockCollider = spawningRock.GetComponent<Collider>();
        rockCollider.enabled = spawningRock.transform.position.y > 0.5f;

        Rigidbody rockRigidbody = spawningRock.GetComponent<Rigidbody>();
        if (spawningRock.transform.position.y < -0.4f &&
            rockRigidbody.velocity.y < 0)
        {
            Destroy(spawningRock);
            spawningRock = null;
            Debug.Log("spawning rock destroyed");
        }
    }

    void LaunchRock()
    {
        if (spawningRock != null)
        {
            Rigidbody rockRigidbody = spawningRock.GetComponent<Rigidbody>();
            rockRigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            rockRigidbody.AddForce(jabDiff * 7000.0f);
            spawningRock = null;
        }
    }


    void OpenPalmUp()
    {
        //Debug.Log("OpenPalmUp");
        Vector3 position = new Vector3(25.0f, 4.0f, 25.0f);
        Instantiate(rockPrefab, position, Quaternion.identity);
    }

    void Fist()
    {
        Debug.Log("fist");
        rightFist = true;

        if (gazedRock != null && gazedRock.tag.Equals("Rock"))
        {
            Debug.Log("Grabbing rock " + gazedRock.GetInstanceID());
            grabStartHandPos = rightHandPos;
            grabStartPos = gazedRock.transform.position;
            grabbedRock = gazedRock;
            gazedRock = null;

            prevGrabbedRockPos = grabbedRock.transform.position;

            Rigidbody rockRigidbody = grabbedRock.GetComponent<Rigidbody>();
            rockRigidbody.useGravity = false;
            rockRigidbody.velocity = Vector3.zero;
        }
    }

    void NoFist()
    {
        Debug.Log("no fist");
        rightFist = false;

        if (grabbedRock != null)
        {
            Rigidbody rockRigidbody = grabbedRock.GetComponent<Rigidbody>();
            rockRigidbody.useGravity = true;
            rockRigidbody.velocity = grabbedRockDir * 1.8f;
            //rockRigidbody.AddForce(grabbedRockDir * 80.0f);
            GetComponent<Gaze>().Drop();
        }
        grabbedRock = null;
    }

    void LeftHandOpen()
    {
        Debug.Log("Left hand open");
        player.GetComponent<Teleport>().Tele();
        //leftOpen = true;
    }

    void LeftHandClose()
    {
        Debug.Log("Left hand close");
        //leftOpen = false;
    }

    void Test()
    {
        Debug.Log("palm up");
    }
}
