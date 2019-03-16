using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;


public class InputSystemTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Gamepad active_gamepad = InputSystemManager.GetGamePad(0);

        float horizontal_val = active_gamepad.leftStick.x.ReadValue();
        float vertical_val = active_gamepad.leftStick.y.ReadValue();

        bool a_pressed_this_frame = active_gamepad.aButton.wasPressedThisFrame;
        bool b_pressed_this_frame = active_gamepad.bButton.wasPressedThisFrame;

        
        if (a_pressed_this_frame) 
        {
            Debug.Log("A pressed");
            Debug.Log(horizontal_val);
            Debug.Log(vertical_val);
        } else if (b_pressed_this_frame)
        {
            Debug.Log("B Pressed");
        }

    }
}
