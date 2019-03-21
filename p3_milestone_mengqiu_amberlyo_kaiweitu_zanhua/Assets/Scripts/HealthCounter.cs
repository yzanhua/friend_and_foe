using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public int maxHealth = 100;

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }


    public void AlterHealth(int alt)
    {
        health += alt;
    }
}
