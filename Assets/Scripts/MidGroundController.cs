using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidGroundController : MonoBehaviour
{
    Vector3 camera_origin_position;
    Vector3 midground_origin_position;

    // Start is called before the first frame update
    void Start()
    {
        camera_origin_position = Camera.main.transform.position;
        midground_origin_position = transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = midground_origin_position + (Camera.main.transform.position - camera_origin_position);
    }
}
