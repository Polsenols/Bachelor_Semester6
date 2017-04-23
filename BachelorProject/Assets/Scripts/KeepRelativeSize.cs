using UnityEngine;

public class KeepRelativeSize : MonoBehaviour
{
    public Camera cam;
    public  float targetRadius = 25.0f;

    // set the initial scale, and setup reference camera
    void Start()
    {
        // if no specific camera, grab the default camera
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        float dist = (cam.transform.position - transform.position).magnitude;

        float diameter = ((targetRadius * 2) * dist * cam.fieldOfView) / (Mathf.Rad2Deg * Screen.height);
        transform.localScale = new Vector3(diameter, diameter, diameter);
    }
}