using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Credit: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager?playlist=17150
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.

    private Dictionary<string, float> volumeMap;             

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        volumeMap = new Dictionary<string, float>();
        foreach (Transform c in transform)
        {
            volumeMap.Add(c.name, c.gameObject.GetComponent<AudioSource>().volume);
        }

    }
    

    //Used to play single sound clips.
    public void PlaySound(string s)
    {
        Transform child_transform = transform.Find(s);
        if (child_transform == null)
        {
            Debug.LogWarning("WARNING: no such kind of audio clip");
            return;
        }
        AudioSource temp = child_transform.GetComponent<AudioSource>();
        if (temp == null)
        {
            Debug.LogWarning("WARNING: audio source component not found");
            return;
        }

        temp.Play();
    }

    public void StopSound(string s)
    {
        Transform child_transform = transform.Find(s);
        if (child_transform == null)
        {
            Debug.LogWarning("WARNING: no such kind of audio clip");
            return;
        }
        AudioSource temp = child_transform.GetComponent<AudioSource>();
        if (temp == null)
        {
            Debug.LogWarning("WARNING: audio source component not found");
            return;
        }

        if (temp.isPlaying)
        {
            temp.Stop();
        }
    }

    public void SoundTransition (string fadeout, string fadein, float fadeRate = 0.5f)
    {
        StartCoroutine(FadeInout(fadeout, fadein, fadeRate));
    }

    IEnumerator FadeInout(string fadeout, string fadein, float fadeRate)
    {
        Transform child_transform_out= transform.Find(fadeout);
        Transform child_transform_in = transform.Find(fadein);

        AudioSource fade_out = child_transform_out.GetComponent<AudioSource>();
        AudioSource fade_in = child_transform_in.GetComponent<AudioSource>();

        if (fade_out.isPlaying)
        {
            fade_in.Play();
            fade_in.volume = 0f;
            while (fade_out.volume > 0.1f || fade_in.volume < volumeMap[fadein])
            {
                fade_out.volume = Mathf.Lerp(fade_out.volume, 0.0f, fadeRate * Time.deltaTime);
                fade_in.volume = Mathf.Lerp(fade_in.volume, 1.0f, fadeRate * Time.deltaTime);
                yield return null;
            }

            // Close enough, turn it off!
            fade_out.volume = 0.0f;
            fade_out.Stop();
            fade_in.volume = volumeMap[fadein];
        }
        else if (!fade_in.isPlaying)
        {
            fade_in.Play();
            fade_in.volume = volumeMap[fadein];
        }
    }

}