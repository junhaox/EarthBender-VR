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

	// Use this for initialization
	void Start () {
        camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        rightHandPos = rightPalm.transform.position;
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
    }

    void NoFist()
    {
        Debug.Log("no fist");
        rightFist = false;
    }

    void Test()
    {
        Debug.Log("palm up");
    }
}
