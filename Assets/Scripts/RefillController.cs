using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillController : MonoBehaviour
{
    public float refillTime = 2.0f;
    public GameObject weapon;
    public GameObject progress_bar;
    public GameObject refillStation;

    // whether key was pressed in this collision period
    public bool keyDown = false;
    public bool playerTrigger = false;
    public bool bulletFull = false;

    int playerID;
    float curFilledTime = 0;
    SpriteRenderer refillRend;


    void Start()
    {
        progress_bar.SetActive(false);

    }

    void Update()
    {
        if (bulletFull || !playerTrigger)
        {
            keyDown = false;
        }
        else if (playerTrigger && InputSystemManager.GetAction2(playerID))
        {
            keyDown = true;
        }

        if (keyDown)
        {
            if (curFilledTime <= 0)
            {
                progress_bar.SetActive(true);
            }
            curFilledTime += Time.deltaTime;
            progress_bar.GetComponent<HealthBar>().SetSize((float)curFilledTime / (float)refillTime);
            refillRend.color = new Color(refillRend.color.r, refillRend.color.g, refillRend.color.b, 1.0f);
        }
        else
        {
            refillRend.color = new Color(refillRend.color.r, refillRend.color.g, refillRend.color.b, 0.3f);
        }

        //finished filling
        if (curFilledTime >= refillTime)
        {
            curFilledTime = 0;
            keyDown = false;
            weapon.GetComponent<WeaponController>().FillBullets();
            progress_bar.SetActive(false);
            bulletFull = true;
            if (TutorialManager.instance != null && TutorialManager.instance.tutorialMode)
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
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;
        }

    }

    public void SetBulletStatus(bool status)
    {
        bulletFull = status;
        refillRend = refillStation.GetComponent<SpriteRenderer>();
    }
}
