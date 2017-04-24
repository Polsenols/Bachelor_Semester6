using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyScale : MonoBehaviour {

    public Material selectedMat;
    public Transform[] xyz_box;
    private Vector3[] originalPos;

    void Start()
    {
        originalPos = new Vector3[xyz_box.Length];
        for (int i = 0; i < xyz_box.Length; i++)
        {
            originalPos[i] = xyz_box[i].localPosition;
        }
    }

    public void SetOriginalPos()
    {
        for (int i = 0; i < xyz_box.Length; i++)
        {
            xyz_box[i].localPosition = originalPos[i];
        }
    }

    public void TranslateBoxes(float moveAmount)
    {
        for (int i = 0; i < xyz_box.Length; i++)
        {
            xyz_box[i].position += xyz_box[i].forward * moveAmount;
        }
    }
}
