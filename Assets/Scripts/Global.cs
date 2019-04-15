﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : ScriptableObject
{
    private static Global _instance = null;

    public float maxScreenSize = 12f;
    public bool GameEndCustomizeScreen = false;
    public bool CameraShake;

    public bool AllPlayersMovementEnable = true;
    public bool isGameEnd = false;
    public bool[] SelectedStatus = new bool[] { false, false, false, false };
    public int[] PlayerID2GamePadID = new int[] { -1, -1, -1, -1 };
    public bool SelectionEnable = true;
    public int numOfPlayers = 4;

    public bool bombCreate = true;
    public bool cameraControl = true;
    public bool godMode = false;

    public bool AutomatedShiled = true;
    public bool[] ExtraSkillEnable = new bool[] {false, false};
    public bool[] ExtraSkillEnableDown = new bool[] {false, false};

    public static Global instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance(typeof(Global)) as Global;
            }
            return _instance;
        }
    }

    public static void initializeVariables()
    {
        instance.PlayerID2GamePadID = new int[] { -1, -1, -1, -1 };
        instance.SelectedStatus = new bool[] { false, false, false, false };
        instance.SelectionEnable = true;
        instance.AllPlayersMovementEnable = true;
        instance.isGameEnd = false;
        instance.godMode = false;
        instance.cameraControl = true;
        instance.bombCreate = true;
        instance.GameEndCustomizeScreen = false;
        InputSystemManager.instance.PlayerID2GamePadID = new int[] { 0, 1, 2, 3 }; 
        instance.ExtraSkillEnable = new bool[] { false, false };
        instance.ExtraSkillEnableDown = new bool[] { false, false };
    }
}
