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
    bool playerTrigger = false;
    PlayerInputController playerInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTrigger)
        {
            if (!keyDown && playerInput.inputDevice.Action2)
                keyDown = true;
            if (keyDown)
            {
                curFilledTime += Time.deltaTime;
                print(curFilledTime);
            }
        }
        //finished filling
        if (curFilledTime >= refillTime)
        {
            curFilledTime = 0;
            keyDown = false;
            weapon.GetComponent<WeaponController>().fillBullets();

        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player")){
            playerInput = other.GetComponent<PlayerInputController>();
            playerTrigger = true;
            if (!keyDown && playerInput.inputDevice.Action2)
                keyDown = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;
            keyDown = false;
        }
      
    }
}
