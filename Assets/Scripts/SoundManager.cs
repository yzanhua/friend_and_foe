using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Credit: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager?playlist=17150
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             

    private Hashtable _current_playing = new Hashtable();
    private int _next_ID = 0;
    private List<int> _available_ID = new List<int>();

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
    public int PlaySound(string s)
    {
        Transform child_transform = transform.Find(s);
        if (child_transform == null)
        {
            Debug.LogWarning("WARNING: no such kind of audio clip");
            return -1;
        }
        AudioSource temp = child_transform.GetComponent<AudioSource>();
        if (temp == null)
        {
            Debug.LogWarning("WARNING: audio source component not found");
            return -1;
        }
        temp.Play();

        int ID = -1;
        if (_available_ID.Count != 0)
        {
            ID = _available_ID[_available_ID.Count - 1];
        }
        else
        {
            ID = _next_ID;
            _next_ID++;
        }
        _current_playing.Add(ID, temp);

        return ID;
    }

    public void StopSound(int ID)
    {
        if (_current_playing.Contains(ID))
        {
            AudioSource aS = (AudioSource)_current_playing[ID];
            aS.Stop();
            _current_playing.Remove(ID);
        }
    }

}