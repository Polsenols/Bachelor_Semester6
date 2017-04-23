using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManipulator : MonoBehaviour
{

    public float moveDampener = 10f;
    public LayerMask indicatorLayer;

    private GameObject x_axis;
    private GameObject y_axis;
    private GameObject z_axis;

    private GameObject rootObject;

    private GameObject selectedAxis;

    // Use this for initialization
    void Start()
    {
        x_axis = gameObject.transform.Find("x-axis").gameObject;
        y_axis = gameObject.transform.Find("y-axis").gameObject;
        z_axis = gameObject.transform.Find("z-axis").gameObject;
        rootObject = gameObject.transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, indicatorLayer.value))
            {
                print("axis hit");
                if (hit.collider.gameObject == x_axis)
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
        float startXpos = rootObject.transform.position.x;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = (Input.mousePosition - mouseCurrentPos) / moveDampener;
            float dotp = Vector3.Dot(Camera.main.transform.right, diff.normalized);
            Debug.Log(dotp);

            float difference = Mathf.Abs(diff.x) * Mathf.Sign(dotp) + startXpos;
            rootObject.transform.position = new Vector3(difference, rootObject.transform.position.y, rootObject.transform.position.z);
            yield return null;
        }
    }

    private IEnumerator MoveY_axis()
    {
        print("coroutine started");
        Vector3 mouseCurrentPos = Input.mousePosition;
        float startYpos = rootObject.transform.position.y;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = (Input.mousePosition - mouseCurrentPos) / moveDampener;
            float dotp = Vector3.Dot(Camera.main.transform.up, diff.normalized);
            Debug.Log(dotp);
            float difference = Mathf.Abs(diff.y) * Mathf.Sign(dotp) + startYpos;
            rootObject.transform.position = new Vector3(rootObject.transform.position.x, difference, rootObject.transform.position.z);
            yield return null;
        }
    }

    private IEnumerator MoveZ_axis()
    {
        print("coroutine started");
        Vector3 mouseCurrentPos = Input.mousePosition;
        float startZpos = rootObject.transform.position.z;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = (Input.mousePosition - mouseCurrentPos) / moveDampener;
            float dotp = Vector3.Dot(Camera.main.transform.forward, diff.normalized);
            Debug.Log(dotp);
            float difference = Mathf.Abs(diff.z) * Mathf.Sign(dotp) + startZpos;
            rootObject.transform.position = new Vector3(rootObject.transform.position.x, rootObject.transform.position.y, difference);
            yield return null;
        }
    }
}
