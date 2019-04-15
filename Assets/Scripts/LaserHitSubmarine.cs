using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitSubmarine : MonoBehaviour
{
    private GameObject submarine = null;
    private HealthCounter health = null;
    private SubmarineController subController = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("Submarine"))
        {
            submarine = collision.gameObject;
            health = submarine.GetComponent<HealthCounter>();
            subController = submarine.GetComponent<SubmarineController>();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (submarine == collision.gameObject)
        {
            submarine = null;
            health = null;
            subController = null;
        }
    }

    private void FixedUpdate()
    {
        if (submarine == null)
            return;
        health.AlterHealth(-0.2f);
        InputSystemManager.SetVibrationBySubmarine(subController.ID, 0.7f, Time.fixedDeltaTime);                                           
    }

}
