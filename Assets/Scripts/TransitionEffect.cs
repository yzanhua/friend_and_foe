using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffect : MonoBehaviour
{
    public static TransitionEffect instance;

    // Use this for initialization
    public Image dark_mask;
    public Canvas canvas;

    public static bool isTransition = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
            instance = this;
    }


    public static void TriggerDarkTransition()
    {

        if (!isTransition)
            instance.StartCoroutine(instance.StartTransition());

    }

    IEnumerator StartTransition()
    {
        isTransition = true;
        Image mask = Instantiate(dark_mask, canvas.transform) as Image;
        while (mask.color.a < 1)
        {
            Color temp_color = mask.color;
            temp_color.a += 0.03f;
            mask.color = temp_color;
            yield return null;
        }


        while (mask.color.a > 0)
        {
            Color temp_color = mask.color;
            temp_color.a -= 0.03f;
            mask.color = temp_color;
            yield return null;
        }
        isTransition = false;
        // GetComponent<KeyController>().AllControlsEnable = true;
        // GetComponent<BoxCollider>().isTrigger = false;
        Destroy(mask);

    }
}
