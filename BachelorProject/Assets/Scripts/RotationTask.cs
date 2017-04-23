using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTask : MonoBehaviour {

    public Transform target;
    public float allowedError;

    private bool rotatedToTarget;

    private Vector3 targetRotation;
    private Vector3 objectRotation;
	
	// Update is called once per frame
	void Update ()
    {
        objectRotation = gameObject.transform.rotation.eulerAngles;

        if (WithinTargetRotation(objectRotation, targetRotation))
        {
            rotatedToTarget = true;
        }
        else
        {
            rotatedToTarget = false;
        }
    }

    private bool WithinTargetRotation(Vector3 oRot, Vector3 tRot)
    {
        bool xWithin = false;
        bool yWithin = false;
        bool zWithin = false;
        if (oRot.x < tRot.x + allowedError && oRot.x > tRot.x - allowedError) { xWithin = true; }
        if (oRot.y < tRot.y + allowedError && oRot.y > tRot.y - allowedError) { yWithin = true; }
        if (oRot.z < tRot.z + allowedError && oRot.z > tRot.z - allowedError) { zWithin = true; }

        if (xWithin && yWithin && zWithin)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool GetRotationSuccess() { return rotatedToTarget; }
    public void SetTarget(Transform transform)
    {
        target = transform;
        targetRotation = target.rotation.eulerAngles;
    }
}
