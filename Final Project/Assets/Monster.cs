using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    private GameObject playerPos;
    public Rigidbody rig;
	// Use this for initialization
	void Start () {
        playerPos = GameObject.Find("LMHeadMountedRig");
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = playerPos.transform.position - transform.position;
        transform.forward = new Vector3(forward.x, 0.0f, forward.z);
        transform.position += new Vector3(transform.forward.x, 0.0f, transform.forward.z) * 0.005f;
        if ( Vector3.Distance(transform.position, playerPos.transform.position) < 4.0f)
        {
            Spawn.RemoveMonster(gameObject);
            Spawn.SubScore();
        }
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.gameObject.tag.Equals("Rock"))
        {
            Debug.Log("hit");
            rig = other.gameObject.GetComponent<Rigidbody>();
            if (rig.velocity.magnitude > 1.0f)
            {
                Spawn.RemoveMonster(gameObject);
                Spawn.AddScore();
            }
        }
    }
}
