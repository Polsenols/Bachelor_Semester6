using UnityEngine;
using System.Collections;

public class ScaleManipulator : MonoBehaviour
{
    public Transform box_forward, box_up, box_right;
    public Transform line_forward, line_up, line_right;

    void Start()
    {

    }

    void Update()
    {
        SetLineScale();
    }

    void SetLineScale()
    {
        line_forward.localScale = new Vector3(-box_forward.localPosition.x, 1, 1);
        line_right.localScale = new Vector3(1, 1, box_right.localPosition.z);
        line_up.localScale = new Vector3(1, box_up.localPosition.y, 1);
    }

}