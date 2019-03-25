using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    bool inWaitRoutine = false;
    bool bombTriggered = false;
    HealthCounter myHealth;

    private void Start()
    {
        myHealth = GetComponent<HealthCounter>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Bullet"))
        {
            GetComponent<HealthCounter>().AlterHealth(-5);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("hit");
        }
        if (other.CompareTag("Submarine"))
        {
            CameraShakeEffect.ShakeCamera(0.2f, 0.5f);
            GetComponent<HealthCounter>().AlterHealth(-5);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("collide");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Fish"))
        {
            if (!inWaitRoutine)
                StartCoroutine(waitForAlterHealth());
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bomb"))
            return;
        bombTriggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bomb"))
            return;
        bombTriggered = false;
    }

    private void Update()
    {
        if (bombTriggered)
            myHealth.AlterHealth(-0.3f);
    }

    IEnumerator waitForAlterHealth() {
        inWaitRoutine = true;
        GetComponent<HealthCounter>().AlterHealth(-1);
        yield return new WaitForSeconds(1);
        inWaitRoutine = false;
    }

}
