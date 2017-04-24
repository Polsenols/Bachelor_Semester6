using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManipulator : MonoBehaviour
{

    public float moveDampener = 10f;
    public LayerMask indicatorLayer;
    public Material selectedMat;

    private GameObject x_axis;
    private GameObject y_axis;
    private GameObject z_axis;

    private GameObject rootObject;

    private GameObject selectedAxis;

    public Renderer x_axisRender, y_axisRender, z_axisRender;
    private Material original_X_Mat, original_Z_Mat, original_Y_Mat;

    // Use this for initialization
    void Start()
    {
        x_axis = gameObject.transform.Find("x-axis").gameObject;
        y_axis = gameObject.transform.Find("y-axis").gameObject;
        z_axis = gameObject.transform.Find("z-axis").gameObject;
        rootObject = gameObject.transform.root.gameObject;
        original_X_Mat = x_axisRender.material;
        original_Z_Mat = z_axisRender.material;
        original_Y_Mat = y_axisRender.material;
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
        x_axis.GetComponent<Renderer>().material = selectedMat;
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
        x_axis.GetComponent<Renderer>().material = original_X_Mat;
    }

    private IEnumerator MoveY_axis()
    {
        print("coroutine started");
        y_axis.GetComponent<Renderer>().material = selectedMat;
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
        y_axis.GetComponent<Renderer>().material = original_Y_Mat;
    }

    private IEnumerator MoveZ_axis()
    {
        print("coroutine started");
        z_axis.GetComponent<Renderer>().material = selectedMat;
        Vector3 mouseCurrentPos = Input.mousePosition;
        //Vector3 planeNormal = (transform.position - target.position).normalized;
        float startZpos = rootObject.transform.position.z;
        while (Input.GetMouseButton(0))
        {
            
            Vector3 diff = (Input.mousePosition - mouseCurrentPos) / moveDampener;
            float dotp = Vector3.Dot(Camera.main.transform.forward, diff.normalized);
            Debug.Log(dotp);
            float difference = Mathf.Abs(diff.x*diff.y) * Mathf.Sign(dotp) + startZpos;
            rootObject.transform.position = new Vector3(rootObject.transform.position.x, rootObject.transform.position.y, difference);
            yield return null;
        }
        z_axis.GetComponent<Renderer>().material = original_Z_Mat;
    }

    public static float MagnitudeInDirection(Vector3 vector, Vector3 direction, bool normalizeParameters = true)
    {
        if (normalizeParameters) direction.Normalize();
        return Vector3.Dot(vector, direction);
    }
}
