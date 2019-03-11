﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Bullet"))
        {
            GetComponent<HealthCounter>().AlterHealth(-5);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;

        if (other.CompareTag("Submarine"))
        {
            // Debug.Log("Submarine");
            CameraShakeEffect.ShakeCamera(0.2f, 0.5f);
            //Debug.Log(collision.relativeVelocity);
            // other.transform.position -= (transform.position - other.transform.position).normalized;
            //GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position).normalized * 400000, ForceMode2D.Impulse);
            //GetComponent<HealthCounter>().AlterHealth(-10);
        }
    }
}
