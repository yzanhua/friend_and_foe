using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillController : MonoBehaviour
{
    public float refillTime = 2.0f;
    public GameObject weapon;
    public GameObject progress_bar;
    public GameObject refillStation;
    public Sprite activeSprite;

    public bool bulletFull = false;

    private int playerID = -1;
    private float curFilledTime = 0;
    private SpriteRenderer refillRend;
    private Sprite inactiveSprite;
    private HealthBar healthbar_of_progggressbar;


    void Start()
    {
        progress_bar.SetActive(false);
        refillRend = refillStation.GetComponent<SpriteRenderer>();
        inactiveSprite = refillRend.GetComponent<SpriteRenderer>().sprite;
        healthbar_of_progggressbar = progress_bar.GetComponent<HealthBar>();
    }

    void Update()
    {
        if (playerID < 0)
            return;

        if (bulletFull)
        {
            if (InputSystemManager.GetAction2(playerID))
                InputSystemManager.SetVibration(playerID, 0.3f, 0.3f);
            return;
        }

        if (InputSystemManager.GetAction2Hold(playerID))
        {
            if (curFilledTime <= 0)
                progress_bar.SetActive(true);
            
            curFilledTime += Time.deltaTime;
            healthbar_of_progggressbar.SetSize((float)curFilledTime / (float)refillTime);
            refillRend.sprite = activeSprite;
            //refillRend.color = new Color(refillRend.color.r, refillRend.color.g, refillRend.color.b, 1.0f);
        }
        else
        {
            refillRend.sprite = inactiveSprite;
            //refillRend.color = new Color(refillRend.color.r, refillRend.color.g, refillRend.color.b, 0.3f);
        }

        //finished filling
        if (curFilledTime >= refillTime)
        {
            curFilledTime = 0;
            weapon.GetComponent<WeaponController>().FillBullets();
            progress_bar.SetActive(false);
            bulletFull = true;
            if (TutorialManager.instance != null)
                TutorialManager.CompleteTask(TutorialManager.TaskType.REFILL, transform.position.x > 0f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player") && playerID == -1)
            playerID = other.GetComponent<PlayerMovementController>().playerID;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player") && playerID == other.GetComponent<PlayerMovementController>().playerID)
            playerID = -1;          
    }

    public void SetBulletStatus(bool status)
    {
        bulletFull = status;
    }
}
