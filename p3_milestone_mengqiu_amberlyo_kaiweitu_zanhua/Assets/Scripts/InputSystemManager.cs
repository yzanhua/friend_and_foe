using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Input;

public class InputSystemManager : ScriptableObject
{
    private static InputSystemManager _instance = null;
    public float deadZoneValue = 0.2f;


    private List<Gamepad> _gamepad_list;

    public static InputSystemManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance(typeof(InputSystemManager)) as InputSystemManager;
            }
            return _instance;
        }
    }

    void Awake()
    {
        _gamepad_list = new List<Gamepad>(Gamepad.all);

    }


    static public Gamepad GetGamePad(int playerID) 
    {
        Gamepad gp = playerID >= instance._gamepad_list.Count ? null : instance._gamepad_list[playerID];
        return gp;
    }

    static public int GetGamePadNum()
    {
        return instance._gamepad_list.Count;
    }

    static public float GetRightSVertical(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return instance.GetDeadZoneValue(gp.rightStick.y.ReadValue());
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return 0f;
        }

    }

    static public float GetRightSHorizontal(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return instance.GetDeadZoneValue(gp.rightStick.x.ReadValue());
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return 0f;
        }
    }

    static public float GetLeftSVertical(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return instance.GetDeadZoneValue(gp.leftStick.y.ReadValue());
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return 0f;
        }
    }

    static public float GetLeftSHorizontal(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return instance.GetDeadZoneValue(gp.leftStick.x.ReadValue());
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return 0f;
        }
    }

    static public bool GetAction1(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return gp.aButton.wasPressedThisFrame;
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return false;
        }
    }

    static public bool GetAction2(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return gp.bButton.wasPressedThisFrame;
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return false;
        }
    }

    static public bool GetAction3(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return gp.xButton.wasPressedThisFrame;
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return false;
        }
    }

    static public bool GetAction4(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return gp.yButton.wasPressedThisFrame;
        }
        else
        {
            //Debug.Log("Unknown player ID");
            return false;
        }
    }

    static public bool GetRightShoulder(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return gp.rightShoulder.wasPressedThisFrame;
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return false;
        }
    }

    static public bool GetLeftShoulder(int playerID)
    {
        Gamepad gp = GetGamePad(playerID);

        if (gp != null)
        {
            return gp.leftShoulder.wasPressedThisFrame;
        }
        else
        {
            //Debug.LogError("Unknown player ID");
            return false;
        }
    }


    private float GetDeadZoneValue(float value)
    {
        return Mathf.Abs(value) < deadZoneValue ? 0f : value;
    }

}
