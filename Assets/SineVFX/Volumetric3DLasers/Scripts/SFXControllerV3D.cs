using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXControllerV3D : MonoBehaviour
{
    public int submarineID = 0;
    public AudioSource loopingSFX;
    public GameObject[] waveSfxPrefabs;

    private float globalProgress;

    public void SetGlobalProgress(float gp)
    {
        globalProgress = gp;
    }

    void Update()
    {
        if (Global.instance.ExtraSkillEnableDown[submarineID])
        {
            Instantiate(waveSfxPrefabs[Random.Range(0, waveSfxPrefabs.Length)], transform.position, transform.rotation);
        }

        loopingSFX.volume = globalProgress;
    }
}
