using UnityEngine;
using System.Collections;

// Credit: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager?playlist=17150
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             

    public AudioClip shoot_clip;
    public AudioClip move_clip;
    public AudioClip bubble_generate_clip;
    public AudioClip bubble_break_clip;
    public AudioClip hit_clip;
    public AudioClip win_clip;
    public AudioClip warning_clip;
    public AudioClip collide_clip;
    public AudioClip bell_clip;
    public AudioClip bomb_explode_clip; 

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
    public void PlaySound(string s, Vector3? pos = null)
    {
        Vector3 _pos;
        if (!pos.HasValue)
            _pos = Camera.main.transform.position;
        else _pos = pos.Value;

        switch (s)
        {
            case "win":
                AudioSource temp = transform.Find("win").GetComponent<AudioSource>();
                temp.Play();
                break;
            case "shoot":
                AudioSource.PlayClipAtPoint(shoot_clip, _pos);
                break;
            case "move":
                AudioSource.PlayClipAtPoint(move_clip, _pos);
                break;
            case "bubble_generate":
                AudioSource.PlayClipAtPoint(bubble_generate_clip, _pos);
                break;
            case "bubble_break":
                AudioSource.PlayClipAtPoint(bubble_generate_clip, _pos);
                break;
            case "hit":
                AudioSource.PlayClipAtPoint(hit_clip, _pos);
                break;
            case "warning":
                AudioSource.PlayClipAtPoint(warning_clip, _pos);
                break;
            case "collide":
                AudioSource.PlayClipAtPoint(collide_clip, _pos);
                break;
            case "bomb_bell":
                AudioSource.PlayClipAtPoint(bell_clip, _pos);
                break;
            case "bomb_explode":
                AudioSource.PlayClipAtPoint(bomb_explode_clip, _pos);
                break;
            default:
                Debug.LogWarning("WARNING: no such kind of audio clip");
                break;
        }

    }

}