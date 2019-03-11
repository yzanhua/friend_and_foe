﻿using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    [Range(0f, 10f)]
    public float speed = 2f;

    private int _ladderLayer;

    private PlayerInputController _inputController;

    private Rigidbody2D _rb2d;
 
    private float _initGravityScale;

    private Vector2 _target;

    private Animator _an;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _ladderLayer = LayerMask.NameToLayer("Ladder");
        _initGravityScale = _rb2d.gravityScale;
        _an = GetComponent<Animator>();
        _inputController = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        if (_inputController.inputDevice == null)
        {
            _an.speed = 0f;
            return;
        }

        //movement for character and for climbing ladder
        float verticalInput = _inputController.inputDevice.LeftStickY;
        float horizontalInput = _inputController.inputDevice.LeftStickX;
        bool canClimb = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, 1 << _ladderLayer);
        bool canDown = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, 1 << _ladderLayer);

        if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
        {
            horizontalInput = 0f;
        }
        else
        {
            verticalInput = 0f;
        }
        _an.speed = 1f;
        _an.SetFloat("vertical", verticalInput);
        _an.SetFloat("horizontal", horizontalInput);

        if (verticalInput > 0 && canClimb)
        {
            _rb2d.velocity = speed * new Vector2(horizontalInput, verticalInput);
        }
        else if (verticalInput < 0 && canDown)
        {
            _rb2d.velocity = speed * new Vector2(horizontalInput, verticalInput);
        }
        else 
        {
            _rb2d.velocity = speed * new Vector2(horizontalInput, 0);
            if (_rb2d.velocity == Vector2.zero)
            {
                _an.speed = 0f;
            }
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