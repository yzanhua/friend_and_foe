using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float bumpForce = 15f;
    bool inWaitRoutine = false;
    HealthCounter myHealth;
    Rigidbody2D rb2d;

    private void Start()
    {
        myHealth = GetComponent<HealthCounter>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Bullet"))
        {
            myHealth.AlterHealth(-2f);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("hit");
        }
        if (other.CompareTag("Submarine"))
        {
            CameraShakeEffect.ShakeCamera(0.2f, 0.5f);
            if (!collision.otherCollider.CompareTag("Weapon") && !collision.otherCollider.CompareTag("Shield"))
                myHealth.AlterHealth(-2f);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("collide");
        }
        if (other.CompareTag("Submarine") || other.CompareTag("Weapon") || other.CompareTag("Shield"))
        {
            Vector2 direction = ((Vector2)(transform.position - other.transform.position)).normalized;
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(direction * bumpForce * rb2d.mass, ForceMode2D.Impulse);
            InputSystemManager.SetVibration(-1, 0.6f, 0.3f);
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

    IEnumerator waitForAlterHealth() {
        inWaitRoutine = true;
        myHealth.AlterHealth(-0.5f);
        yield return new WaitForSeconds(1);
        inWaitRoutine = false;
    }

}
