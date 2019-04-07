using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEffect : MonoBehaviour
{
    private SpriteRenderer _sr;


    private bool _increase = false;
    private float speed = 4f;
    private bool isFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        StartCoroutine(RotateSelf());
    }

    IEnumerator RotateSelf()
    {
        yield return new WaitForSeconds(2f);
        isFlashing = true;
    }

    private void Update()
    {
        if (!isFlashing)
            transform.Rotate(Vector3.forward, Time.deltaTime * 20f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFlashing)
        {
            return;
        }

        speed += Time.fixedDeltaTime * 2;
        if (!_increase)
        {
            Color c = _sr.color;
            c.a = _sr.color.a - Time.fixedDeltaTime * speed;
            _sr.color = c;

            if (c.a < 0.1f)
            {
                _increase = true;
            }
        }
        else
        {
            Color c = _sr.color;
            c.a = _sr.color.a + Time.fixedDeltaTime * speed;
            _sr.color = c;

            if (c.a > 0.9f)
            {
                _increase = false;
            }

        }
    }
}
