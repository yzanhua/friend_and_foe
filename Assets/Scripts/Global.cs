using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : ScriptableObject
{
    private static Global _instance = null;
 
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

    public bool AllPlayersMovementEnable = true;

    public bool[] SelectedStatus = new bool[] {false, false, false, false};
    public int[] PlayerID2GamePadID = new int[] {-1, -1, -1, -1};
    public bool SelectionEnable = true;


}
