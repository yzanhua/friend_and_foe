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
        SoundManager.instance.PlaySound("main_scene_background");
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystemManager.GetAction4(0))
        {
            // Initiate.Fade("Selection", Color.black, 2.0f);
            TransitionManager.Instance.TransitionOutAndLoadScene("Selection");
        }
    }
}
