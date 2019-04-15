using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// credit: https://gist.github.com/GuilleUCM/d882e228d93c7f7d0820

public class SubmarineController : MonoBehaviour
{
    public int ID;
    public float bumpForce = 15f;
    public int playerID1 = 0;
    public int playerID2 = 2;
    public GameObject sparkParticle;
    public GameObject shieldBounceParticle;
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
            myHealth.AlterHealth(-3f);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("hit");
            InputSystemManager.SetVibration(playerID1, 0.3f, 0.2f);
            InputSystemManager.SetVibration(playerID2, 0.3f, 0.2f);
        }

        Collider2D thisCollider = collision.otherCollider;

        if ((other.CompareTag("Submarine") || other.CompareTag("Weapon")) && !thisCollider.tag.Contains("Shield"))
        { // if no shiled involved
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("collide");
            Vector2 direction = ((Vector2)(transform.position - other.transform.position)).normalized;
            rb2d.velocity = Vector2.zero;
            Vector3 contactPoint = collision.GetContact(0).point;

            // myHealth.AlterHealth(-myHealth.maxHealth * 0.05f);
            myHealth.AlterHealth(-8f);
            rb2d.AddForce(direction * bumpForce * rb2d.mass, ForceMode2D.Impulse);
            GameObject spark = Instantiate(sparkParticle, transform);
            spark.transform.position = contactPoint;
            Destroy(spark, 2f);

            InputSystemManager.SetVibration(playerID1, 0.7f, 0.3f);
            InputSystemManager.SetVibration(playerID2, 0.7f, 0.3f);
            shake();
        }
        else if (other.CompareTag("Shield"))
        {
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound("collide");

            Vector2 direction = ((Vector2)(transform.position - other.transform.position)).normalized;
            rb2d.velocity = Vector2.zero;
            Vector3 contactPoint = collision.GetContact(0).point;

            if (!thisCollider.tag.Contains("Shield"))
                //myHealth.AlterHealth(-myHealth.maxHealth * 0.15f);
                myHealth.AlterHealth(-12f);
            

            float angle = -Vector2.SignedAngle(Vector2.up, direction);
            GameObject bounceEffect = Instantiate(shieldBounceParticle, transform);
            bounceEffect.transform.position = contactPoint;
            SetParticleRotation(bounceEffect, angle);
            Destroy(bounceEffect, 1f);
            rb2d.AddForce(direction * bumpForce * rb2d.mass * 2f, ForceMode2D.Impulse);

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

    private IEnumerator waitForAlterHealth()
    {
        inWaitRoutine = true;
        myHealth.AlterHealth(-0.5f);
        yield return new WaitForSeconds(1);
        inWaitRoutine = false;
    }

    private void SetParticleRotation(GameObject particle, float angle)
    {
        var sh = particle.GetComponent<ParticleSystem>().shape;
        sh.rotation = new Vector3(sh.rotation.x, angle, sh.rotation.z);
        for (int i = 0; i < particle.transform.childCount; ++i)
        {
            sh = particle.transform.GetChild(i).GetComponent<ParticleSystem>().shape;
            sh.rotation = new Vector3(sh.rotation.x, angle, sh.rotation.z);
        }
    }

}
