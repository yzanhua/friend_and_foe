using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCharacterMovement : MonoBehaviour
{
    Rigidbody2D _rb2d;
    int _ladderLayer;
    float _initGravityScale;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _ladderLayer = LayerMask.NameToLayer("Ladder");
        _initGravityScale = _rb2d.gravityScale;
    }

    void Update()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        //movement for character and for climbing ladder
        bool canClimb = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, 1 << _ladderLayer);
        bool canDown = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, 1 << _ladderLayer);

        if (vertical_input > 0 && canClimb)
        {
            _rb2d.velocity = new Vector2(horizontal_input, vertical_input);
        }
        else if (vertical_input < 0 && canDown)
        {
            _rb2d.velocity = new Vector2(horizontal_input, vertical_input);
        }
        else
        {
            _rb2d.velocity = new Vector2(horizontal_input, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rb2d.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rb2d.gravityScale = _initGravityScale;
        }
    }
}
