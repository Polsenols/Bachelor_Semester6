using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManipulator : MonoBehaviour {

    public float moveMultiplier = 0.05f;
    public LayerMask indicatorLayer;

    private GameObject x_axis;
    private GameObject y_axis;
    private GameObject z_axis;

    private GameObject rootObject;

    private GameObject selectedAxis;

	// Use this for initialization
	void Start ()
    {
        x_axis = gameObject.transform.Find("x-axis").gameObject;
        y_axis = gameObject.transform.Find("y-axis").gameObject;
        z_axis = gameObject.transform.Find("z-axis").gameObject;
        rootObject = gameObject.transform.root.gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000f, indicatorLayer.value))
            {
                print("axis hit");
                if(hit.collider.gameObject == x_axis)
                {
                    print("X-axis hit");
                    StartCoroutine(MoveX_axis());
                }

                else if (hit.collider.gameObject == y_axis)
                {
                    print("Y-axis hit");
                    StartCoroutine(MoveY_axis());
                }
                else if (hit.collider.gameObject == z_axis)
                {
                    print("Z-axis hit");
                    StartCoroutine(MoveZ_axis());
                }
            }

        }
	}

    private IEnumerator MoveX_axis()
    {
        print("coroutine started");
        Vector3 mouseCurrentPos = Input.mousePosition;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = Input.mousePosition - mouseCurrentPos;
            mouseCurrentPos = Input.mousePosition;

            rootObject.transform.position += rootObject.transform.right * (diff.x * moveMultiplier);
            yield return null;
        }        
    }

    private IEnumerator MoveY_axis()
    {
        print("coroutine started");
        Vector3 mouseCurrentPos = Input.mousePosition;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = Input.mousePosition - mouseCurrentPos;
            mouseCurrentPos = Input.mousePosition;

            rootObject.transform.position += rootObject.transform.up * (diff.y * moveMultiplier);
            yield return null;
        }
    }

    private IEnumerator MoveZ_axis()
    {
        print("coroutine started");
        Vector3 mouseCurrentPos = Input.mousePosition;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = Input.mousePosition - mouseCurrentPos;
            mouseCurrentPos = Input.mousePosition;

            rootObject.transform.position += rootObject.transform.forward * (diff.x * moveMultiplier);
            yield return null;
        }
    }
}
