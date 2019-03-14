using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    public static CameraShakeEffect instance;

    private Vector3 _original_pos;
    private bool isShake = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

    }

    public static void ShakeCamera(float duration, float amount)
    {
        if (instance.isShake)
            return;
        instance._original_pos = instance.transform.position;
        instance.StartCoroutine(instance.Shake(duration, amount));
    }

    IEnumerator Shake(float duration, float amount)
    {
        float endTime = Time.time + duration;
        isShake = true;

      
        while (Time.time < endTime)
        {
            transform.position = _original_pos + Random.insideUnitSphere * amount;

            //duration -= Time.deltaTime;

            yield return null;
        }
        isShake = false;

        transform.position = _original_pos;
    }

}
