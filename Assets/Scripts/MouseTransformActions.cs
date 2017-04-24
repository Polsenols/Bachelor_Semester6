using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTransformActions : MonoBehaviour {

    public int totalActions = 0;
    public int positionActions = 0;
    public int rotationActions = 0;
    public int scaleActions = 0;

    private Vector3 currentPosition;
    private Vector3 currentRotation;
    private Vector3 currentScale;
	// Use this for initialization
	void Start ()
    {
        currentPosition = gameObject.transform.position;
        currentRotation = gameObject.transform.rotation.eulerAngles;
        currentScale = gameObject.transform.localScale;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(currentPosition != gameObject.transform.position)
            {
                currentPosition = gameObject.transform.position;
                positionActions++;
                totalActions++;
            }

            if (currentRotation != gameObject.transform.rotation.eulerAngles)
            {
                currentRotation = gameObject.transform.rotation.eulerAngles;
                rotationActions++;
                totalActions++;
            }

            if (currentScale != gameObject.transform.localScale)
            {
                currentScale = gameObject.transform.localScale;
                scaleActions++;
                totalActions++;
            }
        }
	}
}
