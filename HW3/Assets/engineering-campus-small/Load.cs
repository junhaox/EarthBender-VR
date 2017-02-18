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
        foreach (string line in lines)
        {
            char[] delim = { ' ' };
            string[] coordinates = line.Split(delim);

            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 pos = new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2]));
            point.transform.position = pos + transform.position;
            points.Add(point);
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
