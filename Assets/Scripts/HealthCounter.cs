using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public GameObject MyChargeBar;
    public CracksController cracksController;
    public GameObject laserReadySign;

    private float currentCharge;
    private int submarine_id;
    private bool laserNotFinished = false;

    public bool readyToShootLaser
    {
        get
        {
            return currentCharge >= 1f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        currentCharge = 0;
        submarine_id = GetComponent<SubmarineController>().ID;
        SetChargeBar(0f);
        laserReadySign.SetActive(false);
    }


    public void AlterHealth(float alt)
    {
        if (Global.instance.godMode)
            return;

        health += alt;
        if (alt <= 0)
            SetChargeBar(-alt);

        if (health <= 0)
        {
            health = 0;

            if (TutorialManager.instance != null)
            {
                health = maxHealth * 0.1f;
            }
        }

        cracksController.UpdateCracks(health / maxHealth);
    }

    public void SetChargeBar(float alt)
    {
        if (laserNotFinished || MyChargeBar == null)
            return;

        currentCharge += alt / (maxHealth * 0.3f);
        if (currentCharge >= 1f)
        {
            currentCharge = 1f;
            laserReadySign.SetActive(true);
            StartCoroutine(FlashEffect());
            laserNotFinished = true;
        }
        
        MyChargeBar.transform.localScale = new Vector3(currentCharge, 1f);
    }

    public void ResetChargeBar()
    {
        StartCoroutine(ResetSize());
    }

    IEnumerator ResetSize()
    {
        float temp = 0;
        while (temp <= 1f)
        {
            temp += Time.deltaTime / 4.0f;
            MyChargeBar.transform.localScale = new Vector3( 1 - temp, 1f);
            yield return null;
        }
        currentCharge = 0f;
        MyChargeBar.transform.localScale = new Vector3(0f, 1f);
        laserNotFinished = false;
        laserReadySign.SetActive(false);
    }

    IEnumerator FlashEffect()
    {
        bool temp = true;
        while(currentCharge >= 1f && !Global.instance.ExtraSkillEnable[submarine_id])
        {
            temp = !temp;
            MyChargeBar.SetActive(temp);
            yield return new WaitForSeconds(0.3f);
        }
        MyChargeBar.SetActive(true);
    }
}
