using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    bool inWaitRoutine = false;

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
            // Debug.Log("Submarine");
            CameraShakeEffect.ShakeCamera(0.2f, 0.5f);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("collide");
            //Debug.Log(collision.relativeVelocity);
            //other.transform.position -= (transform.position - other.transform.position).normalized;
            //GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position).normalized * 400000, ForceMode2D.Impulse);
            //GetComponent<HealthCounter>().AlterHealth(-10);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Submarine") || other.CompareTag("Fish") || other.CompareTag("Weapon"))
        {
            if (!inWaitRoutine)
                StartCoroutine(waitForAlterHealth());
        }
    }

    IEnumerator waitForAlterHealth() {
        inWaitRoutine = true;
        GetComponent<HealthCounter>().AlterHealth(-1);
        yield return new WaitForSeconds(1);
        inWaitRoutine = false;
    }

}
