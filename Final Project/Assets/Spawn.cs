using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour {
    public static List<GameObject> monsters = new List<GameObject>();
    public GameObject rhino;
    public GameObject monster;
    public GameObject min, max;
    public System.Random rnd;
    public System.Random rnd2;
    public Image image;
    public Text scoreText;
    public static int score;
    public static bool start;
    public GameObject player;

    private float rangeX, rangeZ, startX, startZ;
	// Use this for initialization
	void Start () {
        rnd = new System.Random();
        rnd2 = new System.Random();
        rangeX = max.transform.position.x - min.transform.position.x;
        rangeZ = max.transform.position.z - min.transform.position.z;
        startX = min.transform.position.x;
        startZ = min.transform.position.z;
        score = 0;
        start = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (start && monsters.Count < 5)
        {
            GameObject newMon;
            Vector3 pos = new Vector3((float)(rnd2.NextDouble()) * rangeX + startX, 0.0f, (float)(rnd2.NextDouble()) * rangeZ + startZ);
            if (rnd.Next(2) == 0)
            {
                newMon = (GameObject)Instantiate(monster, pos, Quaternion.identity);
            }
            else
            {
                newMon = (GameObject)Instantiate(rhino, pos, Quaternion.identity);
            }
            monsters.Add(newMon);
            Radar.AddRadarObject(newMon, image);
        }

        scoreText.text = "Score:    " + score;
    }

    public static void RemoveMonster(GameObject o)
    {
        List<GameObject> newList = new List<GameObject>();
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == o)
            {
                Radar.RemoveRadarObject(monsters[i]);
                Destroy(monsters[i]);
                continue;
            }
            else
            {
                newList.Add(monsters[i]);
            }
        }
        monsters.RemoveRange(0, monsters.Count);
        monsters.AddRange(newList);
    }

    public static void AddScore()
    {
        score += 10;
    }

    public static void SubScore()
    {
        score -= 10;
    }

    public void Reset()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            Radar.RemoveRadarObject(monsters[i]);
            Destroy(monsters[i]);
        }
        monsters.RemoveRange(0, monsters.Count);
        start = true;
        score = 0;

        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject rock in rocks) {
            Radar.RemoveRadarObject(rock);
            Destroy(rock);
        }

        player.transform.position = new Vector3(24.89686f, 1.79962f, 7.751f);
    }
}
