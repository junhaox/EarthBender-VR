using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour {

    private const int LASER_MODE = 0;
    private const int CANNON_MODE = 1;
    private const int OFF_MODE = 2;

    private int mode = OFF_MODE;

    private bool targetSelected = false;
    private bool atSky = false;
    private int targetId = 0;
    private System.DateTime lastReset = System.DateTime.MinValue;
    private System.DateTime laserBegin;

    [SerializeField] private SpawnBricks spawnBricks;
    [SerializeField] private GameObject cannonball;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit rayHit;

        Reset();

        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            GameObject target = rayHit.transform.gameObject;
            // Select target
            if (!targetSelected || target.GetInstanceID() != targetId)
            {
                targetSelected = true;
                targetId = target.GetInstanceID();
                laserBegin = System.DateTime.Now;
            } else
            {
                double timeGazed = (System.DateTime.Now - laserBegin).TotalSeconds;
                
                // Check if target is a switch
                Switch(target, timeGazed);

                if (mode == LASER_MODE)
                {
                    Laser(target, timeGazed);
                }
                else if (mode == CANNON_MODE)
                {
                    Cannon(target, timeGazed);
                }
            }
        }
	}

    private void Reset()
    {
        Camera cam = GetComponent<Camera>();
        Vector3 lookDir = cam.transform.forward;

        float theta = Vector3.Angle(lookDir, new Vector3(0.0f, 1.0f, 0.0f));

        if (theta <= 35.0f
            && (System.DateTime.Now - lastReset).TotalSeconds >= 2)
        {
            Debug.Log("Reset");
            lastReset = System.DateTime.Now;
            //SpawnBricks sb = gameObject.GetComponent<SpawnBricks>();
            spawnBricks.Reset();
        }
    }

    void Switch(GameObject target, double timeGazed)
    {
        string tag = target.GetComponent<Collider>().tag;
        if (tag.Equals("LaserSwitch"))
        {
            mode = LASER_MODE;
            Debug.Log("Laser Mode");
        } else if (tag.Equals("CannonSwitch"))
        {
            mode = CANNON_MODE;
            Debug.Log("Cannon Mode");
        } else if (tag.Equals("OffSwitch"))
        {
            mode = OFF_MODE;
            Debug.Log("Off Mode");
        }
    }

    void Laser(GameObject target, double timeGazed)
    {
        if (target.GetComponent<Collider>().tag.Equals("LaserDestroyable") &&
            timeGazed >= 1)
        {
            Destroy(target);
        }
    }

    void Cannon(GameObject target, double timeGazed)
    {
        if (!target.GetComponent<Collider>().tag.Equals("LaserDestroyable") ||
            timeGazed < 1)
        {
            return;
        }

        Camera cam = GetComponent<Camera>();

        GameObject ball = Instantiate(
            cannonball,
            cam.transform.position,
            Quaternion.identity
            );

        Vector3 shootDir = cam.transform.forward;
        ball.GetComponent<Rigidbody>().AddForce(shootDir * 7000.0f);

        // Reset gaze time
        laserBegin = System.DateTime.Now;
    }
}
