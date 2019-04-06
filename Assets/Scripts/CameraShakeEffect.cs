using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    public static CameraShakeEffect instance;
    Transform cam;

    public float k = 0.1f;
    public float dampening_factor = 0.1f;
    public float debug_displacement = 1.0f;

    private bool _isShake;

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

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        cam = transform.Find("Main Camera");
        // StartCoroutine(Shake());
    }

    Vector3 _velocity = Vector3.zero;

    public static void Bump(float amount)
    {
        //instance.velocity += new Vector3(force.x, force.y, 0) * 10;
        instance._velocity += Random.onUnitSphere * amount;
        instance._velocity.z = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.instance.GameEndCustomizeScreen)
            return;
        Vector3 d = -cam.localPosition;
        Vector3 acceleration = k * d;

        Vector3 vel_delta = acceleration - _velocity * (1.0f - dampening_factor);
        _velocity += vel_delta * Time.deltaTime * Application.targetFrameRate;

        cam.localPosition += _velocity;
    }

    public static void ShakeCamera(float duration, float amount)
    {
        if (instance._isShake)
            return;
        // instance._original_pos = instance.transform.position;
        instance.StartCoroutine(instance.Shake(duration, amount));
    }


    IEnumerator Shake(float duration, float amount)
    {
        float endTime = Time.time + duration;
        _isShake = true;


        while (Time.time < endTime)
        {
            // transform.position = _original_pos + Random.insideUnitSphere * amount;
            Bump(amount);
            yield return null;
        }
        _isShake = false;

    }
}
