using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEffect : MonoBehaviour
{
    private SpriteRenderer _sr;


    private bool _increase = false;
    private float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
