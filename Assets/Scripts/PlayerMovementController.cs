using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D _rb2d;
    PlayerInputController _inputController;
    int _ladderLayer;
    float _initGravityScale;
    Vector2 _target;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _ladderLayer = LayerMask.NameToLayer("Ladder");
        _initGravityScale = _rb2d.gravityScale;
        _inputController = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        if (_inputController.inputDevice == null)
        {
            return;
        }

        //movement for character and for climbing ladder
        float verticalInput = _inputController.inputDevice.LeftStickY;
        float horizontalInput = _inputController.inputDevice.LeftStickX;
        bool canClimb = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, 1 << _ladderLayer);
        bool canDown = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, 1 << _ladderLayer);

        if (verticalInput > 0 && canClimb)
        {
            _rb2d.velocity = new Vector2(horizontalInput, verticalInput);
        }
        else if (verticalInput < 0 && canDown)
        {
            _rb2d.velocity = new Vector2(horizontalInput, verticalInput);
        }
        else
        {
            _rb2d.velocity = new Vector2(horizontalInput, 0);

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
