using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsTooClose : MonoBehaviour {

    public Color hitObjects;
    public Color selectedObject;
    public Color nonHitColor;

    public float radius = 1f;

    public KeyCode selectKey = KeyCode.K;

    public GameObject[] jointsInScene;
    private GameObject[] hitJoints;

    private bool objectsSelected = false;
    private List<GameObject> selectedGameObjects;

	void Start ()
    {
        jointsInScene = GameObject.FindGameObjectsWithTag("Joint");
        selectedGameObjects = new List<GameObject>();
    }

    void Update ()
    {
        //Cast a ray from screen to mouse and get all objects within the radius of the cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius);

        RaycastHit2Gameobject(hits);

        //Are we hitting any joints and are there even any joints in the scene
        if (hits != null && jointsInScene != null)
        {
            //Single object is hit by mouse
            if (Input.GetKeyDown(selectKey) && hitJoints.Length == 1)
            {
                //Remove previously selected objects from list
                selectedGameObjects.Clear();
                
                objectsSelected = true;
                selectedGameObjects.Add(hitJoints[0]);

                //Got to here
            }

            //If multiple objects are hit by mouse
            else if(Input.GetKeyDown(selectKey) && hitJoints.Length > 1)
            {
                //Remove previously selected objects from list
                selectedGameObjects.Clear();

                objectsSelected = true;

                //Got to here                
            }

            if (objectsSelected == false)
            {
                UpdateColorOfJoints();
            }
        }
    }

    //Convert RaycastHit to GameObjects
    private void RaycastHit2Gameobject(RaycastHit[] hits)
    {
        hitJoints = new GameObject[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            hitJoints[i] = hits[i].collider.gameObject;
        }
    }

    //Check if each of the joints in the scene is hit and change its color
    private void UpdateColorOfJoints()
    {
        foreach (GameObject joint in jointsInScene)
        {
            if (hitJoints.Contains(joint))
            {
                joint.GetComponent<Renderer>().material.color = hitObjects;
            }
            else
            {
                joint.GetComponent<Renderer>().material.color = nonHitColor;
            }
        }
    }
}
