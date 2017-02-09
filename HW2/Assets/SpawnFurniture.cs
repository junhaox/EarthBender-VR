using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFurniture : MonoBehaviour {

    [SerializeField]
    private GameObject desk;

    [SerializeField]
    private GameObject chair;

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
        }
    }

    private void spawnFurniture()
    {
        Instantiate(
                desk,
                new Vector3(0.0f, 20.0f, 0.0f),
                Quaternion.Euler(new Vector3(0.0f, 1.0f, 0.0f))
                );
    }
}
