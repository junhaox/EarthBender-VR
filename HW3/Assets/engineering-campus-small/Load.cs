using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Load : MonoBehaviour {
    public GameObject[] points;
	// Use this for initialization
	void Start () {
        LoadData();
        DrawData();
        
	}
	
	// Update is called once per frame
	void Update () {
		
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

        GameObject camera = GameObject.Find("LMHeadMountedRig");
        camera.transform.position = points[0].transform.position;
    }

    void DrawData()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].GetComponent<Renderer>().material.color = Color.red;

            if (i != 0)
            {
                GameObject lineObject = new GameObject();
                LineRenderer line = lineObject.AddComponent<LineRenderer>();

                line.SetPosition(0, points[i - 1].transform.position);
                line.SetPosition(1, points[i].transform.position);
            }
        }
    }
}
