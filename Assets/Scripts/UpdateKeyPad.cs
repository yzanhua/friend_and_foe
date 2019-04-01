using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeyPad : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        InputSystemManager.UpdateGamePad();
    }
}
