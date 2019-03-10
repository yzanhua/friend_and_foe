using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public int ID = 0;
    public InputDevice inputDevice;

    [Range(0f, 10f)]
    public float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        inputDevice = (InputManager.Devices.Count > ID) ? InputManager.Devices[ID] : null;
    }
}
