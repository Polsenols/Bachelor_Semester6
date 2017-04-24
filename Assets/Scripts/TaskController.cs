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
    private ScaleTask scaleT;
    private PositionTask posT;
    private RotationTask rotT;

    // Use this for initialization
    void Start()
    {
        startColor = gameObject.GetComponent<Renderer>().material.color;

        posT = gameObject.GetComponent<PositionTask>();
        rotT = gameObject.GetComponent<RotationTask>();
        scaleT = gameObject.GetComponent<ScaleTask>();

        posT.SetTarget(target);
        rotT.SetTarget(target);
        scaleT.SetTarget(target);

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
            StoreDataInFile();
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
            if (posT != null)
            {
                bool newState = posT.GetTransformSuccess();

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
                print("Position task script is null");
                return false;
            }
        }
        else { return true; }
    }

    private bool RotationCheck()
    {
        if (rotationTask == true)
        {
            if (rotT != null)
            {
                bool newState = rotT.GetRotationSuccess();

                //Check if they completed the task of rotate the object correctly and store the time it took if they did
                if (newState && !lastRotationState)
                {
                    rotateCompletionTime = Time.time - taskStartTime;
                }

                return newState;
            }
            else
            {
                print("Rotation task script is null");
                return false;
            }
        }
        else { return true; }
    }

    private bool ScaleCheck()
    {
        if (scaleTask == true)
        {
            if (scaleT != null)
            {
                bool newState = scaleT.GetScaleSuccess();

                if (newState && !lastScaleState)
                {
                    scaleCompletionTime = Time.time - taskStartTime;
                }

                return newState;
            }
            else
            {
                print("Scale task script is null");
                return false;
            }
        }
        else { return true; }
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
