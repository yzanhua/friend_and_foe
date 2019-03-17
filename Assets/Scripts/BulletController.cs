using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: use a bullet pool later
public class BulletController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float LongestDistance = 9.0f;
    public Vector3 direction; 
    private Rigidbody2D rb;
    private Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originPos = transform.position;
        rb.velocity = direction.normalized * Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs((transform.position - originPos).magnitude) > LongestDistance)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;

        if (other.CompareTag("Submarine") || other.CompareTag("Edge") || other.CompareTag("Fish"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        print(other.name);
        if (other.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }

}
