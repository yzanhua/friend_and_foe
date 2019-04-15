using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public float maxHealth = 100f;

    public float health = 100f;

    public GameObject MyChargeBar;
    private float currentCharge;
    private int submarine_id;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        currentCharge = 0;
        submarine_id = GetComponent<SubmarineController>().ID;
    }


    public void AlterHealth(float alt)
    {
        if (Global.instance.godMode)
            return;

        health += alt;
        if (alt < 0)
            SetChargeBar(-alt);
        if (health < 0)
        {
            health = 0;
        }
    }

    private void SetChargeBar(float alt)
    {
        currentCharge += alt / (maxHealth * 0.3f);
        if (currentCharge >= 1f)
        {
            currentCharge = 1f;
            Global.instance.ChargeBarFull[submarine_id] = true;
        }
            
        MyChargeBar.transform.localScale = new Vector3(currentCharge, 1f);
    }
}
