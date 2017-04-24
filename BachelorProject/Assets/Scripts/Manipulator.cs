using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour
{

    public Transform target;
    public LayerMask selectMask, ManipMask;
    public ManipulatorType currentManip;
    public float moveMultiplier, rotationMultiplier, scaleMultiplier, anyAxisRotateMultiplier;
    public KeyCode SetMove = KeyCode.W;
    public KeyCode SetRotate = KeyCode.E;
    public KeyCode SetScale = KeyCode.R;
    public Material selectedMat;
    public GameObject moveObj, rotateObj, scaleObj;

    private Camera cam;
    Vector3 originalTargetPosition;
    private bool isTransforming = false;

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

        if(!isTransforming)SetManipType();

        if (target != null)
        {
            PositionManipulators();
        }
    }

    void PositionManipulators()
    {
        moveObj.transform.position = target.position;
        rotateObj.transform.localRotation = target.rotation;
    }

    void SetManipulatorPos()
    {
        //moveObj.transform.localPosition = target.position;
        moveObj.transform.localRotation = target.rotation;
        rotateObj.transform.position = target.position;
        scaleObj.transform.position = target.position;
        scaleObj.transform.rotation = target.rotation;
        rotateObj.transform.rotation = target.rotation;
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
            if(target!= null) rotateObj.transform.position = target.position;
            scaleObj.SetActive(false);
            rotateObj.SetActive(true);
            moveObj.SetActive(false);
            currentManip = ManipulatorType.Rotate;
        }
        else if (Input.GetKeyDown(SetScale))
        {
            if(target!=null) scaleObj.transform.position = target.position;
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
        isTransforming = true;
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
                            Vector3 localAxis = axis;
                            projected = transform.right;
                            float scaleAmount = Vector3.Dot((mousePos - previousMousePosition), projected) * scaleMultiplier;
                            target.localScale += (new Vector3(Mathf.Abs(target.localScale.normalized.x), Mathf.Abs(target.localScale.normalized.y), Mathf.Abs(target.localScale.normalized.z)) * scaleAmount);
                            axisObj.GetComponent<AnyScale>().TranslateBoxes(scaleAmount);
                        }
                        else
                        {
                            Vector3 localAxis = target.InverseTransformDirection(axis);
                            projected = projectedAxis;
                            float scaleAmount = Vector3.Dot((mousePos - previousMousePosition), projected) * scaleMultiplier;
                            target.localScale += (localAxis * scaleAmount);
                            axisObj.transform.position += axis * scaleAmount;
                        }
                        break;

                    case ManipulatorType.Rotate:
                        if (axisObj.CompareTag("Any"))
                        {
                            Vector3 rotation = transform.TransformDirection(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0));
                            target.Rotate(rotation * anyAxisRotateMultiplier, Space.World);
                        }
                        else
                        {
                            projected = (IsParallel(axis, planeNormal)) ? planeNormal : Vector3.Cross(axis, planeNormal);
                            float rotateAmount = (Vector3.Dot(mousePos - previousMousePosition, projected) * rotationMultiplier) / GetDistanceMultiplier();
                            target.Rotate(axis, rotateAmount, Space.World);
                        }
                        break;
                }
            }

            previousMousePosition = mousePos;
            yield return null;
        }
        if(currentManip == ManipulatorType.Scale) axisObj.transform.position = originalManipulatorPos;
        if (currentManip == ManipulatorType.Scale && axisObj.CompareTag("Any")) axisObj.GetComponent<AnyScale>().SetOriginalPos();
        axisObj.GetComponent<Renderer>().material = originalMat;
        isTransforming = false;
    }

    public static bool IsParallel(Vector3 direction, Vector3 otherDirection, float precision = .0001f)
    {
        return Vector3.Cross(direction, otherDirection).sqrMagnitude < precision;
    }

    float GetDistanceMultiplier()
    {
        if (target == null) return 0f;
        return Mathf.Max(.01f, Mathf.Abs(Vector3.Dot(target.position - transform.position, cam.transform.forward)));
    }
}
