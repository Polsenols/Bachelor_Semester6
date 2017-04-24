using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTask : MonoBehaviour {

    public Transform target;
    public float allowedError;

    private bool scaledToTarget;

    private Vector3 targetScale;
    private Vector3 objectScale;
	
	// Update is called once per frame
	void Update ()
    {
        objectScale = gameObject.transform.localScale;

        if (WithinTargetScale(objectScale, targetScale))
        {
            scaledToTarget = true;
        }
        else
        {
            scaledToTarget = false;
        }
    }

    private bool WithinTargetScale(Vector3 oScale, Vector3 tScale)
    {
        bool xWithin = false;
        bool yWithin = false;
        bool zWithin = false;
        if (oScale.x < tScale.x + allowedError && oScale.x > tScale.x - allowedError) { xWithin = true; }
        if (oScale.y < tScale.y + allowedError && oScale.y > tScale.y - allowedError) { yWithin = true; }
        if (oScale.z < tScale.z + allowedError && oScale.z > tScale.z - allowedError) { zWithin = true; }

        if (xWithin && yWithin && zWithin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
