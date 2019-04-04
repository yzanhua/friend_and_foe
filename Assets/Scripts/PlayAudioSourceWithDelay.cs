using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSourceWithDelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child_transform in transform)
        {
            AudioSource audioSource = child_transform.gameObject.GetComponent<AudioSource>();
            float delay = child_transform.gameObject.GetComponent<ParticleSystem>().main.startDelay.constant;
            audioSource.PlayDelayed(delay);
        }
    }
}
