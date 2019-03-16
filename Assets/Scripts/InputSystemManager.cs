using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Input;

public class InputSystemManager : MonoBehaviour
{
    static public InputSystemManager instance;


    private List<Gamepad> _gamepad_list;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _gamepad_list = new List<Gamepad>(Gamepad.all);
    }


    static public Gamepad GetGamePad(int playerID) 
    {
        Gamepad gp = playerID >= instance._gamepad_list.Count ? null : instance._gamepad_list[playerID];
        return gp;
    }
}
