using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public int ID = 0;

    [Range(0f, 10f)]
    public float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        var inputDevice = (InputManager.Devices.Count > ID) ? InputManager.Devices[ID] : null;

        if (inputDevice != null)
        {
            transform.position += new Vector3(inputDevice.DPadX, inputDevice.DPadY, 0) * Time.deltaTime * speed;   
        }
    }
}
