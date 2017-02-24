using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour {
    public GameObject[] points;
    public GameObject target;
    public GameObject pointer;
    public LineRenderer pointerLine;
    public GameObject headMount;
    public GameObject camera;

    public Text text;
    public Text timeText;

    float start = 0;
    float countdown = 1.0f;

    private int nextIdx = 1;
    // Use this for initialization
    void Start () {
        LoadData();
        DrawData();

        start = Time.time;

        pointer = new GameObject();
        pointerLine = pointer.AddComponent<LineRenderer>();
        pointerLine.material.color = Color.blue;

        headMount = GameObject.Find("LMHeadMountedRig");
        headMount.transform.position = points[0].transform.position;
        headMount.transform.LookAt(points[1].transform);

        camera = GameObject.Find("CenterEyeAnchor");

        target = points[nextIdx];
    }

    // Update is called once per frame
    void Update() {
        target = points[nextIdx];
        pointerLine.SetPosition(0, camera.transform.position + camera.transform.forward * 10.0f);
        pointerLine.SetPosition(1, target.transform.position);
        pointerLine.startWidth = 0.1f;
        pointerLine.endWidth = 0.1f;

        float time = Time.time - start;
        if (time < countdown)
        {
            text.text = (countdown - time).ToString("0");
            timeText.text = "";
        } else
        {
            float dist = Vector3.Distance(target.transform.position, headMount.transform.position);
            text.text = dist.ToString("0") + "m";

            float timer = Time.time - start - countdown;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            int ms = Mathf.FloorToInt((timer - (float)(int)timer) * 100);
            timeText.text = string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, ms);
        }
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
            //point.tag = i == 1 ? "Next" : "NotNext";
            point.transform.position = new Vector3(float.Parse(coordinates[0]) * 0.0254F, float.Parse(coordinates[1]) * 0.0254F, float.Parse(coordinates[2]) * 0.0254F) + transform.position;
            point.transform.localScale = new Vector3(9.144F, 9.144F, 9.144F);
            
            SphereCollider sc = point.AddComponent<SphereCollider>();
            sc.radius = 1.0f;
            point.tag = "CheckPoint";
            //Rigidbody rb = point.AddComponent<Rigidbody>();
            //rb.useGravity = false;
 
            points[i] = point;
        }
    }

    void DrawData()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].GetComponent<Renderer>().material.color = i == 0 ? Color.green : Color.red;

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

    public void HitCheckPoint(int id)
    {
        if (id == points[nextIdx].GetInstanceID())
        {
            points[nextIdx].GetComponent<Renderer>().material.color = Color.green;
            nextIdx++;
        }
        if (nextIdx >= points.Length)
        {
            nextIdx = 0;
            Debug.Log("FINISHED");
        }
    }

    public bool Playing()
    {
        return Time.time - start >= countdown;
    }
}
