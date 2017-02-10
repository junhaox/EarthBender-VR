using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroup : MonoBehaviour
{

    [SerializeField]
    private GameObject rightHand;

    private GameObject selected = null;
    private float dist = 0;

    private Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    private Dictionary<int, List<Color>> colors = new Dictionary<int, List<Color>>();
    private List<Vector3> wbOffsets = new List<Vector3>();

    private int groupType = 0; // 0 - empty, 1 - furniture, 2 - whiteboard

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            Debug.Log("R hand pressed");
            selectMoveObject();
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            Debug.Log("R hand released");
            releaseObject();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            Debug.Log("B trigger pressed");
            selectObject();
        }


        moveObject();
    }

    private void selectObject()
    {
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if ((hit.collider.tag.Equals("Furniture") && groupType != 2 )|| 
                (hit.collider.tag.Equals("Whiteboard") && groupType != 1))
            {
                if (hit.collider.tag.Equals("Furniture"))
                {
                    groupType = 1;
                } else if (hit.collider.tag.Equals("Whiteboard"))
                {
                    groupType = 2;
                }
                GameObject selected = hit.collider.gameObject;
                if (!objects.ContainsKey(selected.GetInstanceID()))
                {
                    objects.Add(selected.GetInstanceID(), selected);
                    List<Color> cols = new List<Color>();
                    foreach (Renderer r in selected.GetComponentsInChildren<Renderer>())
                    {
                        cols.Add(r.material.GetColor("_Color"));
                        r.material.SetColor("_Color", Color.red);
                    }
                    colors.Add(selected.GetInstanceID(), cols); // store colors.
                    dist = hit.distance;
                    Debug.Log("Furniture selected");
                }
                else
                {
                    int i = 0;
                    List<Color> cols;
                    colors.TryGetValue(selected.GetInstanceID(), out cols);
                    foreach (Renderer r in selected.GetComponentsInChildren<Renderer>())
                    {
                        r.material.SetColor("_Color", cols[i]);
                        i++;
                    }
                    objects.Remove(selected.GetInstanceID());
                    colors.Remove(selected.GetInstanceID());
                    if (objects.Count == 0)
                    {
                        groupType = 0;
                    }
                }
            }
        }
    }

    private void selectMoveObject()
    {
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (objects.ContainsKey(hit.collider.gameObject.GetInstanceID()))
            {
                selected = hit.collider.gameObject;
                dist = hit.distance;
                Debug.Log("Furniture selected");

                if (groupType == 2)
                {
                    wbOffsets.Clear();
                    foreach (GameObject obj in objects.Values)
                    {
                        wbOffsets.Add(Vector3.Normalize(obj.transform.position - rightHand.transform.position) - rightHand.transform.forward);
                    }
                }
            }
        }
    }

    private void releaseObject()
    {
        selected = null;
        dist = 0;
    }

    private void moveObject()
    {
        if (selected != null)
        {
            if (groupType == 1)
            {
                moveFurniture();
            }
            else if (groupType == 2)
            {
                Debug.Log("Move Whiteboard");
                moveWhiteboards();
            }
        }
    }

    private void moveFurniture()
    {
        Transform touchTrans = rightHand.transform;

        foreach (GameObject obj in objects.Values)
        {
            obj.transform.position += (touchTrans.position + touchTrans.forward * dist) - selected.transform.position;
            Vector2 rot = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            obj.transform.Rotate(new Vector3(rot.y, 0.0f, rot.x));
        }

        Vector2 rotDist = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        dist += 0.2f * rotDist.y;
    }

    private void moveWhiteboards()
    {
        int i = 0;
        foreach (GameObject obj in objects.Values)
        {
            moveWhiteboard(obj, wbOffsets[i]);
            i++;
        }
    }

    private void moveWhiteboard(GameObject whiteboard, Vector3 dir)
    {
        //Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        Ray ray = new Ray(rightHand.transform.position, rightHand.transform.forward + dir);
        RaycastHit[] hits;

        hits = Physics.RaycastAll(ray, Mathf.Infinity);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag.Equals("Wall"))
            {
                whiteboard.transform.position = hit.point;
                whiteboard.transform.rotation = Quaternion.LookRotation(whiteboard.transform.forward, -hit.normal);
                break;
            }
        }
    }
}
