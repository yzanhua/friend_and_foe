using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSoundController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip bell_clip;
    public AudioClip bombSound;

    public bool isBell = false;
    private bool bell_played = false;

    public bool isBombSound = false;
    private bool bomb_played = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SoundManager.instance == null)
            return;

        if (isBell && !bell_played)
        {
            SoundManager.instance.PlaySound("bomb_bell");
            bell_played = true;
        }
          
        if (isBombSound && !bomb_played)
        {
            StartCoroutine(PlayBombExplodeClip());
            bomb_played = true;
        }
    }

    private IEnumerator PlayBombExplodeClip()
    {
        for (int i = 0; i < 55; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SoundManager.instance.PlaySound("bomb_explode");
        }
        
    }
}
