using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Load : MonoBehaviour {
    public ArrayList points;
	// Use this for initialization
	void Start () {
        points = new ArrayList();
        LoadData();
        DrawData();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadData()
    {
        string[] lines = System.IO.File.ReadAllLines("comptetion-track.txt");
        bool startPoint = false;
        foreach (string line in lines)
        {
            char[] delim = { ' ' };
            string[] coordinates = line.Split(delim);

            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.position = new Vector3(float.Parse(coordinates[0]) * 0.0254F, float.Parse(coordinates[1]) * 0.0254F, float.Parse(coordinates[2]) * 0.0254F) + transform.position;
            point.transform.localScale = new Vector3(9.144F, 9.144F, 9.144F);
            points.Add(point);

            if (!startPoint)
            {
                GameObject camera = GameObject.Find("LMHeadMountedRig");
                camera.transform.position = point.transform.position;
                startPoint = true;
            }
        }
    }

    void DrawData()
    {
        foreach (GameObject point in points)
        {
            point.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
