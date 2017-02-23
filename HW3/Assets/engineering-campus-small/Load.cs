using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Load : MonoBehaviour {
    public GameObject[] points;
    public GameObject target;
    public GameObject pointer;
    public LineRenderer pointerLine;
    public GameObject headMount;
    // Use this for initialization
    void Start () {
        LoadData();
        DrawData();

        pointer = new GameObject();
        pointerLine = pointer.AddComponent<LineRenderer>();
        pointerLine.material.color = Color.blue;

        headMount = GameObject.Find("LMHeadMountedRig");
        headMount.transform.position = points[0].transform.position;

        target = points[1];
    }

    // Update is called once per frame
    void Update() {
        pointerLine.SetPosition(0, headMount.transform.position + headMount.transform.forward * 10.0f);
        pointerLine.SetPosition(1, target.transform.position);
        pointerLine.startWidth = 0.1f;
        pointerLine.endWidth = 0.1f;
    }

    void LoadData()
    {
        string[] lines = System.IO.File.ReadAllLines("comptetion-track.txt");
        points = new GameObject[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            char[] delim = { ' ' };
            string[] coordinates = lines[i].Split(delim);

            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.position = new Vector3(float.Parse(coordinates[0]) * 0.0254F, float.Parse(coordinates[1]) * 0.0254F, float.Parse(coordinates[2]) * 0.0254F) + transform.position;
            point.transform.localScale = new Vector3(9.144F, 9.144F, 9.144F);
            points[i] = point;
        }
    }

    void DrawData()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].GetComponent<Renderer>().material.color = Color.red;

            if (i != 0)
            {
                GameObject path = new GameObject();
                LineRenderer pathLine = path.AddComponent<LineRenderer>();
                pathLine.SetPosition(0, points[i - 1].transform.position);
                pathLine.SetPosition(1, points[i].transform.position);
                pathLine.startWidth = 0.1f;
                pathLine.endWidth = 0.1f;
            }
        }
    }
}
