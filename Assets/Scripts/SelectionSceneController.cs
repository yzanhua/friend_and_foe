﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.SoundTransition("background_battle", "main_scene_background");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
