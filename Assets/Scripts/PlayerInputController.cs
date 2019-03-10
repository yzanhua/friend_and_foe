using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public int ID = 0;

    [Range(0f, 10f)]
    public float speed = 2f;

    private bool _vertical_movement;

    // Update is called once per frame
    void Update()
    {
        var inputDevice = (InputManager.Devices.Count > ID) ? InputManager.Devices[ID] : null;

        if (inputDevice != null)
        {
            float vertical = _vertical_movement ? inputDevice.DPadY : 0f;
            transform.position += new Vector3(inputDevice.DPadX, vertical, 0) * Time.deltaTime * speed;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Contains("ladder"))
        {
            _vertical_movement = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag.Contains("ladder"))
        {
            _vertical_movement = false;
        }
    }
}
