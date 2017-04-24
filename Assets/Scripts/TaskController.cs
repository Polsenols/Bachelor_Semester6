using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class TaskController : MonoBehaviour
{

    public Transform target;

    public bool positionTask;
    public bool rotationTask;
    public bool scaleTask;

    public float positionOffset;
    public float rotationOffset;
    public float scaleOffset;

    public Color successColor;

    private float taskStartTime;

    //Success states
    private bool transformSuccess;
    private bool lastTransformState;
    private float transformCompletionTime;

    private bool rotateSuccess;
    private bool lastRotationState;
    private float rotateCompletionTime;

    private bool scaleSucess;
    private bool lastScaleState;
    private float scaleCompletionTime;

    private Color startColor;
    private bool dataStored;

    //Target info
    Vector3 targetPosition;
    Vector3 targetRotation;
    Vector3 targetScale;
    //private ScaleTask scaleT;
    //private PositionTask posT;
    //private RotationTask rotT;

    // Use this for initialization
    void Start()
    {
        targetPosition = target.transform.position;
        targetRotation = target.transform.rotation.eulerAngles;
        targetScale = target.transform.localScale;
        startColor = gameObject.GetComponent<Renderer>().material.color;

        //posT = gameObject.GetComponent<PositionTask>();
        //rotT = gameObject.GetComponent<RotationTask>();
        //scaleT = gameObject.GetComponent<ScaleTask>();
        //posT.SetTarget(target);
        //rotT.SetTarget(target);
        //scaleT.SetTarget(target);

        dataStored = false;

        lastTransformState = false;
        lastRotationState = false;
        lastScaleState = false;

        taskStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Check success of manipulations
        transformSuccess = TransformCheck();
        rotateSuccess = RotationCheck();
        scaleSucess = ScaleCheck();

        if (transformSuccess && rotateSuccess && scaleSucess)
        {
            gameObject.GetComponent<Renderer>().material.color = successColor;
            if (!dataStored)
            {
                dataStored = true;
                StoreDataInFile();
            }   
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = startColor;
        }

        //If non of the tasks are picked
        if (!positionTask && !rotationTask && !scaleTask)
        {
            gameObject.GetComponent<Renderer>().material.color = startColor;
        }
    }

    private bool TransformCheck()
    {
        if (positionTask == true)
        {
            if (target != null)
            {
                bool newState = WithinTargetPosition(transform.position, targetPosition);

                //Check if they completed the task of positioning the object correctly and store the time it took if they did
                if (newState && !lastTransformState)
                {
                    transformCompletionTime = Time.time - taskStartTime;
                }

                lastTransformState = newState;
                return newState;
            }
            else
            {
                print("target is null");
                return false;
            }
        }
        else { return true; }
    }

    private bool WithinTargetPosition(Vector3 oPos, Vector3 tPos)
    {
        bool xWithin = false;
        bool yWithin = false;
        bool zWithin = false;
        if (oPos.x >= tPos.x - positionOffset && oPos.x <= tPos.x + positionOffset) { xWithin = true; }
        if (oPos.y <= tPos.y + positionOffset && oPos.y > tPos.y - positionOffset) { yWithin = true; }
        if (oPos.z >= tPos.z - positionOffset && oPos.z <= tPos.z + positionOffset) { zWithin = true; }
        //Debug.Log("xwithin: " + xWithin);
        //Debug.Log("ywithin: " + yWithin);
        //Debug.Log("zwithin: " + zWithin);
        if (xWithin && yWithin && zWithin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RotationCheck()
    {
        if (rotationTask == true)
        {
            if (target != null)
            {
                bool newState = WithinTargetRotation(transform.rotation.eulerAngles, targetRotation);

                //Check if they completed the task of rotate the object correctly and store the time it took if they did
                if (newState && !lastRotationState)
                {
                    rotateCompletionTime = Time.time - taskStartTime;
                }

                return newState;
            }
            else
            {
                print("target is null");
                return false;
            }
        }
        else { return true; }
    }

    private bool WithinTargetRotation(Vector3 oRot, Vector3 tRot)
    {
        bool xWithin = false;
        bool yWithin = false;
        bool zWithin = false;
        if (oRot.x < tRot.x + rotationOffset && oRot.x > tRot.x - rotationOffset) { xWithin = true; }
        if (oRot.y < tRot.y + rotationOffset && oRot.y > tRot.y - rotationOffset) { yWithin = true; }
        if (oRot.z < tRot.z + rotationOffset && oRot.z > tRot.z - rotationOffset) { zWithin = true; }

        if (xWithin && yWithin && zWithin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ScaleCheck()
    {
        if (scaleTask == true)
        {
            if (target != null)
            {
                bool newState = WithinTargetScale(transform.localScale, targetScale);

                if (newState && !lastScaleState)
                {
                    scaleCompletionTime = Time.time - taskStartTime;
                }

                return newState;
            }
            else
            {
                print("target is null");
                return false;
            }
        }
        else { return true; }
    }

    private bool WithinTargetScale(Vector3 oScale, Vector3 tScale)
    {
        bool xWithin = false;
        bool yWithin = false;
        bool zWithin = false;
        if (oScale.x < tScale.x + scaleOffset && oScale.x > tScale.x - scaleOffset) { xWithin = true; }
        if (oScale.y < tScale.y + scaleOffset && oScale.y > tScale.y - scaleOffset) { yWithin = true; }
        if (oScale.z < tScale.z + scaleOffset && oScale.z > tScale.z - scaleOffset) { zWithin = true; }

        if (xWithin && yWithin && zWithin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StoreDataInFile()
    {
        string nameOfFile = "testtext.txt";

        //If the files does not exist
        if (!File.Exists(nameOfFile))
        {
            // Create a file to write to.
            string headline = "Completion Time" + Environment.NewLine;
            File.WriteAllText(nameOfFile, headline);
        }
        //This line is added and makes the file longer
        //Added participant number somewhere
        string appendText = "Transform Time: " + transformCompletionTime + ", Rotation Time: " + rotateCompletionTime + ", Scale Time: " + scaleCompletionTime + Environment.NewLine;
        File.AppendAllText(nameOfFile, appendText);
    }
}
