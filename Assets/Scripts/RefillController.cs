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
    public bool keyDown = false;
    public bool playerTrigger = false;
    public bool bulletEmpty = true;

    int playerID;

    public void SetBulletStatus(bool status)
    {
        bulletEmpty = !status;
    }

    void Start()
    {
        progress_bar.SetActive(false);
    }

    void Update()
    {
        if (!bulletEmpty)
            return;

        if (playerTrigger)
        {
            if (!keyDown && InputSystemManager.GetAction2(playerID))
                keyDown = true;
            if (keyDown)
            {
                if (curFilledTime <= 0)
                {
                    progress_bar.SetActive(true);
                }
                curFilledTime += Time.deltaTime;
                progress_bar.GetComponent<HealthBar>().SetSize((float)curFilledTime / (float)refillTime);
            }
        }
        //finished filling
        if (curFilledTime >= refillTime)
        {
            curFilledTime = 0;
            keyDown = false;
            weapon.GetComponent<WeaponController>().FillBullets();
            progress_bar.SetActive(false);
            bulletEmpty = false;
            if (TutorialManager.instance.tutorialMode)
            {
                if (!TutorialManager.TaskComplete(3, transform.position.x > 0f))
                {
                    return;
                }

            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            playerID = other.GetComponent<PlayerMovementController>().playerID;
            playerTrigger = true;
            if (!keyDown && InputSystemManager.GetAction2(playerID))
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
