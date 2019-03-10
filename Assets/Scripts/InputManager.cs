using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Public field of the InputManager
    public static InputManager instance;

    public enum Mode {PS4, XBOX};

    public static Mode InputMode;


    // Private field of the InputManager
    [System.Serializable]
    private class KeyMap
    {
        public string Type;
        public string Platform;
        public int RUpButton;
        public int RDownButton;
        public int RLeftButton;
        public int RRightButton;
        public string RHorizontal;
        public string RVertical;
        public string LHorizontal;
        public string LVertical;
    }

    private KeyMap[] _km;
    private string _jsonFilePath = "Assets/KeyMap.json";

    // Public Method of the InputManager
    public static bool GetRUpButton(int ID)
    {
        return Input.GetButtonDown("J_" + ID + "_" + instance._km[(int)InputMode].RUpButton);
    }

    public static bool GetRDownButton(int ID)
    {
        return Input.GetButtonDown("J_" + ID + "_" + instance._km[(int)InputMode].RDownButton);
    }

    public static bool GetRLeftButton(int ID)
    {
        return Input.GetButtonDown("J_" + ID + "_" + instance._km[(int)InputMode].RLeftButton);
    }

    public static bool GetRRightButton(int ID)
    {
        return Input.GetButtonDown("J_" + ID + "_" + instance._km[(int)InputMode].RRightButton);
    }

    public static float GetRHorizontal(int ID)
    {
        return Input.GetAxisRaw("J_A_" + ID + "_" + instance._km[(int)InputMode].RHorizontal);
    }

    public static float GetRVertical(int ID)
    {
        return Input.GetAxisRaw("J_A_" + ID + "_" + instance._km[(int)InputMode].RVertical);
    }

    public static float GetLHorizontal(int ID)
    {
        return Input.GetAxisRaw("J_A_" + ID + "_" + instance._km[(int)InputMode].LHorizontal);
    }

    public static float GetLVertical(int ID)
    {
        return Input.GetAxisRaw("J_A_" + ID + "_" + instance._km[(int)InputMode].LVertical);
    }


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        // Initialize the controller mode
        InputMode = InputManager.Mode.PS4;
        // Read in the KeyMap file
        StreamReader reader = new StreamReader(_jsonFilePath);
        string jsonString = reader.ReadToEnd();
        reader.Close();
        _km = JsonHelper.FromJson<KeyMap>(jsonString);
    }

}
