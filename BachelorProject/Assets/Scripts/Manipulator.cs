using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour
{

    public Transform target;
    public LayerMask selectMask, ManipMask;
    public ManipulatorType currentManip;
    public float moveMultiplier, rotationMultiplier, scaleMultiplier;
    public KeyCode SetMove = KeyCode.W;
    public KeyCode SetRotate = KeyCode.E;
    public KeyCode SetScale = KeyCode.R;
    public Material selectedMat;
    public GameObject moveObj, rotateObj, scaleObj;

    private Camera cam;
    Vector3 originalTargetPosition;

    public enum ManipulatorType
    {
        Move,
        Rotate,
        Scale
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, ManipMask))
            {
                Debug.Log("hit manip");
                StartCoroutine(TransformObject(currentManip, hit.transform.gameObject));
            }
            else if (Physics.Raycast(ray, out hit, 1000f, selectMask))
            {
                target = hit.transform;
                SetManipulatorPos();
                originalTargetPosition = target.position;
            }
        }
        //SetManipType();
        if (target != null)
        {
            PositionManipulators();
        }
    }

    void PositionManipulators()
    {
        scaleObj.transform.position = target.position;
        rotateObj.transform.position = target.position;
        moveObj.transform.position = target.position;
    }

    void SetManipulatorPos()
    {
        //scaleObj.transform.SetParent(target);
        //rotateObj.transform.SetParent(target);
        //moveObj.transform.SetParent(target);
        moveObj.transform.localPosition = Vector3.zero;
        moveObj.transform.localRotation = Quaternion.identity;

    }

    void SetManipType()
    {
        if (Input.GetKeyDown(SetMove))
        {
            currentManip = ManipulatorType.Move;
            scaleObj.SetActive(false);
            rotateObj.SetActive(false);
            moveObj.SetActive(true);
        }
        else if (Input.GetKeyDown(SetRotate))
        {
            scaleObj.SetActive(false);
            rotateObj.SetActive(true);
            moveObj.SetActive(false);
            currentManip = ManipulatorType.Rotate;
        }
        else if (Input.GetKeyDown(SetScale))
        {
            scaleObj.SetActive(true);
            rotateObj.SetActive(false);
            moveObj.SetActive(false);
            currentManip = ManipulatorType.Scale;
        }
    }

    Vector3 GetSelectedAxis(GameObject axis)
    {
        if (axis.CompareTag("X"))
        {
            return target.right;
        }
        else if (axis.CompareTag("Y"))
        {
            return target.up;
        }
        else if (axis.CompareTag("Z"))
        {
            return target.forward;
        }
        else if (axis.CompareTag("Any"))
        {
            return Vector3.one;
        }
        return Vector3.one;
    }

    IEnumerator TransformObject(ManipulatorType type, GameObject axisObj)
    {
        if (target == null)
        {
            Debug.Log("Target is null, breaking");
            yield break;
        }
        Material originalMat = axisObj.GetComponent<Renderer>().material;
        axisObj.GetComponent<Renderer>().material = selectedMat;
        Vector3 planeNormal = (transform.position - target.position).normalized;
        Vector3 axis = GetSelectedAxis(axisObj);
        Vector3 projectedAxis = Vector3.ProjectOnPlane(axis, planeNormal).normalized;
        Vector3 previousMousePosition = Vector3.zero;
        Vector3 originalManipulatorPos = axisObj.transform.position;
        while (Input.GetMouseButton(0))
        {
            Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePos = Geometry.LinePlaneIntersect(mouseRay.origin, mouseRay.direction, originalTargetPosition, planeNormal);

            if (previousMousePosition != Vector3.zero && mousePos != Vector3.zero)
            {
                switch (currentManip)
                {
                    case ManipulatorType.Move:
                        float movementAmount = Vector3.Dot((mousePos - previousMousePosition), projectedAxis) * moveMultiplier;
                        target.Translate(axis * movementAmount, Space.World);
                        break;
                    case ManipulatorType.Scale:
                        Vector3 projected;
                        if (axisObj.CompareTag("Any"))
                        {
                            Vector3 localAxis = target.InverseTransformDirection(axis);
                            projected = transform.right;
                            float scaleAmount = Vector3.Dot((mousePos - previousMousePosition), projected) * scaleMultiplier;
                            target.localScale += (new Vector3(Mathf.Abs(target.localScale.normalized.x), Mathf.Abs(target.localScale.normalized.y), Mathf.Abs(target.localScale.normalized.z)) * scaleAmount);

                        }
                        else
                        {
                            Vector3 localAxis = axis;
                            projected = projectedAxis;
                            float scaleAmount = Vector3.Dot((mousePos - previousMousePosition), projected) * scaleMultiplier;
                            target.localScale += (localAxis * scaleAmount);
                        }

                        break;
                    case ManipulatorType.Rotate:
                        break;
                }
            }

            previousMousePosition = mousePos;
            yield return null;
        }
        axisObj.GetComponent<Renderer>().material = originalMat;
    }
}
