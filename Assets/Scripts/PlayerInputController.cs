using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public int ID = 0;

    [Range(0f, 10f)]
    public float speed = 2f;


    private Animator an;

    void Start()
    {
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var inputDevice = (InputManager.Devices.Count > ID) ? InputManager.Devices[ID] : null;

        if (inputDevice != null)
        {
            an.speed = 1.0f;

            if (System.Math.Abs(inputDevice.DPadX) > 0.1f)
            {
                an.SetFloat("horizontal", inputDevice.DPadX);
                an.SetFloat("vertical", 0f);
                transform.position += new Vector3(inputDevice.DPadX, 0, 0) * Time.deltaTime * speed;
            }
            else if (System.Math.Abs(inputDevice.DPadY) > 0.1f)
            {
                an.SetFloat("vertical", inputDevice.DPadY);
                an.SetFloat("horizontal", 0f);
                transform.position += new Vector3(0, inputDevice.DPadY, 0) * Time.deltaTime * speed;
            }
            else
            {
                an.speed = 0.0f;
            }
        }

    }

}
