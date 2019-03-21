using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;


public class InputSystemSample : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < InputSystemManager.GetGamePadNum(); i++)
        {
            Gamepad active_gamepad = InputSystemManager.GetGamePad(i);
            float horizontal_val = active_gamepad.leftStick.x.ReadValue();
            float vertical_val = active_gamepad.leftStick.y.ReadValue();

            bool a_pressed_this_frame = active_gamepad.rightShoulder.wasPressedThisFrame;
            bool b_pressed_this_frame = active_gamepad.bButton.wasPressedThisFrame;


            string header = "[" + active_gamepad.name + "]: ";
            if (a_pressed_this_frame)
            {
                Debug.Log(header + "A pressed");
                Debug.Log(header + horizontal_val);
                Debug.Log(header + vertical_val);
            }
            else if (b_pressed_this_frame)
            {
                Debug.Log(header + "B Pressed");
                Debug.Log(header + active_gamepad.rightTrigger.ReadValue());
            }
        }


    }
}
