using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeautifulTransitions.Scripts.Transitions.Components;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.SoundTransition("background_battle", "main_scene_background", 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystemManager.GetAction3(0))
        {
            TransitionManager.Instance.TransitionOutAndLoadScene("Ready");
        }
    }
}
