using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillController : MonoBehaviour
{
    public float refillTime = 2.0f;
    public GameObject weapon;


    float curFilledTime = 0;

    // whether key was pressed in this collision period
    bool keyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //finished filling
        if (curFilledTime >= refillTime)
        {
            curFilledTime = 0;
            keyDown = false;
            weapon.GetComponent<WeaponController>().fillBullets();

        }
    }


    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Player")){
            PlayerInputController inputController = other.GetComponent<PlayerInputController>();
            if (!keyDown && inputController.inputDevice.Action2)
                keyDown = true;
            if (keyDown)
            {
                curFilledTime += Time.deltaTime;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Player"))
        {
            keyDown = false;
        }
    }
}
