using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour {

    public Transform target, taskObj;

    public bool positionTask;
    public bool rotationTask;
    public bool scaleTask;

    public Color successColor;

    //Success states
    private bool transformSuccess;
    private bool rotateSuccess;
    private bool scaleSucess;

    private Color startColor;
    private ScaleTask scaleT;
    private PositionTask posT;
    private RotationTask rotT;

	// Use this for initialization
	void Start ()
    {


        posT = gameObject.GetComponent<PositionTask>();
        rotT = gameObject.GetComponent<RotationTask>();
        scaleT = gameObject.GetComponent<ScaleTask>();

        posT.SetTaskObject(taskObj);
        posT.SetTarget(target);
        rotT.SetTarget(target);
        scaleT.SetTarget(target);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Check success of manipulations
        transformSuccess = TransformCheck();
        rotateSuccess = RotationCheck();
        scaleSucess = ScaleCheck();
        /*
        if (transformSuccess && rotateSuccess && scaleSucess)
        {
            gameObject.GetComponent<Renderer>().material.color = successColor;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = startColor;
        }

        //If non of the tasks are picked
        if(!positionTask && !rotationTask && !scaleTask)
        {
            gameObject.GetComponent<Renderer>().material.color = startColor;
        }*/
	}

    private bool TransformCheck()
    {
        if (positionTask == true)
        {
            if(posT != null)
            {
                return posT.GetTransformSuccess();
            }
            else
            {
                print("Position task script is null");
                return false;
            }
        }
        else{ return true; }       
    }

    private bool RotationCheck()
    {
        if (rotationTask == true)
        {
            if (rotT != null)
            {
                return rotT.GetRotationSuccess();
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
                return scaleT.GetScaleSuccess();
            }
            else
            {
                print("Scale task script is null");
                return false;
            }
        }
        else { return true; }
    }
}
