using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTask : MonoBehaviour {

    public Transform goalObj;
    public float allowedError;

    private bool positionedToTarget;

    private Vector3 objectPos;
    private Vector3 targetPos;
    private Transform taskObj;
    void Start()
    {
        SetTarget(goalObj);
    }

	void Update ()
    {
        objectPos = taskObj.position;

        if (WithinTargetPosition(objectPos, targetPos))
        {
            positionedToTarget = true;
        }
        else
        {
            positionedToTarget = false;
        }
	}

    private bool WithinTargetPosition(Vector3 oPos, Vector3 tPos)
    {
        bool xWithin = false;
        bool yWithin = false;
        bool zWithin = false;
        if (oPos.x >= tPos.x - allowedError && oPos.x <= tPos.x + allowedError) { xWithin = true; }
        if (oPos.y <= tPos.y + allowedError && oPos.y > tPos.y - allowedError) { yWithin = true; }
        if (oPos.z >= tPos.z - allowedError && oPos.z <= tPos.z + allowedError) { zWithin = true; }
        Debug.Log("xwithin: " + xWithin);
        Debug.Log("ywithin: " + yWithin);
        Debug.Log("zwithin: " + zWithin);
        if (xWithin && yWithin && zWithin)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool GetTransformSuccess()
    {
        return positionedToTarget;
    }

    public void SetTarget(Transform transform)
    {
        goalObj = transform;
        targetPos = goalObj.position;
    }

    public void SetTaskObject(Transform taskObj)
    {
        taskObj = this.taskObj;
    }
}
