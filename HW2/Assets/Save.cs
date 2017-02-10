using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour {
    [SerializeField]
    private GameObject furniture;

    List<GameObject> objects = new List<GameObject>();

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            Debug.Log("Left Thumbstick press");
            SaveData();
        }
    }

    void SaveData()
    {
        StreamWriter file = new StreamWriter("data.txt");
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (obj.activeInHierarchy &&
               (obj.tag.Equals("Furniture") || obj.tag.Equals("Whiteboard")))
            {
                //Debug.Log(obj.name);
                char[] delim = { ' ', '(' };
                string type = obj.name.Split(delim)[0];

                Vector3 pos = obj.transform.position;
                string line =
                    type + "|" + pos.x + "|" + pos.y + "|" + pos.z + "|" +
                    obj.transform.rotation.x + "|" + obj.transform.rotation.y + "|" +
                    obj.transform.rotation.z + "|" + obj.transform.rotation.w;

                Debug.Log(line);
                file.WriteLine(line);
            }
        }
        file.Close();
    }
}
