using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillController : MonoBehaviour
{
    public float refillTime = 2.0f;
    public GameObject weapon;
    public GameObject progress_bar;

    float curFilledTime = 0;
    // whether key was pressed in this collision period
    bool keyDown = false;
    bool playerTrigger = false;
    bool bulletEmpty = true;

    PlayerInputController playerInput;

    public void BulletNeedRefill()
    {
        bulletEmpty = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        progress_bar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!bulletEmpty)
            return;
        if (playerTrigger)
        {
            if (!keyDown && playerInput.inputDevice.Action2)
                keyDown = true;
            if (keyDown)
            {
                if (curFilledTime == 0)
                {
                    progress_bar.SetActive(true);
                }
                curFilledTime += Time.deltaTime;
                progress_bar.GetComponent<HealthBar>().SetSize((float)curFilledTime / (float)refillTime);
                print(curFilledTime);
            }
        }
        //finished filling
        if (curFilledTime >= refillTime)
        {
            curFilledTime = 0;
            keyDown = false;
            weapon.GetComponent<WeaponController>().fillBullets();
            progress_bar.SetActive(false);
            bulletEmpty = false;

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
