using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public float maxHealth = 100f;

    public float health = 100f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }


    public void AlterHealth(float alt)
    {
        if (Global.instance.godMode)
            return;

        health += alt;
        if (health < 0)
        {
            health = 0;
        }
    }
}
