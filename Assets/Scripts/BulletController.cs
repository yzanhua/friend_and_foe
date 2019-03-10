using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: use a bullet pool later
public class BulletController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float LongestDistance = 9.0f;
    public Vector3 direction; 
    private Rigidbody rb;
    private Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originPos = transform.position;
        //print(direction.normalized);
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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Submarine"))
        {
            Destroy(gameObject);
        }
    }

}
