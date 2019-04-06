using UnityEngine;
using System.Collections;

// Credit: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager?playlist=17150
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             

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

}