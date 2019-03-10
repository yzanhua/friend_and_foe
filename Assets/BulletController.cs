using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: use a bullet pool later
public class BulletController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float LongestDistance = 9.0f;
    private Rigidbody rb;
    private Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originPos = transform.position;
        //Vector3 direction = new Vector3(-1, 0, 0);
        Vector3 direction = transform.rotation * -transform.right;
        print(direction);
        //Vector3 direction = transform.forward;
        rb.velocity = direction * Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs((transform.position - originPos).magnitude) > LongestDistance)
        {
            Destroy(gameObject);
        }
    }

    //public void Shoot()
    //{
    //    Vector3 direction = new Vector3(-1, 0, 0);
    //    rb.velocity = direction * Speed;
    //}
}
