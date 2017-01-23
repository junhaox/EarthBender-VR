using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour {
    [SerializeField] private GameObject prefab;

    private List<GameObject> bricks = new List<GameObject>();

	// Use this for initialization
	void Start () {
        Debug.Log("Start()");

        //GameObject spawnedBrick;
        //Quaternion startRotation = Quaternion.Euler(Vector3.zero);

        /*for (int i = 0; i < 10; i++)
        {
            Instantiate(prefab, new Vector3(i + 2.0f, 0, 0), Quaternion.Euler(new Vector3(0.0f, 45.0f, 0.0f)));
        }*/
        spawnBricks();

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Update()");
	}

    void spawnBricks()
    {
        int numBricks = 17;
        for (int i = 0; i < 10; i++)
        {
            float offset = i % 2 == 0 ? 0.0f : getRotation(1, numBricks) / 2.0f;
            spawnBrickLayer(i, numBricks, offset);
        }
    }

    void spawnBrickLayer(int layer, int numBricks, float offset)
    {
        BoxCollider boxCollider = prefab.GetComponent<BoxCollider>();
        Vector3 scale = prefab.transform.localScale;
        float sizeY = boxCollider.size.y * scale.y;

        float r = 2.9f;

        for (int i = 0; i < numBricks; i++)
        {
            float theta = getRotation(i, numBricks) + offset;
            float deg = radToDeg(theta);
            Debug.Log(theta);
            bricks.Add(Instantiate(
                prefab,
                //new Vector3(i + 2.0f, layer * sizeY, 0),
                new Vector3(r * Mathf.Cos(theta), layer * sizeY, r * Mathf.Sin(theta)),
                Quaternion.Euler(new Vector3(0.0f, -deg + 90.0f, 0.0f))
                ));
        }
    }

    float getRotation(int i, int n)
    {
        return (2.0f * Mathf.PI) / n * i;
    }

    float radToDeg(float rad)
    {
        return rad / (Mathf.PI * 2.0f) * 360.0f;
    }

    public void Reset()
    {
        Debug.Log("SpawnBricks Reset()");
        foreach (GameObject brick in bricks)
        {
            Destroy(brick);
        }
        bricks.Clear();

        spawnBricks();
    }
}
