using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: use a bullet pool later
public class BulletController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float LongestDistance = 9.0f;
    public Vector3 direction;
    public PhysicsMaterial2D bounceMaterial;
    public PhysicsMaterial2D noBounceMaterial;

    private Rigidbody2D rb;
    private Vector3 originPos;
    private int hitCount;
    private Animator animator;

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
        }
        else
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        GetComponent<BoxCollider2D>().sharedMaterial = noBounceMaterial;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
