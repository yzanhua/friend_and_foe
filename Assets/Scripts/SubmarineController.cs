using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public int ID;
    public float bumpForce = 15f;
    public int playerID1 = 0;
    public int playerID2 = 2;
    bool inWaitRoutine = false;
    HealthCounter myHealth;
    Rigidbody2D rb2d;

    // shake effect
    private Quaternion originRotation;
    private float temp_shake_intensity = 0f;
    public float shake_decay = 0.004f;
    public float shake_intensity = .2f;

    private void Start()
    {
        myHealth = GetComponent<HealthCounter>();
        rb2d = GetComponent<Rigidbody2D>();
        originRotation = transform.rotation;
    }

    public void shake(float shake_intensity_ = -1f)
    {
        if (shake_intensity_ < 0f) shake_intensity_ = shake_intensity;
        temp_shake_intensity = shake_intensity_;
    }

    private void Update()
    {
        if (temp_shake_intensity > 0)
        {
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
            temp_shake_intensity -= shake_decay;
            if (temp_shake_intensity <= 0)
                transform.rotation = originRotation;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Bullet"))
        {
            myHealth.AlterHealth(-2f);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("hit");
            InputSystemManager.SetVibration(playerID1, 0.3f, 0.2f);
            InputSystemManager.SetVibration(playerID2, 0.3f, 0.2f);
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
            InputSystemManager.SetVibration(playerID1, 0.7f, 0.3f);
            InputSystemManager.SetVibration(playerID2, 0.7f, 0.3f);
            shake();
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
