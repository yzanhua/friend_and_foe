//code credit: https://answers.unity.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float LongestDistance = 9.0f;
    public Vector3 direction;
    public PhysicsMaterial2D bounceMaterial;
    public PhysicsMaterial2D noBounceMaterial;
    public GameObject laserTrail;
    public GameObject explosionParticle;

    private Rigidbody2D rb;
    private Vector3 originPos;
    private int hitCount;
    private Animator animator;
    private bool inExplosion;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        originPos = transform.position;
        rb.velocity = direction.normalized * Speed;
        hitCount = 0;
        GetComponent<BoxCollider2D>().sharedMaterial = bounceMaterial;
        inExplosion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs((transform.position - originPos).magnitude) > LongestDistance)
        {
            Destroy(gameObject);
        }
        if (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y) <= 0.1)
        {
            StartCoroutine(Explode());
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Shield") && hitCount == 0)
        {
            GetComponent<BoxCollider2D>().sharedMaterial = bounceMaterial;
            hitCount += 1;
            StartCoroutine(ChangeRotation());
        }
        else
        {
            if (other.CompareTag("Submarine"))
            {
                collision.collider.GetComponent<HealthCounter>().AlterHealth(-1);
            }
            if (other.CompareTag("Fish"))
            {
                // Deactivate the fish
                other.transform.parent.GetComponent<SchoolMovement>().KillFish();
                other.SetActive(false);
            }
            StartCoroutine(Explode());
        }
    }

    IEnumerator ChangeRotation()
    {
        yield return new WaitForSeconds(0.05f);
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 180;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    IEnumerator Explode()
    {
        if (inExplosion)
        {
            yield break;
        }
        inExplosion = true;
        laserTrail.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = null;
        //animator.SetTrigger("Explode");
        animator.enabled = false;
        GetComponent<BoxCollider2D>().sharedMaterial = noBounceMaterial;
        rb.velocity = Vector2.zero;
        GameObject exp = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        exp.transform.localScale = new Vector2(1.5f, 1.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(exp);
        Destroy(gameObject);
    }
}
