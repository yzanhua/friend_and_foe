using UnityEngine;
using System.Collections;
using Rewired;

public class InputSystemManager : ScriptableObject
{
    private static InputSystemManager _instance = null;
    public float deadZoneValue = 0.2f;

    public int[] PlayerID2GamePadID = new int[] { 0, 1, 2, 3 };
    private Player[] players = new Player[4];

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
        for (int i = 0; i < 4; i++)
            players[i] = ReInput.players.GetPlayer(i);
    }

    static public float GetRightSVertical(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return 0f;
        return instance.GetDeadZoneValue(instance.players[newID].GetAxis("RightStickY"));
    }

    static public float GetRightSHorizontal(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return 0f;
        return instance.GetDeadZoneValue(instance.players[newID].GetAxis("RightStickX"));
    }

    static public float GetLeftSVertical(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return 0f;
        return instance.GetDeadZoneValue(instance.players[newID].GetAxis("LeftStickY"));
    }

    static public float GetLeftSHorizontal(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return 0f;
        return instance.GetDeadZoneValue(instance.players[newID].GetAxis("LeftStickX"));
    }

    static public void SetVibration(int playerID, float motorLevel, float duration)
    {
        if (playerID == -1)
        {
            for (int i = 0; i < Global.instance.numOfPlayers; i++)
                instance.players[i].SetVibration(0, motorLevel, duration);
            return;
        }

        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return;
        instance.players[newID].SetVibration(0, motorLevel, duration);
    }

    static public void SetVibrationBySubmarine(int submarineID, float motorLevel, float duration)
    {
        int[] IDs =  new int[] { (submarineID + 1) % 2, 2 + (submarineID + 1) % 2 };
        foreach(int playerID in IDs)
        {
            int newID = instance.PlayerID2GamePadID[playerID];
            if (newID < 0) return;
            instance.players[newID].SetVibration(0, motorLevel, duration);
        }
    }

    static public bool GetAction1(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("ActionButton1");
    }

    static public bool GetAction2(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("ActionButton2");
    }

    static public bool GetAction2Hold(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButton("ActionButton2");
    }

    static public bool GetAction3(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("ActionButton3");
    }

    static public bool GetAction4(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("ActionButton4");
    }

    static public bool GetMenuButton(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("Menu");
    }

    static public bool GetRightShoulder1(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("R1");
    }

    static public bool GetRightShoulder2(int playerID)
    {
        int newID = instance.PlayerID2GamePadID[playerID];
        if (newID < 0) return false;
        return instance.players[newID].GetButtonDown("R2");
    }

    //static public bool GetRightShoulderTrigger(int playerID)
    //{
    //    int newID = instance.PlayerID2GamePadID[playerID];
    //    if (newID < 0) return false;
    //    return instance.players[newID].GetButtonDown("RightShoulderTrigger");
    //}

    //static public bool GetLeftShoulderButton(int playerID)
    //{
    //    int newID = instance.PlayerID2GamePadID[playerID];
    //    if (newID < 0) return false;
    //    return instance.players[newID].GetButtonDown("LeftShoulderButton");
    //}

    //static public bool GetLeftShoulderTrigger(int playerID)
    //{
    //    int newID = instance.PlayerID2GamePadID[playerID];
    //    if (newID < 0) return false;
    //    return instance.players[newID].GetButtonDown("LeftShoulderTrigger");
    //}



    private float GetDeadZoneValue(float value)
    {
        return Mathf.Abs(value) < deadZoneValue ? 0f : value;
    }

}
