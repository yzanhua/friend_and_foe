// credit: https://answers.unity.com/questions/1142089/moving-camera-with-2-players.html
// Author: TreyH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// requirement:
// 1): initial positions of two submarines (proxy):
//     a): z-value are both 0
//     b): Midpoint initially at (0,0,0)
// 2): initial Camera's orthographicSize = 5f
// 3): initial Camera's z-position = -10f;
public class CameraController : MonoBehaviour
{

    // How many units should we keep from the players
    public float zoomFactor = 0.8f;
    public float followTimeDelta = 0.8f;

    public Transform submarine1;
    public Transform submarine2;

    private Camera cam;
    private float init_distance;

    private void Start()
    {
        cam = GetComponent<Camera>();
        init_distance = (submarine1.position - submarine2.position).magnitude;
    }

    private void Update()
    {
        FollowSubmarines();
    }

    private void FollowSubmarines()
    {
        // Midpoint we're after
        Vector3 midpoint = (submarine1.position + submarine2.position) / 2f;

        // Distance between objects
        float distance = (submarine1.position - submarine2.position).magnitude;

        // Move camera a certain distance
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;
        if (cameraDestination.z > -10f)
            cameraDestination.z = -10f;

        //cameraDestination += Vector3.forward * 10f;

        // Adjust ortho size if we're using one of those
        if (cam.orthographic)
        {
            // The camera's forward vector is irrelevant, only this size will matter
            cam.orthographicSize = distance / init_distance * 5f;
            if (cam.orthographicSize < 5f)
                cam.orthographicSize = 5f;
        }
        // You specified to use MoveTowards instead of Slerp
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }

}
