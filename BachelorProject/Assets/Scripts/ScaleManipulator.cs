using UnityEngine;
using System.Collections;

public class ScaleManipulator : MonoBehaviour
{
    public Transform box_forward, box_up, box_right;
    public Transform line_forward, line_up, line_right;
    public LayerMask mask;
    public float moveMultiplier;
    private Transform currentBoxSelected;

    void Start()
    {

    }

    private enum MoveType
    {
        Forward,
        Up,
        Right
    }

    void Update()
    {
        SetLineScale();

        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                if(hit.transform == box_forward)
                {
                    currentBoxSelected = box_forward;
                }
                else if (hit.transform == box_up)
                {
                    currentBoxSelected = box_up;
                }
                else if (hit.transform == box_right)
                {
                    currentBoxSelected = box_right;
                }
            }
        }
        */
    }

    /*
    private IEnumerator Move_axis(Vector3 direction, MoveType moveType)
    {
        print("coroutine started");
        Vector3 mouseCurrentPos = Input.mousePosition;
        while (Input.GetMouseButton(0))
        {
            Vector3 diff = Input.mousePosition - mouseCurrentPos;
            mouseCurrentPos = Input.mousePosition;
            switch (moveType)
            {
                case MoveType.Forward:
                    currentBoxSelected.position += direction * (diff.y * moveMultiplier);
                    break;
                case MoveType.Up:
                    currentBoxSelected.position += direction * (diff.y * moveMultiplier);
                    break;
                case MoveType.Right:
                    currentBoxSelected.position += direction * (diff.y * moveMultiplier);
                    break;
            }
            yield return null;
        }
    }*/

    void SetLineScale()
    {
        line_forward.localScale = new Vector3(-box_forward.localPosition.x, 1, 1);
        line_forward.GetComponent<Renderer>().material = box_forward.GetComponent<Renderer>().material;
        line_right.localScale = new Vector3(1, 1, box_right.localPosition.z);
        line_right.GetComponent<Renderer>().material = box_right.GetComponent<Renderer>().material;
        line_up.localScale = new Vector3(1, box_up.localPosition.y, 1);
        line_up.GetComponent<Renderer>().material = box_up.GetComponent<Renderer>().material;
    }

}