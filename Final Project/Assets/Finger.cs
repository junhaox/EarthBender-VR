using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour {

    public UnityEngine.UI.Button startButton;
    public UnityEngine.UI.Text startButtonText;
    public GameObject mon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag.Equals("StartButton"))
        {
            Debug.Log("Start Enter");
            //startButton.Select();
            startButtonText.text = "- Start -";
            mon.GetComponent<Spawn>().Reset();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag.Equals("StartButton"))
        {
            Debug.Log("Start Exit");
            startButtonText.text = "Start";
        }
    }
}
