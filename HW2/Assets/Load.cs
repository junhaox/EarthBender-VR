using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Load : MonoBehaviour
{
    [SerializeField]
    private GameObject furniture;

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

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            Debug.Log("Right Thumbstick press");
            DeleteObjects();
            LoadData();
        }
    }

    void DeleteObjects()
    {
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (obj.activeInHierarchy &&
               (obj.tag.Equals("Furniture") || obj.tag.Equals("Whiteboard")))
            {
                Destroy(obj);
            }
        }
    }

    void LoadData()
    {
        string[] lines = System.IO.File.ReadAllLines("data.txt");
        foreach (string line in lines)
        {
            char[] delim = { '|' };
            string[] words = line.Split(delim);
            GameObject type = null;
            if (words[0].Equals("chair"))
            {
                type = chair;
            }
            else if (words[0].Equals("desk"))
            {
                type = desk;
            }
            else if (words[0].Equals("cabinet"))
            {
                type = cabinet;
            }
            else if (words[0].Equals("locker"))
            {
                type = locker;
            }
            else if (words[0].Equals("3DTV"))
            {
                type = tv;
            }
            else if (words[0].Equals("whiteboard"))
            {
                type = whiteboard;
            }
            Vector3 pos = new Vector3(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
            Quaternion rot = new Quaternion(float.Parse(words[4]), float.Parse(words[5]), float.Parse(words[6]), float.Parse(words[7]));
            Instantiate(type, pos, rot);
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
                    obj.transform.rotation.x + "|" + obj.transform.rotation.y + "|" + obj.transform.rotation.z;

                Debug.Log(line);
                file.WriteLine(line);
            }
        }
        file.Close();
    }
}
